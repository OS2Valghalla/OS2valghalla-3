using AutoMapper;

using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Responses;
using Valghalla.Internal.Application.Modules.Shared.WorkLocationTemplate.Responses;

namespace Valghalla.Internal.Infrastructure.Automapper
{
    internal class WorkLocationTemplateProfile : Profile
    {
        public WorkLocationTemplateProfile()
        {
            CreateMap<WorkLocationTemplateEntity, WorkLocationTemplateResponse>()
                .ForMember(f => f.AreaName, opt => opt.MapFrom(src => src.Area != null ? src.Area.Name : string.Empty));
            CreateMap<WorkLocationTemplateEntity, WorkLocationTemplateDetailResponse>()
                .ForMember(f => f.AreaName, opt => opt.MapFrom(src => src.Area != null ? src.Area.Name : string.Empty));
            CreateMap<WorkLocationTemplateEntity, WorkLocationTemplateSharedResponse>();
        }
    }
}
