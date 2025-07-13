using PaymentCoreServiceApi.Core.Entities.BankAccountGenerated;

namespace PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;

public interface IBankAccountReadRepository:  IBaseReadRepository<BankAccount>
{
    Task<bool> ExistsBankAccountByUserIdAsync(long userId);
}