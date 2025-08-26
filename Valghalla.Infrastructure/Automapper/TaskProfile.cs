using AutoMapper;

using Valghalla.Application.TaskValidation;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Infrastructure.Automapper
{
    internal class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskTypeEntity, EvaluatedTaskType>();
        }
    }
}
