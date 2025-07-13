namespace PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;

public interface IUserReadRepository
{
    Task<bool> ExistsAsync(string username);
}