using AutoMapper;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.Shared.WorkLocation.Responses;
using Valghalla.External.Application.Modules.Tasks.Responses;

namespace Valghalla.External.Infrastructure.Automapper
{
    internal class WorkLocationProfile:Profile
    {
        public WorkLocationProfile()
        {
            CreateMap<WorkLocationEntity, WorkLocationSharedResponse>();
            CreateMap<WorkLocationEntity, TaskPreviewWorkLocation>();
        }
    }
}
