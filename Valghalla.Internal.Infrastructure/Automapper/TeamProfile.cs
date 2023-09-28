using AutoMapper;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Team.Responses;
using Valghalla.Internal.Application.Modules.Shared.Team.Responses;

namespace Valghalla.Internal.Infrastructure.Automapper
{
    internal class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<TeamEntity, ListTeamsItemResponse>()
                .ForMember(x => x.ResponsibleCount, x => x.MapFrom(src => src.TeamResponsibles.Count()))
                .ForMember(x => x.TaskCount, x => x.MapFrom(src => src.WorkLocationTeams.Sum(t => t.TaskAssignments.Count)))
                ;
            CreateMap<TeamEntity, TeamDetailResponse>()
                .ForMember(x => x.TaskCount, x => x.MapFrom(src => src.WorkLocationTeams.Sum(t => t.TaskAssignments.Count)))
                .ForMember(x => x.ResponsibleIds, x => x.MapFrom(src => src.TeamResponsibles.Select(i => i.ParticipantId)));

            CreateMap<TeamEntity, TeamSharedResponse>();
        }
    }
}
