using PaymentCoreServiceApi.Common;
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Core.Entities.MessageGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using PaymentCoreServiceApi.Services;

namespace PaymentCoreServiceApi.Features.Messages.Commands;

public class SendMessageCommandHandler : IRequestApiResponseHandler<SendMessageCommand, Message>
{
    private readonly IMessageWriteRepository _messageWriteRepository;
    private readonly IUserReadRepository _userReadRepository;
    private readonly IExecutionContext _currentUser;

    public SendMessageCommandHandler(
        IMessageWriteRepository messageWriteRepository,
        IUserReadRepository userReadRepository,
        IExecutionContext currentUser)
    {
        _messageWriteRepository = messageWriteRepository;
        _userReadRepository = userReadRepository;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<Message>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        return null;
    }
}
