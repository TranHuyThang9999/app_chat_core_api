namespace PaymentCoreServiceApi.Core.Interfaces.Repositories.IUnitOfWork;

public interface IDbUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
    void Commit();
}