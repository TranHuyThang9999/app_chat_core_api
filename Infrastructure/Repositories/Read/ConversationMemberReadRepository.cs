using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Core.Entities.ConversationMemberGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Infrastructure.DbContexts;

namespace PaymentCoreServiceApi.Infrastructure.Repositories.Read;

public class ChannelMemberReadRepository : EfBaseReadOnlyRepository<ChannelMember>, IChannelMemberReadRepository
{
    private readonly AppDbContext _context;

    public ChannelMemberReadRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> IsUserInChannelAsync(long channelId, long userId, CancellationToken cancellationToken = default)
    {
        return await _context.ChannelMembers
            .AnyAsync(cm => cm.ChannelId == channelId && 
                           cm.UserId == userId && 
                           !cm.IsLeft, cancellationToken);
    }

    public async Task<IEnumerable<ChannelMember>> GetChannelMembersAsync(long channelId, CancellationToken cancellationToken = default)
    {
        return await _context.ChannelMembers
            .Where(cm => cm.ChannelId == channelId && !cm.IsLeft)
            .ToListAsync(cancellationToken);
    }
}