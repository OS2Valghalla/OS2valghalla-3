using AutoMapper;
using Valghalla.Database.Entities.Tables;
using Valghalla.Worker.Infrastructure.Modules.Tasks.Responses;

namespace Valghalla.Worker.Infrastructure.Automapper
{
    internal class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskAssignmentEntity, TaskAssignmentResponse>();
        }
    }
}
