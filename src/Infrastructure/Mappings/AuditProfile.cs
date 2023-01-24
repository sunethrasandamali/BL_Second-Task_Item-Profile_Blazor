using AutoMapper;
using BlueLotus360.Com.Infrastructure.Models.Audit;
using BlueLotus360.Com.Application.Responses.Audit;

namespace BlueLotus360.Com.Infrastructure.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditResponse, Audit>().ReverseMap();
        }
    }
}