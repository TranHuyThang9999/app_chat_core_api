using PaymentCoreServiceApi.Core.Interfaces.Repositories.IUnitOfWork;
using PaymentCoreServiceApi.Infrastructure.DbContexts;

namespace PaymentCoreServiceApi.Infrastructure.Repositories.UnitOfWork
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}