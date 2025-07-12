namespace PaymentCoreServiceApi.Core.Interfaces.Repositories.IUnitOfWork;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}