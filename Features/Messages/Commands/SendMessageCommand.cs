using System.ComponentModel.DataAnnotations;
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Core.Entities.MessageGenerated;

namespace PaymentCoreServiceApi.Features.Messages.Commands;

public record SendMessageCommand : IRequestApiResponse<Message>
{   
    [Required]
    public long ReceiverId { get; set; }
    
    [Required]
    [StringLength(1000, MinimumLength = 1)]
    public string Content { get; set; } = string.Empty;
}