using AutoMapper;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.Tasks.Responses;
using Valghalla.External.Application.Modules.Team.Responses;

namespace Valghalla.External.Infrastructure.Automapper
{
    internal class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<TeamEntity, TaskPreviewTeam>();
            CreateMap<TeamEntity, TeamResponse>();
            CreateMap<ParticipantEntity, TeamMemberResponse>()
                .ForMember(f => f.AssignedTasksCount, a => a.Ignore())
                .ForMember(f => f.CanBeRemoved, a => a.Ignore())
                .ForMember(f => f.Name, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
        }
    }
}
