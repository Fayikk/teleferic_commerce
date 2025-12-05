using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using teleferic_core_domain.Data;
using teleferic_core_domain.Entities;
using teleferic_core_domain.Interfaces.Repository;

namespace teleferic_commerce_infrastructure.Concrete.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true)
        {
            var query = _dbSet.Where(predicate);
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = true)
        {
            var query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet;
            return await query.ToListAsync();   
        }

        public async Task<IEnumerable<T>> GetAllAsyncWithInclude(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeExpression, bool asNoTracking = true)
        {
            var query = _dbSet.Where(predicate);
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            query = includeExpression(query);
            return query.AsEnumerable();
        }

        public async Task<T?> GetByIdAsync(Guid Id, bool asNoTracking = true)
        {
            var query = _dbSet.AsQueryable();
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public Task<T?> GetByIdAsyncExpressionWithInclude(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeExpression, bool asNoTracking = true)
        {
            var query = _dbSet.Where(predicate);
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            query = includeExpression(query);
            return query.FirstOrDefaultAsync();
        }

        public async Task SoftDeleteAsync(Guid Id)
        {
            var entity = await _dbSet.FindAsync(Id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                _dbSet.Update(entity);
            }
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);  
        }
    }
}
