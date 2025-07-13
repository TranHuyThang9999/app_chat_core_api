using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Core.Entities.BankAccountGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using PaymentCoreServiceApi.Infrastructure.DbContexts;

namespace PaymentCoreServiceApi.Infrastructure.Repositories.Write;

public class BankAccountWriteRepository : EfBaseWriteOnlyRepository<BankAccount>, IBankAccountWriteRepository
{
    private readonly AppDbContext _context;

    public BankAccountWriteRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsBankAccountByUserIdAsync(long userId)
    {
        return await _context.BankAccounts.AnyAsync(ba => ba.UserId == userId);
    }
}