using teleferic_core_domain.Entities;

namespace teleferic_core_domain.Interfaces.Repository
{
    public interface IInMemoryRepository
    {
        Task<Basket?> GetBasketAsync(string basketId);
        Task<Basket?> UpdateBasketAsync(Basket basket);
        Task<bool> DeleteBasketAsync(string basketId);

    }
}
