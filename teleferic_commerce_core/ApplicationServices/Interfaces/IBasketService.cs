using teleferic_commerce_core.DTO.Basket;
using teleferic_commerce_infrastructure.Models;
using teleferic_core_domain.Entities;

namespace teleferic_commerce_core.ApplicationServices.Interfaces
{
    public interface IBasketService
    {
        Task<ResponseModel<BasketDTO?>> GetBasketAsync(string basketId);
        Task<ResponseModel<BasketDTO?>> UpdateBasketAsync(string basketId,AddItemToBasketDto basketDto);
        Task<ResponseModel<bool>> DeleteBasketAsync(string basketId);

        Task<ResponseModel<BasketDTO>> RemoveItemFromBasket(string basketId, Guid productId);
    }
}
