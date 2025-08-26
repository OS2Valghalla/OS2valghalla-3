using AutoMapper;

using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Responses;
using Valghalla.Internal.Application.Modules.Shared.TaskType.Responses;
using Valghalla.Internal.Application.Modules.Tasks.Responses;

namespace Valghalla.Internal.Infrastructure.Automapper
{
    internal class TaskTypeProfile : Profile
    {
        public TaskTypeProfile()
        {
            CreateMap<TaskTypeEntity, TaskTypeListingItemResponse>();
            CreateMap<TaskTypeEntity, TaskTypeDetailResponse>();
            CreateMap<TaskTypeEntity, TaskTypeSharedResponse>();
            CreateMap<TaskTypeEntity, TaskTypeWithAreaIdsResponse>()
                .ForMember(f => f.AreaIds, a => a.Ignore());
            CreateMap<TaskTypeEntity, TaskTypeWithTeamIdsResponse>()
                .ForMember(f => f.TeamIds, a => a.Ignore());
        }
    }
}
