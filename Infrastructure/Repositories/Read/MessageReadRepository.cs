using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Core.Entities.MessageGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Infrastructure.DbContexts;

namespace PaymentCoreServiceApi.Infrastructure.Repositories.Read;

public class MessageReadRepository : EfBaseReadOnlyRepository<Message>, IMessageReadRepository
{
    private readonly AppDbContext _context;

    public MessageReadRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

}
