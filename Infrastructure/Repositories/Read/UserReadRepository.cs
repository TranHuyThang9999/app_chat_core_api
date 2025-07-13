using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Core.Entities.UserGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Infrastructure.DbContexts;

namespace PaymentCoreServiceApi.Infrastructure.Repositories.Read;

public class UserReadRepository : EfBaseReadOnlyRepository<User>, IUserReadRepository
{
    private readonly AppDbContext _context;

    public UserReadRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.UserName == username);
    }
}