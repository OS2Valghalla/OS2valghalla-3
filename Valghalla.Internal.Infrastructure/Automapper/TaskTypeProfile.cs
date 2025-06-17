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
            CreateMap<TaskTypeEntity, TaskTypeListingItemResponse>()
                .ForMember(f => f.AreaName, opt => opt.MapFrom(src =>
                    src.WorkLocationTaskTypes
                        .Select(x => x.WorkLocation.Area.Name)
                        .FirstOrDefault() ?? string.Empty))
                .ForMember(f => f.AreaId, opt => opt.MapFrom(src =>
                    src.WorkLocationTaskTypes
                        .Select(x => x.WorkLocation.Area.Id)
                        .DefaultIfEmpty(Guid.Empty)
                        .FirstOrDefault()))
                .ForMember(dest => dest.WorkLocationId, opt => opt.MapFrom(src =>
                    src.WorkLocationTaskTypes
                        .Select(e => e.WorkLocation.Id)
                        .FirstOrDefault()))
                .ForMember(dest => dest.WorkLocationTitle, opt => opt.MapFrom(src =>
                    src.WorkLocationTaskTypes
                        .Select(e => e.WorkLocation.Title)
                        .FirstOrDefault()))
                .ForMember(dest => dest.TaskTypeTemplateId, opt => opt.MapFrom(src =>
                    src.TaskTypeTemplateEntityId.HasValue ? src.TaskTypeTemplateEntityId.Value.ToString() : string.Empty))
                .ForMember(dest => dest.TaskTypeTemplateTitle, opt => opt.MapFrom(src =>
                    src.TaskTypeTemplate != null ? src.TaskTypeTemplate.Title : string.Empty))
                .ForMember(dest => dest.ElectionId, opt => opt.MapFrom(src =>
                    src.WorkLocationTaskTypes
                        .Select(x => x.WorkLocation)
                        .Where(wl => wl != null)
                        .SelectMany(wl => wl.Elections)
                        .Select(e => e.Id)
                        .FirstOrDefault()))
                .ForMember(dest => dest.ElectionTitle, opt => opt.MapFrom(src =>
                    src.WorkLocationTaskTypes
                        .Select(x => x.WorkLocation)
                        .Where(wl => wl != null)
                        .SelectMany(wl => wl.Elections)
                        .Select(e => e.Title)
                        .FirstOrDefault()));

            CreateMap<TaskTypeEntity, TaskTypeDetailResponse>()
                .ForMember(dest => dest.TaskTypeTemplateId, opt => opt
                .MapFrom(src => src.TaskTypeTemplateEntityId.ToString() ?? string.Empty))

                .ForMember(dest => dest.TaskTypeTemplateTitle, opt => opt
                .MapFrom(src => src.Title ?? string.Empty))

              .ForMember(dest => dest.ElectionId, opt => opt.MapFrom(src =>
                    src.WorkLocationTaskTypes
                        .Select(x => x.WorkLocation)
                        .Where(wl => wl != null)
                        .SelectMany(wl => wl.Elections)
                        .Select(e => e.Id)
                        .FirstOrDefault()))

                .ForMember(dest => dest.ElectionTitle, opt => opt.MapFrom(src =>
                    src.WorkLocationTaskTypes
                        .Select(x => x.WorkLocation)
                        .Where(wl => wl != null)
                        .SelectMany(wl => wl.Elections)
                        .Select(e => e.Title)
                        .FirstOrDefault()))

                .ForMember(dest => dest.WorkLocationId, opt => opt.MapFrom(src =>
                    src.WorkLocationTaskTypes
                        .Select(e => e.WorkLocation.Id)
                        .FirstOrDefault()))
                .ForMember(dest => dest.WorkLocationTitle, opt => opt.MapFrom(src =>
                    src.WorkLocationTaskTypes
                        .Select(e => e.WorkLocation.Title)
                        .FirstOrDefault()));

            CreateMap<TaskTypeEntity, TaskTypeSharedResponse>();
            CreateMap<TaskTypeEntity, TaskTypeWithAreaIdsResponse>()
                .ForMember(f => f.AreaIds, a => a.Ignore());
            CreateMap<TaskTypeEntity, TaskTypeWithTeamIdsResponse>()
                .ForMember(f => f.TeamIds, a => a.Ignore());
        }
    }
}
