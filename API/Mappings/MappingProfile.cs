using AutoMapper;
using Integrations.CCP.DTO;
using Integrations.Crayon.Database.Models;
using ReadFacade.CCP.DTO;
using WriteFacade.Crayon.DTO;

namespace API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken))
                .ForMember(dest => dest.RefreshTokenExpiry, opt => opt.MapFrom(src => src.RefreshTokenExpiry))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash))
                .ForMember(dest => dest.Salt, opt => opt.MapFrom(src => src.Salt))
                .ForMember(dest => dest.Accounts, opt => opt.MapFrom(src => src.Accounts))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<CCPSoftware, PurchasableSoftware>();
            CreateMap<License, LicenseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SubscriptionId, opt => opt.MapFrom(src => src.SubscriptionId))
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Key));
            CreateMap<Account, AccountDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.Subscriptions, opt => opt.MapFrom(src => src.Subscriptions))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
            CreateMap<Subscription, SubscriptionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SoftwareId, opt => opt.MapFrom(src => src.SoftwareId))
                .ForMember(dest => dest.ValidUntil, opt => opt.MapFrom(src => src.ValidUntil))
                .ForMember(dest => dest.SoftwareName, opt => opt.MapFrom(src => src.SoftwareName))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                .ForMember(dest => dest.Licenses, opt => opt.MapFrom(src => src.Licenses))
                .ForMember(dest => dest.SoftwareDescription, opt => opt.MapFrom(src => src.SoftwareDescription));
        }
    }
}
