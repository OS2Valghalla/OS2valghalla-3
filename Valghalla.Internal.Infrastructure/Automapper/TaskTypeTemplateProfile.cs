using AutoMapper;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Responses;
using Valghalla.Internal.Application.Modules.Shared.TaskTypeTemplate.Responses;
using Valghalla.Internal.Application.Modules.Tasks.Responses;

namespace Valghalla.Internal.Infrastructure.Automapper
{
    internal class TaskTypeTemplateProfile : Profile
    {
        public TaskTypeTemplateProfile()
        {
            CreateMap<TaskTypeTemplateEntity, TaskTypeTemplateListingItemResponse>();
            CreateMap<TaskTypeTemplateEntity, TaskTypeTemplateDetailResponse>();
            CreateMap<TaskTypeTemplateEntity, TaskTypeTemplateSharedResponse>();        
        }
    }
}
