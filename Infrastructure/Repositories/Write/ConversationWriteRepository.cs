using AppChat.Core.Entities.ConversationGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using PaymentCoreServiceApi.Infrastructure.DbContexts;

namespace PaymentCoreServiceApi.Infrastructure.Repositories.Write;

public class ChannelWriteRepository : EfBaseWriteOnlyRepository<Channel>, IChannelWriteRepository
{
    private readonly AppDbContext _context;
    public ChannelWriteRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
