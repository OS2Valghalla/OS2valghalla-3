using AutoMapper;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Participant.Responses;
using Valghalla.Internal.Application.Modules.Shared.Participant.Responses;

namespace Valghalla.Internal.Infrastructure.Automapper
{
    internal class ParticipantProfile : Profile
    {
        public ParticipantProfile()
        {
            CreateMap<ParticipantEntity, ParticipantListingItemResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName + ' ' + src.LastName))
                .ForMember(dest => dest.DigitalPost, opt => opt.MapFrom(src => !src.ExemptDigitalPost))
                .ForMember(dest => dest.AssignedTask, opt => opt.MapFrom(src => src.TaskAssignments.Any()))
                .ForMember(dest => dest.HasUnansweredTask, opt => opt.MapFrom(src => src.TaskAssignments.Any(x => !x.Accepted)))
                .ForMember(dest => dest.TeamIds, opt => opt.MapFrom(src => src.TeamMembers.Select(i => i.TeamId).Distinct()))
                .ForMember(dest => dest.TaskTypeIds, opt => opt.MapFrom(src => src.TaskAssignments.Select(i => i.TaskTypeId).Distinct()));

            CreateMap<ParticipantEntity, ParticipantDetailResponse>()
                .ForMember(dest => dest.MemberTeamIds, opt => opt.MapFrom(src => src.TeamMembers.Select(i => i.TeamId)))
                .ForMember(dest => dest.SpecialDietIds, opt => opt.MapFrom(src => src.SpecialDietParticipants.Select(i => i.SpecialDietId)));

            CreateMap<ParticipantEntity, ParticipantSharedListingItemResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName + ' ' + src.LastName));

            CreateMap<ParticipantEntity, ParticipantSharedResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName + ' ' + src.LastName));

            CreateMap<ParticipantEventLogEntity, ParticipantEventLogListingItemResponse>();
        }
    }
}
