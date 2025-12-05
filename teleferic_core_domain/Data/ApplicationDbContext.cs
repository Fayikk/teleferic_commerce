using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;
using teleferic_core_domain.Entities;

namespace teleferic_core_domain.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Address> Addresses
        {
            get; set;
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(CreateSoftDelete(entityType.ClrType));
                }
            }
            base.OnModelCreating(modelBuilder);
            // Configure relationships and constraints here if needed
        }

        private LambdaExpression? CreateSoftDelete(Type clrType)
        {
            var parameter = Expression.Parameter(clrType, "e");
            var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
            var condition = Expression.Equal(property, Expression.Constant(false));
            var lambda = Expression.Lambda(condition, parameter);
            return lambda;
        }
    }

    
}
