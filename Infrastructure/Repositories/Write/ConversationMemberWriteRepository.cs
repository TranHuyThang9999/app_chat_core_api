using PaymentCoreServiceApi.Core.Entities.ConversationMemberGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using PaymentCoreServiceApi.Infrastructure.DbContexts;

namespace PaymentCoreServiceApi.Infrastructure.Repositories.Write;
public class ConversationMemberWriteRepository : EfBaseWriteOnlyRepository<ConversationMember>, IConversationMemberWriteRepository
{
    private readonly AppDbContext _context;
    public ConversationMemberWriteRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}