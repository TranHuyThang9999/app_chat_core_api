using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Core.Entities.UserAgents;

namespace PaymentCoreServiceApi.Infrastructure.DbContexts;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Optional: Fluent API config here
    }
}