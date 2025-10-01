using System.ComponentModel.DataAnnotations;
using AppChat.Core.Entities.ConversationGenerated;
using PaymentCoreServiceApi.Common.Mediator;

namespace PaymentCoreServiceApi.Features.Channels.Commands;

public record CreateDirectMessageCommand : IRequestApiResponse<Channel>
{
    [Required(ErrorMessage = "TargetUserId is required")]
    public long TargetUserId { get; set; }
}