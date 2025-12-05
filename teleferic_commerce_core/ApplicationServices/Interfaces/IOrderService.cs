using teleferic_commerce_core.DTO.Order;
using teleferic_commerce_infrastructure.Models;

namespace teleferic_commerce_core.ApplicationServices.Interfaces
{
    public interface IOrderService
    {
        Task<ResponseModel<IEnumerable<OrderDto>>> GetOrders();
        Task<ResponseModel<OrderDto>> GetOrderById(Guid id);

        Task<ResponseModel<OrderDto>> CreateOrder(CreateOrderDto createOrderDto);
    }
}
