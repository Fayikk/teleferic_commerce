using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using teleferic_commerce_core.ApplicationServices.Interfaces;
using teleferic_commerce_core.DTO.Order;
using teleferic_commerce_infrastructure.Models;
using teleferic_commerce_infrastructure.PaymentService;
using teleferic_commerce_infrastructure.UoW;
using teleferic_core_domain.Entities;

namespace teleferic_commerce_core.ApplicationServices.Concretes
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IPaymentService paymentService;
        private readonly string UserId;
        private readonly UserManager<ApplicationUser> userManager;
        public OrderService(IUnitOfWork unitOfWork,IMapper mapper,UserManager<ApplicationUser> userManager,IHttpContextAccessor httpContextAccessor,IPaymentService paymentService)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.paymentService = paymentService;
            UserId = httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<ResponseModel<OrderDto>> CreateOrder(CreateOrderDto createOrderDto)
        {
            var user = await userManager.FindByIdAsync(UserId);
            if (user is null)
            {
                return new ResponseModel<OrderDto>
                {
                    IsSuccess = false,
                    Message = "User not found."
                };
            }

            var adress = await unitOfWork.Addresses.GetByIdAsync(createOrderDto.AddressId);
            if (adress is null || adress.UserId != UserId)
            {
                return new ResponseModel<OrderDto>
                {
                    IsSuccess = false,
                    Message = "Address not found."
                };
            }

            var productIds = createOrderDto.Items.Select(i => i.ProductId).ToList();
            var products = await unitOfWork.Products.FindAsync(
                predicate: x => productIds.Contains(x.Id)
                );
            if (products.Count() != productIds.Count)
            {
                return new ResponseModel<OrderDto>
                {
                    IsSuccess = false,
                    Message = "One or more products not found."
                };
            }
            foreach (var item in createOrderDto.Items)
            {
                var product = products.First(x => x.Id == item.ProductId);
                if (item.Quantity > product.Stock)
                {
                    return new ResponseModel<OrderDto>
                    {
                        IsSuccess = false,
                        Message = $"Insufficient stock for product {product.Name}."
                    };
                }
            }

            //total amount

            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();
            foreach (var item in createOrderDto.Items)
            {
                var product = products.First(x => x.Id == item.ProductId);
                totalAmount += product.Price * item.Quantity;
                orderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name, // ✅ Ekleyin
                    Quantity = item.Quantity,
                    Price = product.Price
                });
            }

            var orderNumber = $"TELEFERIC_ORD{DateTime.UtcNow:yyyyMMddHHmmss}{new Random().Next(1000, 9999)}";

            var paymentRequest = new PaymentRequest
            {
                OrderNumber = orderNumber,
                Amount = totalAmount,
                Card = new teleferic_commerce_infrastructure.PaymentService.PaymentCardDto
                {
                    CardHolderName = createOrderDto.Card.CardHolderName,
                    CardNumber = createOrderDto.Card.CardNumber,
                    ExpireMonth = createOrderDto.Card.ExpireMonth,
                    ExpireYear = createOrderDto.Card.ExpireYear,
                    Cvc = createOrderDto.Card.Cvc
                },
                Buyer = new BuyerInfo
                {
                    Id = UserId,
                    Name = user.FirstName,
                    Surname = user.LastName,
                    Email = user.Email!,
                    RegistrationAddress = adress.AddressLine,
                    City = adress.City
                },
                ShippingAddress = new AddressInfo
                {
                    ContactName = adress.FullName,
                    City = adress.City,
                    Address = adress.AddressLine
                },
                Items = orderItems.Select(oi => new BasketItemInfo
                {
                    Id = oi.ProductId.ToString(),
                    Name = oi.ProductName,
                    Price = oi.Price * oi.Quantity
                }).ToList()
            };

            var paymentResult = await paymentService.ProcessPaymentAsync(paymentRequest);

            if (!paymentResult.Success)
            {
                return new ResponseModel<OrderDto>
                {
                    IsSuccess = false,
                    Message = $"Payment failed: {paymentResult.ErrorMessage}"
                };
            }

            var order = new Order
            {
                OrderNumber = orderNumber,
                UserId = UserId,
                ShippingFullName = adress.FullName,
                ShippingPhoneNumber = adress.PhoneNumber,
                ShippingCity = adress.City,
                ShippingDistrict = adress.District,
                ShippingAddressLine = adress.AddressLine,
                ShippingZipCode = adress.ZipCode,
                TotalAmount = totalAmount,
                PaymentId = paymentResult.PaymentId,
                PaymentStatus = "Completed",
                OrderStatus = "Processing",
                OrderItems = orderItems,
            };

            unitOfWork.Orders.AddAsync(order);
            foreach (var item in createOrderDto.Items)
            {
                var product = products.First(x => x.Id == item.ProductId);
                product.Stock -= item.Quantity;
                product.UpdatedAt = DateTime.UtcNow;
            }

            await unitOfWork.SaveAsync();
            var orderDTO = await unitOfWork.Orders.GetByIdAsyncExpressionWithInclude(
                predicate: x => x.Id == order.Id,
                includeExpression: x => x.Include(y => y.OrderItems)
                );
            var mapObj = mapper.Map<OrderDto>(orderDTO);

            return new ResponseModel<OrderDto>
            {
                IsSuccess = true,
                Data = mapObj
            };

        }

        public async Task<ResponseModel<OrderDto>> GetOrderById(Guid id)
        {
            var orders = await unitOfWork.Orders.GetByIdAsyncExpressionWithInclude(
                predicate:x=>x.Id == id && x.UserId == UserId,includeExpression: x=> x.Include(y => y.OrderItems)
                );
            if (orders is null)
            {
                return new ResponseModel<OrderDto>
                {
                    IsSuccess = false,
                    Message = "Order not found."
                };
            }

            var orderDto = mapper.Map<OrderDto>(orders);
            return new ResponseModel<OrderDto>
            {
                IsSuccess = true,
                Data = orderDto
            };

        }

        public async Task<ResponseModel<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = unitOfWork.Orders.GetAllAsyncWithInclude(
                includeExpression: x => x.Include(y => y.OrderItems),
                predicate: x => x.UserId == UserId
                ).Result.OrderByDescending(x=>x.CreatedAt).ToList();
            if (orders.Count == 0)
            {
                return new ResponseModel<IEnumerable<OrderDto>>
                {
                    IsSuccess = false,
                    Message = "No orders found."
                };  
            }

            var ordersDto = mapper.Map<IEnumerable<OrderDto>>(orders);
            return new ResponseModel<IEnumerable<OrderDto>>
            {
                IsSuccess = true,
                Data = ordersDto
            };
        }
    }
}
