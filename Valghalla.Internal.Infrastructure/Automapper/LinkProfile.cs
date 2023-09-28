using AutoMapper;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Link.Responses;

namespace Valghalla.Internal.Infrastructure.Automapper
{
    internal class TeamLinkProfile : Profile
    {
        public TeamLinkProfile() 
        {
            CreateMap<TeamLinkEntity, LinkResponse>();
        }
    }

    internal class TaskLinkProfile : Profile
    {
        public TaskLinkProfile()
        {
            CreateMap<TaskLinkEntity, LinkResponse>();
        }
    }

    internal class TasksFilteredProfile : Profile
    {
        public TasksFilteredProfile()
        {
            CreateMap<TasksFilteredLinkEntity, LinkResponse>();
        }
    }
}
