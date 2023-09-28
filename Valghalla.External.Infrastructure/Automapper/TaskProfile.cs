using AutoMapper;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.Tasks.Responses;
using Valghalla.External.Application.Modules.Unprotected.Responses;

namespace Valghalla.External.Infrastructure.Automapper
{
    internal class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskAssignmentEntity, UnprotectedAvailableTasksDetailsResponse>()
                .ForMember(f => f.AvailableTasksCount, a => a.Ignore())
                .ForMember(f => f.TaskTypeName, opt => opt.MapFrom(src => src.TaskType.Title))
                .ForMember(f => f.TaskTypeDescription, opt => opt.MapFrom(src => src.TaskType.Description))
                .ForMember(f => f.TaskTypeStartTime, opt => opt.MapFrom(src => src.TaskType.StartTime))
                .ForMember(f => f.WorkLocationName, opt => opt.MapFrom(src => src.WorkLocation != null ? src.WorkLocation.Title : string.Empty))
                .ForMember(f => f.WorkLocationAddress, opt => opt.MapFrom(src => src.WorkLocation != null ? src.WorkLocation.Address : string.Empty))
                .ForMember(f => f.WorkLocationPostalCode, opt => opt.MapFrom(src => src.WorkLocation != null ? src.WorkLocation.PostalCode : string.Empty))
                .ForMember(f => f.WorkLocationCity, opt => opt.MapFrom(src => src.WorkLocation != null ? src.WorkLocation.City : string.Empty));

            CreateMap<TaskTypeEntity, TaskPreviewTaskType>();
            CreateMap<TaskTypeEntity, TeamResponsibleTaskPreviewTaskType>()
                .ForMember(f => f.UnansweredTasksCount, a => a.Ignore())
                .ForMember(f => f.AcceptedTasksCount, a => a.Ignore())
                .ForMember(f => f.AllTasksCount, a => a.Ignore());
            CreateMap<TaskTypeEntity, TaskTypeIncludeFiles>();

            CreateMap<TaskAssignmentEntity, TaskPreviewResponse>();

            CreateMap<TaskAssignmentEntity, TeamResponsibleTaskPreviewResponse>();

            CreateMap<TaskAssignmentEntity, TaskDetailsResponse>()
               .ForMember(dest => dest.TaskAssignmentId, opts => opts.MapFrom(src => src.Id))
               .ForMember(dest => dest.IsLocked, opts => opts.MapFrom(src => DateTime.Today.AddDays(src.Election.LockPeriod) >= src.TaskDate.Date ));

            CreateMap<TaskAssignmentEntity, TaskAssignmentResponse>()
                .ForMember(dest => dest.StartTime, opts => opts.MapFrom(src => src.TaskType.StartTime))
                .ForMember(dest => dest.EndTime, opts => opts.MapFrom(src => src.TaskType.EndTime));

            CreateMap<TaskAssignmentEntity, TeamResponsibleTaskDetailsResponse>()
                .ForMember(f => f.AcceptedTasksCount, a => a.Ignore())
                .ForMember(f => f.UnansweredTasksCount, a => a.Ignore())
                .ForMember(f => f.AllTasksCount, a => a.Ignore())
                .ForMember(f => f.TrustedTaskType, opt => opt.MapFrom(src => src.TaskType.Trusted))
                .ForMember(f => f.TaskTypeName, opt => opt.MapFrom(src => src.TaskType.Title))
                .ForMember(f => f.TaskTypeDescription, opt => opt.MapFrom(src => src.TaskType.Description))
                .ForMember(f => f.TaskTypeStartTime, opt => opt.MapFrom(src => src.TaskType.StartTime))
                .ForMember(f => f.WorkLocationName, opt => opt.MapFrom(src => src.WorkLocation != null ? src.WorkLocation.Title : string.Empty))
                .ForMember(f => f.WorkLocationAddress, opt => opt.MapFrom(src => src.WorkLocation != null ? src.WorkLocation.Address : string.Empty))
                .ForMember(f => f.WorkLocationPostalCode, opt => opt.MapFrom(src => src.WorkLocation != null ? src.WorkLocation.PostalCode : string.Empty))
                .ForMember(f => f.WorkLocationCity, opt => opt.MapFrom(src => src.WorkLocation != null ? src.WorkLocation.City : string.Empty));

            CreateMap<TaskAssignmentEntity, TaskOverviewItem>()
                .ForMember(f => f.AvailableTasksCount, a => a.Ignore())
                .ForMember(f => f.TaskTypeName, opt => opt.MapFrom(src => src.TaskType.Title))
                .ForMember(f => f.TaskTypeDescription, opt => opt.MapFrom(src => src.TaskType.Description))
                .ForMember(f => f.TaskTypeStartTime, opt => opt.MapFrom(src => src.TaskType.StartTime))
                .ForMember(f => f.WorkLocationName, opt => opt.MapFrom(src => src.WorkLocation != null ? src.WorkLocation.Title : string.Empty))
                .ForMember(f => f.WorkLocationAddress, opt => opt.MapFrom(src => src.WorkLocation != null ? src.WorkLocation.Address : string.Empty))
                .ForMember(f => f.WorkLocationPostalCode, opt => opt.MapFrom(src => src.WorkLocation != null ? src.WorkLocation.PostalCode : string.Empty))
                .ForMember(f => f.WorkLocationCity, opt => opt.MapFrom(src => src.WorkLocation != null ? src.WorkLocation.City : string.Empty));
        }
    }
}
