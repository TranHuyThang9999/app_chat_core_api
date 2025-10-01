namespace PaymentCoreServiceApi.Features.Users.DTOs;

public class UserBasicInfoDto
{
    public long Id { get; set; }
    public string? NickName { get; set; }
    public string Avatar { get; set; }
    public int? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Active { get; set; }
}