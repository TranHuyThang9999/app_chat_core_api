using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Core.Entities.BankAccountGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Infrastructure.DbContexts;

namespace PaymentCoreServiceApi.Infrastructure.Repositories.Read;

public class BankAccountReadRepository:  EfBaseReadOnlyRepository<BankAccount>, IBankAccountReadRepository
{
    private readonly AppDbContext _context;
    public BankAccountReadRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsBankAccountByUserIdAsync(long userId)
    {
        return await _context.BankAccounts.AnyAsync(ba => ba.UserId == userId);
    }
}