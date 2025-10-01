using AppChat.Core.Entities.ConversationGenerated;
using PaymentCoreServiceApi.Common;
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Core.Entities.ConversationMemberGenerated;
using PaymentCoreServiceApi.Core.Entities.MessageGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using PaymentCoreServiceApi.Services;


namespace PaymentCoreServiceApi.Features.Messages.Commands;


public class SendMessageCommandHandler : IRequestApiResponseHandler<SendMessageCommand, Message>
{
    private readonly IMessageWriteRepository _messageWriteRepository;
    private readonly IConversationWriteRepository _conversationWriteRepository;
    private readonly IConversationReadRepository _conversationReadRepository;
    private readonly IUserReadRepository _userReadRepository;
    private readonly IExecutionContext _currentUser;
    private readonly ILogger<SendMessageCommandHandler> _logger;
    private readonly IChannelMemberWriteRepository _channelMemberWriteRepository;           


    public SendMessageCommandHandler(
        IMessageWriteRepository messageWriteRepository,
        IConversationWriteRepository conversationWriteRepository,
        IConversationReadRepository conversationReadRepository,
        IChannelMemberWriteRepository channelMemberWriteRepository,
        IUserReadRepository userReadRepository,
        IExecutionContext currentUser,
        ILogger<SendMessageCommandHandler> logger)
    {
        _messageWriteRepository = messageWriteRepository;
        _conversationWriteRepository = conversationWriteRepository;
        _conversationReadRepository = conversationReadRepository;
        _userReadRepository = userReadRepository;
        _currentUser = currentUser;
        _logger = logger;
        _channelMemberWriteRepository = channelMemberWriteRepository;  
    }

    public async Task<ApiResponse<Message>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Kiểm tra xem đã có conversation private giữa 2 user chưa
            var existingConversation = await _conversationReadRepository.GetPrivateConversationAsync(
                _currentUser.Id, request.ReceiverId, cancellationToken);

            Conversation conversation;
            
            if (existingConversation != null)
            {
                // Sử dụng conversation đã có
                conversation = existingConversation;
                _logger.LogInformation("SendMessageCommandHandler: Sử dụng conversation hiện có, conversationId = {conversationId}", conversation.Id);
            }
            else
            {
                // Tạo conversation mới cho private chat
                conversation = new Conversation
                {
                    IsGroup = false,
                    Name = null // Private chat không cần tên
                };
                
                await _conversationWriteRepository.AddAsync(conversation);
                await _conversationWriteRepository.CommitAsync(cancellationToken);
                _logger.LogInformation("SendMessageCommandHandler: Tạo conversation mới, conversationId = {conversationId}", conversation.Id);
                
                // Thêm 2 thành viên vào conversation
                await _channelMemberWriteRepository.AddRangeAsync(new List<ChannelMember>
                {
                    new ChannelMember
                    {
                        ChannelId = conversation.Id,
                        UserId = _currentUser.Id,
                        JoinedAt = DateTime.UtcNow
                    },
                    new ChannelMember
                    {
                        ChannelId = conversation.Id,
                        UserId = request.ReceiverId,
                        JoinedAt = DateTime.UtcNow
                    }
                });
                
                await _channelMemberWriteRepository.CommitAsync(cancellationToken);
            }
            
            // Tạo message
            var message = new Message
            {
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                SenderId = _currentUser.Id,
                ConversationId = conversation.Id,
            };
            
            await _messageWriteRepository.AddAsync(message);
            await _messageWriteRepository.CommitAsync(cancellationToken);
            _logger.LogInformation("SendMessageCommandHandler: messageId = {messageId}", message.Id);
            
            return ApiResponse<Message>.Success(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while sending message");
            throw;
        }
    }
}
