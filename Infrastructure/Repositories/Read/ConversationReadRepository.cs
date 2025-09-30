using AppChat.Core.Entities.ConversationGenerated;
using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Infrastructure.DbContexts;

namespace PaymentCoreServiceApi.Infrastructure.Repositories.Read;

public class ConversationReadRepository : EfBaseReadOnlyRepository<Conversation>, IConversationReadRepository
{
    private readonly AppDbContext _context;

    public ConversationReadRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Conversation?> GetPrivateConversationAsync(long userId1, long userId2, CancellationToken cancellationToken = default)
    {
        // Tìm conversation private giữa 2 user
        var conversation = await _context.Conversations
            .Where(c => !c.IsGroup)
            .Where(c => _context.ConversationMembers
                .Where(cm => cm.ConversationId == c.Id && !cm.IsLeft)
                .Select(cm => cm.UserId)
                .Contains(userId1) &&
                _context.ConversationMembers
                .Where(cm => cm.ConversationId == c.Id && !cm.IsLeft)
                .Select(cm => cm.UserId)
                .Contains(userId2))
            .Where(c => _context.ConversationMembers
                .Count(cm => cm.ConversationId == c.Id && !cm.IsLeft) == 2)
            .FirstOrDefaultAsync(cancellationToken);

        return conversation;
    }

    public async Task<IEnumerable<Conversation>> GetUserConversationsAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.Conversations
            .Where(c => _context.ConversationMembers
                .Any(cm => cm.ConversationId == c.Id && cm.UserId == userId && !cm.IsLeft))
            .OrderByDescending(c => c.UpdatedAt)
            .ToListAsync(cancellationToken);
    }
}