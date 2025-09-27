using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Core.Entities.BankAccountGenerated;
using PaymentCoreServiceApi.Core.Entities.UserGenerated;

namespace PaymentCoreServiceApi.Infrastructure.DbContexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<BankAccount> BankAccounts { get; set; }

    //dotnet ef database update
}