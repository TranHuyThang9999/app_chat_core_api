using System.ComponentModel.DataAnnotations;
using PaymentCoreServiceApi.Common;
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Core.Entities.MessageGenerated;

namespace PaymentCoreServiceApi.Features.Messages.Queries;

public record GetMessagesQuery : IRequestApiResponse<PagedResult<Message>>
{
    [Required(ErrorMessage = "ConversationId is required")]
    public long ConversationId { get; set; }
    public bool OnlyUnread { get; set; } = false;
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 50;
}