using AutoMapper;
using Valghalla.Application.Storage;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Communication.Responses;
using Valghalla.Internal.Application.Modules.Shared.Communication.Responses;

namespace Valghalla.Internal.Infrastructure.Automapper
{
    internal class CommunicationProfile: Profile
    {
        public CommunicationProfile()
        {
            CreateMap<CommunicationTemplateEntity, CommunicationTemplateListingItemResponse>();
            CreateMap<CommunicationTemplateEntity, CommunicationTemplateDetailsResponse>()
                .ForMember(dest => dest.CommunicationTemplateFileReferences, opt => opt.MapFrom(src => src.CommunicationTemplateFileReferences));
            CreateMap<CommunicationTemplateEntity, CommunicationTemplateSharedResponse>();
        }
    }
}
