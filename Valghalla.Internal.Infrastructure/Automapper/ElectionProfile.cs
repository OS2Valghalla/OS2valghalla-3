using AutoMapper;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Election.Responses;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Responses;
using Valghalla.Internal.Application.Modules.App.Responses;
using Valghalla.Internal.Application.Modules.Shared.Election.Responses;
using Valghalla.Internal.Application.Modules.Shared.ElectionType.Responses;

namespace Valghalla.Internal.Infrastructure.Automapper
{
    internal class ElectionProfile : Profile
    {
        public ElectionProfile()
        {
            CreateMap<ElectionEntity, ElectionDetailsResponse>()
               .ForMember(dest => dest.WorkLocationIds, opt => opt.MapFrom(src => src.WorkLocations.Select(i => i.Id).ToArray()));

            CreateMap<ElectionEntity, ElectionListingItemResponse>()
                .ForMember(dest => dest.ElectionTypeName, opt => opt.MapFrom(src => src.ElectionType.Title));

            CreateMap<ElectionTypeEntity, ElectionTypeResponse>()
                .ForMember(dest => dest.ValidationRuleIds, opt => opt.MapFrom(src => src.ValidationRules.Select(r => r.ValidationRuleId).ToList()));

            CreateMap<ElectionEntity, ElectionSharedResponse>();
            CreateMap<ElectionEntity, AppElectionResponse>();
            CreateMap<ElectionTypeEntity, ElectionTypeSharedResponse>();

            CreateMap<ElectionEntity, ElectionCommunicationConfigurationsResponse>()
                .ForMember(f => f.ElectionTaskTypeCommunicationTemplates, a => a.Ignore());
        }
    }
}
