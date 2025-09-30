using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Core.Entities.ConversationMemberGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Infrastructure.DbContexts;

namespace PaymentCoreServiceApi.Infrastructure.Repositories.Read;

public class ConversationMemberReadRepository : EfBaseReadOnlyRepository<ConversationMember>, IConversationMemberReadRepository
{
    private readonly AppDbContext _context;

    public ConversationMemberReadRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> IsUserInConversationAsync(long conversationId, long userId, CancellationToken cancellationToken = default)
    {
        return await _context.ConversationMembers
            .AnyAsync(cm => cm.ConversationId == conversationId && 
                           cm.UserId == userId && 
                           !cm.IsLeft, cancellationToken);
    }

    public async Task<IEnumerable<ConversationMember>> GetConversationMembersAsync(long conversationId, CancellationToken cancellationToken = default)
    {
        return await _context.ConversationMembers
            .Where(cm => cm.ConversationId == conversationId && !cm.IsLeft)
            .ToListAsync(cancellationToken);
    }
}