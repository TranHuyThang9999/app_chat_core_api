using PaymentCoreServiceApi.Core.Entities.UserAgents;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using PaymentCoreServiceApi.Infrastructure.DbContexts;

namespace PaymentCoreServiceApi.Infrastructure.Repositories.Write;

public class UserWriteRepository : EfBaseWriteOnlyRepository<User>, IUserWriteRepository
{
    public UserWriteRepository(AppDbContext context) : base(context)
    {
    }
}