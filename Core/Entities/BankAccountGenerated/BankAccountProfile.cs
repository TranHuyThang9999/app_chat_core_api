using AutoMapper;

namespace PaymentCoreServiceApi.Core.Entities.BankAccountGenerated;

public class BankAccountProfile : Profile
{
    public BankAccountProfile()
    {
        CreateMap<BankAccount, BankAccountDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt != null ? src.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss") : null))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt != null ? src.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss") : null));
    }
}
