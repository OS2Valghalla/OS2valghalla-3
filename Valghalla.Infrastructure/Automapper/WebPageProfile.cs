using AutoMapper;
using Valghalla.Application.Web;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Infrastructure.Automapper
{
    internal class WebPageProfile : Profile
    {
        public WebPageProfile()
        {
            CreateMap<WebPageEntity, WebPage>();
            CreateMap<ElectionCommitteeContactInformationEntity, ElectionCommitteeContactInformationPage>();
        }
    }
}
