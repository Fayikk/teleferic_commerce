using teleferic_commerce_infrastructure.Concrete.Repository;
using teleferic_core_domain.Data;
using teleferic_core_domain.Entities;
using teleferic_core_domain.Interfaces.Repository;

namespace teleferic_commerce_infrastructure.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IGenericRepository<Product> Products { get; }

        public IGenericRepository<Category> Categories { get; }

        public IGenericRepository<Address> Addresses { get; }

        public IGenericRepository<Order> Orders { get; }

        public IGenericRepository<OrderItem> OrderItems { get; }

        public IGenericRepository<ProductImage> ProductImages { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Products = new GenericRepository<Product>(_context);
            Categories = new GenericRepository<Category>(_context);
            Addresses = new GenericRepository<Address>(_context);
            Orders = new GenericRepository<Order>(_context);
            OrderItems = new GenericRepository<OrderItem>(_context);
            ProductImages = new GenericRepository<ProductImage>(_context);
        }


        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
