using System.Linq.Expressions;
using PaymentCoreServiceApi.Core.Entities.BaseModel;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.IUnitOfWork;
using PaymentCoreServiceApi.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace PaymentCoreServiceApi.Infrastructure.Repositories.Write;

public class EfBaseWriteOnlyRepository<TEntity>(AppDbContext context) : IBaseWriteOnlyRepository<TEntity>, IDisposable
    where TEntity : EntityBase
{
    private readonly AppDbContext Context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly DbSet<TEntity> DbSet = context.Set<TEntity>();

    public async Task<TEntity> AddAsync(TEntity entity, string[]? excludeProperties = null)
    {
        await DbSet.AddAsync(entity);
        return entity;
    }

    public async Task<List<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
    {
        var entityBases = entities as TEntity[] ?? entities.ToArray();
        await DbSet.AddRangeAsync(entityBases);
        return entityBases.ToList();
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, string[]? excludeProperties = null)
    {
        DbSet.Update(entity);
        return entity;
    }

    public async Task<List<TEntity>> UpdateBatchAsync(IEnumerable<TEntity> entities, string[]? excludeProperties = null)
    {
        var entityBases = entities as TEntity[] ?? entities.ToArray();
        DbSet.UpdateRange(entityBases);
        return entityBases.ToList();
    }

    public async Task<TEntity> DeleteAsync(TEntity entity)
    {
        DbSet.Remove(entity);
        return entity;
    }

    public async Task<List<TEntity>> DeleteBatchAsync(IEnumerable<TEntity> entities)
    {
        IEnumerable<TEntity> entityBases = entities.ToList();
        DbSet.RemoveRange(entityBases);
        return entityBases.ToList();
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await DbSet.FirstAsync(@base => id == @base.Id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate)
    {
        var query = DbSet.Where(predicate);
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
// Trong một phương thức xử lý
    //public async Task<IActionResult> SearchOrders(OrderSearchModel searchModel)
    // {
    //     // Tạo predicate phức tạp
    //     Expression<Func<Order, bool>> predicate = o => true;
    // 
    //     if (searchModel.StartDate.HasValue && searchModel.EndDate.HasValue)
    //     {
    //         predicate = o => o.OrderDate >= searchModel.StartDate.Value && 
    //                          o.OrderDate <= searchModel.EndDate.Value;
    //     }
    // 
    //     if (searchModel.MinAmount.HasValue)
    //     {
    //         var tempPredicate = predicate;
    //         predicate = o => tempPredicate.Compile()(o) && o.TotalAmount >= searchModel.MinAmount.Value;
    //     }
    // 
    //     if (!string.IsNullOrEmpty(searchModel.CustomerName))
    //     {
    //         var tempPredicate = predicate;
    //         predicate = o => tempPredicate.Compile()(o) && 
    //                          o.Customer.Name.Contains(searchModel.CustomerName);
    //     }
    // 
    //     if (searchModel.Status.HasValue)
    //     {
    //         var tempPredicate = predicate;
    //         predicate = o => tempPredicate.Compile()(o) && o.Status == searchModel.Status.Value;
    //     }
    // 
    //     // Gọi phương thức phân trang
    //     var (orders, totalCount) = await _orderRepository.GetPagedAsync(
    //         searchModel.Page, 
    //         searchModel.PageSize, 
    //         predicate
    //     );
    // 
    //     // Xử lý kết quả...
    // }
    // Class chứa kết quả phân trang
    //    public class PagedResult<T>
    // {
    //     public List<T> Items { get; set; } = new List<T>();
    //     public int TotalCount { get; set; }
    //     public int PageIndex { get; set; }
    //     public int PageSize { get; set; }
    //     public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    //     public bool HasPreviousPage => PageIndex > 0;
    //     public bool HasNextPage => PageIndex < TotalPages - 1;
    // }

    public void Dispose()
    {
        Context.Dispose();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await Context.SaveChangesAsync(cancellationToken);
    }

    public void Commit()
    {
        Context.SaveChanges();
    }
}
