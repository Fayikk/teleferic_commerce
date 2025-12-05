using Microsoft.AspNetCore.Mvc;
using teleferic_commerce_core.ApplicationServices.Interfaces;
using teleferic_commerce_core.DTO.Basket;

namespace teleferic_commerce_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService basketService;

        public BasketController(IBasketService basketService)
        {
            this.basketService = basketService;
        }



        [HttpDelete("{basketId}/items/{productId}")]
        public async Task<IActionResult> RemoveItemFromUniqeBasket(string basketId, Guid productId)
        {
            var basket = await basketService.RemoveItemFromBasket(basketId, productId);
            if (!basket.IsSuccess)
                return NotFound(basket.Message);
            return Ok(basket);
        }
        [HttpGet("{basketId}")]
        public async Task<IActionResult> GetBasket(string basketId)
        {
            var basket = await basketService.GetBasketAsync(basketId);
            if (basket == null)
                return NotFound("Basket not found.");

            return Ok(basket);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateBasket(string id, AddItemToBasketDto addItemDto)
        {

            var updatedBasket = await basketService.UpdateBasketAsync(id, addItemDto);
            return updatedBasket.IsSuccess ? Ok(updatedBasket) : BadRequest(updatedBasket);
        }

        [HttpDelete("{basketId}")]
        public async Task<IActionResult> DeleteBasket(string basketId)
        {
            var result = await basketService.DeleteBasketAsync(basketId);

            if (!result.IsSuccess)
                return NotFound("Basket not found.");

            return Ok(new { message = "Basket deleted successfully." });
        }
    }
}
