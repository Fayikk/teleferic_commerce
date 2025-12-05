using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teleferic_core_domain.Entities;
using teleferic_core_domain.Interfaces.Repository;

namespace teleferic_commerce_infrastructure.UoW
{
    public interface IUnitOfWork
    {
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<Address> Addresses { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<OrderItem> OrderItems { get; }
        IGenericRepository<ProductImage> ProductImages { get; }   

        Task<int> SaveAsync();
    }
}
