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

            CreateMap<TaskTypeTemplateEntity, TaskTypeEntity>()
             .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
             .ForMember(dest => dest.ShortName, opt => opt.MapFrom(src => src.ShortName))
             .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
             .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
             .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
             .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => src.Payment))
             .ForMember(dest => dest.ValidationNotRequired, opt => opt.MapFrom(src => src.ValidationNotRequired))
             .ForMember(dest => dest.Trusted, opt => opt.MapFrom(src => src.Trusted))
             .ForMember(dest => dest.SendingReminderEnabled, opt => opt.MapFrom(src => src.SendingReminderEnabled))
             .ForMember(dest => dest.TaskTypeTemplateEntityId, opt => opt.MapFrom(src => src.Id));
            
        }
    }
}
