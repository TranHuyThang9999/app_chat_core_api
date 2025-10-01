using AppChat.Core.Entities.ConversationGenerated;
using PaymentCoreServiceApi.Common;
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Core.Entities.ConversationMemberGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using PaymentCoreServiceApi.Services;

namespace PaymentCoreServiceApi.Features.Channels.Commands;

public class CreateDirectMessageCommandHandler : IRequestApiResponseHandler<CreateDirectMessageCommand, Channel>
{
    private readonly IChannelReadRepository _channelReadRepository;
    private readonly IChannelWriteRepository _channelWriteRepository;
    private readonly IChannelMemberWriteRepository _channelMemberWriteRepository;
    private readonly IUserReadRepository _userReadRepository;
    private readonly IExecutionContext _executionContext;

    public CreateDirectMessageCommandHandler(
        IChannelReadRepository channelReadRepository,
        IChannelWriteRepository channelWriteRepository,
        IChannelMemberWriteRepository channelMemberWriteRepository,
        IUserReadRepository userReadRepository,
        IExecutionContext executionContext)
    {
        _channelReadRepository = channelReadRepository;
        _channelWriteRepository = channelWriteRepository;
        _channelMemberWriteRepository = channelMemberWriteRepository;
        _userReadRepository = userReadRepository;
        _executionContext = executionContext;
    }

    public async Task<ApiResponse<Channel>> Handle(CreateDirectMessageCommand request, CancellationToken cancellationToken)
    {
        // Lấy thông tin user hiện tại
        var currentUserId = _executionContext.Id;   
        if (currentUserId == null)
        {
            return ApiResponse<Channel>.Unauthorized("Unauthorized", 401);
        }

        // Kiểm tra target user có tồn tại không
        var targetUser = await _userReadRepository.GetByIdAsync(request.TargetUserId);
        if (targetUser == null)
        {
            return ApiResponse<Channel>.NotFound("Target user not found", 404);
        }

        // Không thể tạo DM với chính mình
        if (currentUserId == request.TargetUserId)
        {
            return ApiResponse<Channel>.BadRequest("Cannot create direct message with yourself", 400);
        }

        // Kiểm tra xem đã có DM giữa 2 user này chưa
        var existingChannel = await _channelReadRepository.GetPrivateConversationAsync(currentUserId, request.TargetUserId, cancellationToken);
        
        if (existingChannel != null)
        {
            return ApiResponse<Channel>.Success(existingChannel);
        }

        // Tạo DM mới
        var newChannel = new Channel
        {
            Name = null, // DM không có tên
            IsGroup = false,
            AvatarUrl = null,
            CreatedBy = currentUserId,
            UpdatedBy = currentUserId
        };

        var createdChannel = await _channelWriteRepository.AddAsync(newChannel);

        // Thêm 2 thành viên vào DM
        var members = new List<ChannelMember>
        {
            new ChannelMember
            {
                ChannelId = createdChannel.Id,
                UserId = currentUserId,
                JoinedAt = DateTime.UtcNow,
                IsAdmin = false,
                IsLeft = false,
                CreatedBy = currentUserId,
                UpdatedBy = currentUserId
            },
            new ChannelMember
            {
                ChannelId = createdChannel.Id,
                UserId = request.TargetUserId,
                JoinedAt = DateTime.UtcNow,
                IsAdmin = false,
                IsLeft = false,
                CreatedBy = currentUserId,
                UpdatedBy = currentUserId
            }
        };

        await _channelMemberWriteRepository.AddRangeAsync(members);
        await _channelMemberWriteRepository.CommitAsync();
        
        return ApiResponse<Channel>.Success(createdChannel);
    }
}