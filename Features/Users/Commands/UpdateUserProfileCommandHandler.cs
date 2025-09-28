using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Common;
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Core.Entities.UserGenerated;
using PaymentCoreServiceApi.Infrastructure.DbContexts;
using PaymentCoreServiceApi.Services;

namespace PaymentCoreServiceApi.Features.Users.Commands;

public class UpdateUserProfileCommandHandler : IRequestApiResponseHandler<UpdateUserProfileCommand, User>
{
    private readonly AppDbContext _context;
    private readonly IExecutionContext _currentUser;

    public UpdateUserProfileCommandHandler(AppDbContext context, IExecutionContext currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<User>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated)
        {
            return ApiResponse<User>.Unauthorized("User is not authenticated");
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == _currentUser.Id, cancellationToken);

        if (user == null)
        {
            return ApiResponse<User>.NotFound($"User with ID {_currentUser.Id} not found");
        }

        // Update only provided fields
        if (!string.IsNullOrEmpty(request.NickName))
            user.NickName = request.NickName;
        
        if (!string.IsNullOrEmpty(request.Avatar))
            user.Avatar = request.Avatar;
        
        if (request.Gender.HasValue)
            user.Gender = request.Gender;
        
        if (request.BirthDate.HasValue)
            user.BirthDate = request.BirthDate;
        
        if (request.Age.HasValue)
            user.Age = request.Age.Value;
        
        if (!string.IsNullOrEmpty(request.Email))
            user.Email = request.Email;
        
        if (!string.IsNullOrEmpty(request.PhoneNumber))
            user.PhoneNumber = request.PhoneNumber;
        
        if (!string.IsNullOrEmpty(request.Address))
            user.Address = request.Address;

        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = _currentUser.Id;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return ApiResponse<User>.Success(user, "Profile updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<User>.InternalServerError($"Failed to update profile: {ex.Message}");
        }
    }
}