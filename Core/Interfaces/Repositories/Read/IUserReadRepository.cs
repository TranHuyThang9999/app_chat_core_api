using PaymentCoreServiceApi.Common;
using PaymentCoreServiceApi.Core.Entities.UserGenerated;

namespace PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;

public interface IUserReadRepository: IBaseReadRepository<User>
{
    Task<bool> ExistsAsync(string username);
    Task<PagedResult<User>> GetUsersAsync(int page, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default);
}
