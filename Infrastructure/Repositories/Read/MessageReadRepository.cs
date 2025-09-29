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
    public async Task<IEnumerable<Message>> GetMessagesByConversationAsync(long conversationId,long userId,bool onlyUnread = false,int skip = 0,int take = 50,
    CancellationToken cancellationToken = default)
    {
        // Bước 1: Kiểm tra người dùng có trong hội thoại không
        var isMember = await _context.ConversationMembers
            .AnyAsync(cm => cm.ConversationId == conversationId &&
                            cm.UserId == userId &&
                            !cm.IsLeft,
                    cancellationToken);

        if (!isMember)
            return Enumerable.Empty<Message>();

        // Bước 2: Lấy danh sách tin nhắn
        var query = _context.Messages
            .Where(m => m.ConversationId == conversationId)
            .OrderByDescending(m => m.CreatedAt)
            .Skip(skip)
            .Take(take);

        return await query.ToListAsync(cancellationToken);
    }

}
