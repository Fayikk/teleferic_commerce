using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Extensions.Configuration;

namespace teleferic_commerce_infrastructure.PaymentService
{
    public class IyzicoPaymentService : IPaymentService
    {
        private readonly Options _options;

        public IyzicoPaymentService(IConfiguration configuration)
        {
            _options = new Options
            {
                ApiKey = configuration["IyzicoSettings:ApiKey"]!,
                SecretKey = configuration["IyzicoSettings:SecretKey"]!,
                BaseUrl = configuration["IyzicoSettings:BaseUrl"]!
            };
        }

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            try
            {
                // Toplam sepet tutarını hesapla (Items'dan)
                decimal basketTotal = request.Items.Sum(x => x.Price);

                var paymentRequest = new CreatePaymentRequest
                {
                    Locale = Locale.TR.ToString(),
                    ConversationId = request.OrderNumber,
                    Price = basketTotal.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                    PaidPrice = basketTotal.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                    Currency = Currency.TRY.ToString(),
                    Installment = 1,
                    BasketId = request.OrderNumber,
                    PaymentChannel = PaymentChannel.WEB.ToString(),
                    PaymentGroup = PaymentGroup.PRODUCT.ToString()
                };

                // Payment Card
                var paymentCard = new PaymentCard
                {
                    CardHolderName = request.Card.CardHolderName,
                    CardNumber = request.Card.CardNumber,
                    ExpireMonth = request.Card.ExpireMonth,
                    ExpireYear = request.Card.ExpireYear,
                    Cvc = request.Card.Cvc,
                    RegisterCard = 0
                };
                paymentRequest.PaymentCard = paymentCard;

                // Buyer
                var buyer = new Buyer
                {
                    Id = request.Buyer.Id,
                    Name = request.Buyer.Name,
                    Surname = request.Buyer.Surname,
                    GsmNumber = "+905350000000",
                    Email = request.Buyer.Email,
                    IdentityNumber = request.Buyer.IdentityNumber ?? "11111111111",
                    RegistrationAddress = request.Buyer.RegistrationAddress,
                    Ip = request.Buyer.Ip ?? "85.34.78.112",
                    City = request.Buyer.City,
                    Country = request.Buyer.Country ?? "Turkey"
                };
                paymentRequest.Buyer = buyer;

                // Shipping Address
                var shippingAddress = new Address
                {
                    ContactName = request.ShippingAddress.ContactName,
                    City = request.ShippingAddress.City,
                    Country = request.ShippingAddress.Country ?? "Turkey",
                    Description = request.ShippingAddress.Address
                };
                paymentRequest.ShippingAddress = shippingAddress;

                // Billing Address (same as shipping)
                paymentRequest.BillingAddress = shippingAddress;

                // Basket Items - HER ÜRÜN İÇİN TOPLAM FİYAT GÖNDERİLMELİ
                var basketItems = new List<BasketItem>();
                foreach (var item in request.Items)
                {
                    basketItems.Add(new BasketItem
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Category1 = item.Category ?? "Product",
                        ItemType = BasketItemType.PHYSICAL.ToString(),
                        Price = item.Price.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
                    });
                }
                paymentRequest.BasketItems = basketItems;

                // Make payment request (Synchronous olarak çağrılmalı)
                var payment = await Payment.Create(paymentRequest, _options);

                if (payment.Status == "success")
                {
                    return new PaymentResult
                    {
                        Success = true,
                        PaymentId = payment.PaymentId
                    };
                }
                else
                {
                    return new PaymentResult
                    {
                        Success = false,
                        ErrorMessage = payment.ErrorMessage ?? "Payment failed"
                    };
                }
            }
            catch (Exception ex)
            {
                return new PaymentResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}