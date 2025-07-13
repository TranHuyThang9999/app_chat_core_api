using System.Linq.Expressions;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.IUnitOfWork;

namespace PaymentCoreServiceApi.Core.Interfaces.Repositories.Write
{
    public interface IBaseWriteOnlyRepository<TEntity> : IDbUnitOfWork where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity, string[]? excludeProperties = null);
        Task<List<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

        Task<TEntity> UpdateAsync(TEntity entity, string[]? excludeProperties = null);
        Task<List<TEntity>> UpdateBatchAsync(IEnumerable<TEntity> entities, string[]? excludeProperties = null);

        Task<TEntity> DeleteAsync(TEntity entity);
        Task<List<TEntity>> DeleteBatchAsync(IEnumerable<TEntity> entities);

        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate);

    }
}