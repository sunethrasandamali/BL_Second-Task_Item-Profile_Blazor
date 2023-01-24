using AutoMapper;
using BlueLotus360.Com.Application.Requests.Identity;
using BlueLotus360.Com.Application.Responses.Identity;
using BlueLotus360.Com.Infrastructure.Models.Identity;

namespace BlueLotus360.Com.Infrastructure.Mappings
{
    public class RoleClaimProfile : Profile
    {
        public RoleClaimProfile()
        {
            CreateMap<RoleClaimResponse, BlazorHeroRoleClaim>()
                .ForMember(nameof(BlazorHeroRoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
                .ForMember(nameof(BlazorHeroRoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
                .ReverseMap();

            CreateMap<RoleClaimRequest, BlazorHeroRoleClaim>()
                .ForMember(nameof(BlazorHeroRoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
                .ForMember(nameof(BlazorHeroRoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
                .ReverseMap();
        }
    }
}