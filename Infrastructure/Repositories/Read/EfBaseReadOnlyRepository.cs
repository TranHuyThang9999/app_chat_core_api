using PaymentCoreServiceApi.Core.Entities.BaseModel;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace PaymentCoreServiceApi.Infrastructure.Repositories.Read
{
    public class EfBaseReadOnlyRepository<TEntity> : IBaseReadRepository<TEntity> where TEntity : EntityBase
    {
        private readonly DbSet<TEntity> _dbSet;

        public EfBaseReadOnlyRepository(AppDbContext context)
        {
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(long id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, bool>> predicate)
        {
            var query = _dbSet.AsNoTracking().Where(predicate);
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}