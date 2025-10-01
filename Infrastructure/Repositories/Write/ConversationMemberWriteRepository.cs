using PaymentCoreServiceApi.Core.Entities.ConversationMemberGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using PaymentCoreServiceApi.Infrastructure.DbContexts;

namespace PaymentCoreServiceApi.Infrastructure.Repositories.Write;
public class ChannelMemberWriteRepository : EfBaseWriteOnlyRepository<ChannelMember>, IChannelMemberWriteRepository
{
    private readonly AppDbContext _context;
    public ChannelMemberWriteRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}