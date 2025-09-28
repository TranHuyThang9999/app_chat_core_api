using System.ComponentModel.DataAnnotations;
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Core.Entities.UserGenerated;

namespace PaymentCoreServiceApi.Features.Users.Commands;

public record CreateUserCommand: IRequestApiResponse<User>
{
    public string NickName { get; set; }
    public string Avatar { get; set; }
    public int? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}
