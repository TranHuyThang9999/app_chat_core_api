using PaymentCoreServiceApi.Core.Entities.UserGenerated;

namespace PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;

public interface IUserReadRepository: IBaseReadRepository<User>
{
    Task<bool> ExistsAsync(string username);
}
