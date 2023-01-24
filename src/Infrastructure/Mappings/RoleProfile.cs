using AutoMapper;
using BlueLotus360.Com.Infrastructure.Models.Identity;
using BlueLotus360.Com.Application.Responses.Identity;

namespace BlueLotus360.Com.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, BlazorHeroRole>().ReverseMap();
        }
    }
}