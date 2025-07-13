using System.Linq.Expressions;

namespace PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;

public interface IBaseReadRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate);
}