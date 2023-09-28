using AutoMapper;
using Valghalla.Application.Communication;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Infrastructure.Automapper
{
    internal class CommunicationProfile : Profile
    {
        public CommunicationProfile()
        {
            CreateMap<CommunicationLogEntity, CommunicationLogListingItem>()
                .ForMember(dest => dest.ParticipantName, opts => opts.MapFrom(src => src.Participant.FirstName + " " + src.Participant.LastName))
                .ForMember(dest => dest.Status, opts => opts.MapFrom(src => CommunicationLogStatus.Parse(src.Status)))
                .ForMember(dest => dest.MessageType, opts => opts.MapFrom(src => CommunicationLogMessageType.Parse(src.MessageType)))
                .ForMember(dest => dest.SendType, opts => opts.MapFrom(src => CommunicationLogSendType.Parse(src.SendType)));

            CreateMap<CommunicationLogEntity, CommunicationLogDetails>()
                .ForMember(dest => dest.ParticipantName, opts => opts.MapFrom(src => src.Participant.FirstName + " " + src.Participant.LastName))
                .ForMember(dest => dest.Status, opts => opts.MapFrom(src => CommunicationLogStatus.Parse(src.Status)))
                .ForMember(dest => dest.MessageType, opts => opts.MapFrom(src => CommunicationLogMessageType.Parse(src.MessageType)))
                .ForMember(dest => dest.SendType, opts => opts.MapFrom(src => CommunicationLogSendType.Parse(src.SendType)));

            CreateMap<CommunicationTemplateEntity, CommunicationTemplate>()
                .ForMember(dest => dest.FileRefIds, opts => opts.MapFrom(src => src.CommunicationTemplateFileReferences.Select(i => i.Id).ToArray()));
            CreateMap<TaskAssignmentEntity, TaskAssignmentCommunicationInfo>()
                .ForMember(dest => dest.Active, opts => opts.MapFrom(src => src.Election.Active));
        }
    }
}
