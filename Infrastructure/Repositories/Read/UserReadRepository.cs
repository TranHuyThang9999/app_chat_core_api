using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Common;
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

    public async Task<PagedResult<User>> GetUsersAsync(int page, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Users.AsQueryable();

        // Apply search filter if provided
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchTermLower = searchTerm.ToLower();
            query = query.Where(u => 
                u.UserName.ToLower().Contains(searchTermLower) ||
                u.Email.ToLower().Contains(searchTermLower) ||
                (u.NickName != null && u.NickName.ToLower().Contains(searchTermLower)));
        }

        // Get total count for pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination
        var users = await query
            .OrderBy(u => u.UserName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<User>(users, totalCount, page, pageSize);
    }
}