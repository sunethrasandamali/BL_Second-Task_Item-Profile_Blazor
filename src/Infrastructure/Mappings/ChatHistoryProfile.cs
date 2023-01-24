using AutoMapper;
using BlueLotus360.Com.Application.Interfaces.Chat;
using BlueLotus360.Com.Application.Models.Chat;
using BlueLotus360.Com.Infrastructure.Models.Identity;

namespace BlueLotus360.Com.Infrastructure.Mappings
{
    public class ChatHistoryProfile : Profile
    {
        public ChatHistoryProfile()
        {
            CreateMap<ChatHistory<IChatUser>, ChatHistory<BlazorHeroUser>>().ReverseMap();
        }
    }
}