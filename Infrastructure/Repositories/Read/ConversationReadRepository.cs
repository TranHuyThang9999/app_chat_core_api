using AppChat.Core.Entities.ConversationGenerated;
using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Infrastructure.DbContexts;

namespace PaymentCoreServiceApi.Infrastructure.Repositories.Read;

public class ChannelReadRepository : EfBaseReadOnlyRepository<Channel>, IChannelReadRepository
{
    private readonly AppDbContext _context;

    public ChannelReadRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Channel?> GetPrivateConversationAsync(long userId1, long userId2, CancellationToken cancellationToken = default)
    {
        // Tìm channel private giữa 2 user
        var conversation = await _context.Channels
            .Where(c => !c.IsGroup)
            .Where(c => _context.ChannelMembers
                .Where(cm => cm.ChannelId == c.Id && !cm.IsLeft)
                .Select(cm => cm.UserId)
                .Contains(userId1) &&
                _context.ChannelMembers
                .Where(cm => cm.ChannelId == c.Id && !cm.IsLeft)
                .Select(cm => cm.UserId)
                .Contains(userId2))
            .Where(c => _context.ChannelMembers
                .Count(cm => cm.ChannelId == c.Id && !cm.IsLeft) == 2)
            .FirstOrDefaultAsync(cancellationToken);

        return conversation;
    }

    public async Task<IEnumerable<Channel>> GetUserConversationsAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.Channels
            .Where(c => _context.ChannelMembers
                .Any(cm => cm.ChannelId == c.Id && cm.UserId == userId && !cm.IsLeft))
            .OrderByDescending(c => c.UpdatedAt)
            .ToListAsync(cancellationToken);
    }
}
