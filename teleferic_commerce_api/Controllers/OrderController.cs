using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using teleferic_commerce_core.ApplicationServices.Interfaces;
using teleferic_commerce_core.DTO.Order;

namespace teleferic_commerce_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDTO)
        {
            var result = await orderService.CreateOrder(orderDTO);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return CreatedAtAction(nameof(GetOrderById),new {id=result.Data.Id},result.Data);
        }



        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await orderService.GetOrders();
            if (!orders.IsSuccess)
                return NotFound(orders.Message);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await orderService.GetOrderById(id);
            if (!order.IsSuccess)
                return NotFound(order.Message);
            return Ok(order);
        }
    }
}
