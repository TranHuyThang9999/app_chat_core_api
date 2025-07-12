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
