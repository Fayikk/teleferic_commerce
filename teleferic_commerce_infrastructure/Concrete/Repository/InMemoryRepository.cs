using StackExchange.Redis;
using teleferic_core_domain.Entities;
using teleferic_core_domain.Interfaces.Repository;

namespace teleferic_commerce_infrastructure.Concrete.Repository
{
    public class InMemoryRepository : IInMemoryRepository
    {
        private readonly IDatabase _database;
        public InMemoryRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<Basket?> GetBasketAsync(string basketId)
        {
            var data = await _database.StringGetAsync(basketId);
            if (data.IsNullOrEmpty)
            {
                return null;
            }
            return System.Text.Json.JsonSerializer.Deserialize<Basket>(data);
        }

        public async Task<Basket?> UpdateBasketAsync(Basket basket)
        {
            var created = await _database.StringSetAsync(basket.Id, System.Text.Json.JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
            if (!created)
            {
                return null;
            }
            return await GetBasketAsync(basket.Id);
        }
    }
}
