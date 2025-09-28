using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Core.Entities.MessageGenerated;

namespace PaymentCoreServiceApi.Features.Messages.Queries;

public record GetMessagesQuery : IRequestApiResponse<IEnumerable<Message>>
{
    public long UserId { get; set; }
    public long? OtherUserId { get; set; }
    public bool OnlyUnread { get; set; } = false;
}