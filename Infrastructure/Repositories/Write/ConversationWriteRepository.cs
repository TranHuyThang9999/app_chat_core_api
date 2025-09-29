using AppChat.Core.Entities.ConversationGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using PaymentCoreServiceApi.Infrastructure.DbContexts;

namespace PaymentCoreServiceApi.Infrastructure.Repositories.Write;

public class ConversationWriteRepository : EfBaseWriteOnlyRepository<Conversation>, IConversationWriteRepository
{
    private readonly AppDbContext _context;
    public ConversationWriteRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}