using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using teleferic_core_domain.Entities;

namespace teleferic_core_domain.Interfaces.Repository
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(Guid Id, bool asNoTracking = true);
        Task<T?> GetByIdAsyncExpressionWithInclude(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeExpression, bool asNoTracking = true);
        Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = true);
        Task<IEnumerable<T>> GetAllAsyncWithInclude(Expression<Func<T, bool>> predicate,Func<IQueryable<T>,IIncludableQueryable<T,object>> includeExpression,bool asNoTracking = true);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true);
        Task AddAsync(T entity);
        void Update(T entity);
        Task SoftDeleteAsync(Guid Id);

    }
}
