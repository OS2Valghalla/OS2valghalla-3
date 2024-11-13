using AutoMapper;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Participant.Responses;
using Valghalla.Internal.Application.Modules.Tasks.Responses;
using TaskStatus = Valghalla.Application.Enums.TaskStatus;

namespace Valghalla.Internal.Infrastructure.Automapper
{
    internal class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskAssignmentEntity, TaskAssignmentResponse>()
                .ForMember(f => f.TaskDetailsPageUrl, a => a.Ignore())
                .ForMember(f => f.TaskTypeName, opt => opt.MapFrom(src => src.TaskType.Title))
                .ForMember(f => f.ParticipantName, opt => opt.MapFrom(src => src.Participant != null ? src.Participant.FirstName + " " + src.Participant.LastName : string.Empty))
                .ForMember(f => f.WorkLocationName, opt => opt.MapFrom(src => src.WorkLocation != null ? src.WorkLocation.Title : string.Empty))
                .ForMember(f => f.TeamName, opt => opt.MapFrom(src => src.Team != null ? src.Team.Name : string.Empty));

            CreateMap<RejectedTaskAssignmentEntity, TaskAssignmentResponse>()
                .ForMember(f => f.TaskDetailsPageUrl, a => a.Ignore())
                .ForMember(f => f.Accepted, a => a.Ignore())
                .ForMember(f => f.Responsed, a => a.Ignore())
                .ForMember(f => f.WorkLocationName, a => a.Ignore())
                .ForMember(f => f.TeamName, a => a.Ignore())
                .ForMember(f => f.TaskTypeName, opt => opt.MapFrom(src => src.TaskType.Title))
                .ForMember(f => f.ParticipantName, opt => opt.MapFrom(src => src.Participant.FirstName + " " + src.Participant.LastName));

            CreateMap<TaskAssignmentEntity, AvailableTasksDetailsResponse>()
                .ForMember(f => f.AvailableTasksCount, a => a.Ignore())
                .ForMember(f => f.TaskDetailsPageUrl, a => a.Ignore())
                .ForMember(f => f.TaskTypeName, opt => opt.MapFrom(src => src.TaskType.Title))
                .ForMember(f => f.TaskTypeDescription, opt => opt.MapFrom(src => src.TaskType.Description))
                .ForMember(f => f.TaskTypeStartTime, opt => opt.MapFrom(src => src.TaskType.StartTime))
                .ForMember(f => f.TrustedTask, opt => opt.MapFrom(src => src.TaskType.Trusted))
                .ForMember(f => f.WorkLocationName, opt => opt.MapFrom(src => src.WorkLocation != null ? src.WorkLocation.Title : string.Empty))
                .ForMember(f => f.WorkLocationAddress, opt => opt.MapFrom(src => src.WorkLocation != null ? src.WorkLocation.Address : string.Empty))
                .ForMember(f => f.WorkLocationPostalCode, opt => opt.MapFrom(src => src.WorkLocation != null ? src.WorkLocation.PostalCode : string.Empty))
                .ForMember(f => f.WorkLocationCity, opt => opt.MapFrom(src => src.WorkLocation != null ? src.WorkLocation.City : string.Empty));

            CreateMap<TaskAssignmentEntity, ParticipantTaskResponse>()
                .ForMember(f => f.ElectionName, opt => opt.MapFrom(src => src.Election.Title))
                .ForMember(f => f.TaskTypeName, opt => opt.MapFrom(src => src.TaskType.Title))
                .ForMember(f => f.WorkLocationName, opt => opt.MapFrom(src => src.WorkLocation != null ? src.WorkLocation.Title : string.Empty))
                .ForMember(f => f.TeamName, opt => opt.MapFrom(src => src.Team != null ? src.Team.Name : string.Empty))
                .ForMember(f => f.Status, opt => opt.MapFrom(src => src.Accepted ? TaskStatus.Accepted.ToString() : Valghalla.Application.Enums.TaskStatus.Unanswered.ToString()));

            CreateMap<RejectedTaskAssignmentEntity, ParticipantTaskResponse>()
                .ForMember(f => f.ElectionName, opt => opt.MapFrom(src => src.Election.Title))
                .ForMember(f => f.TaskTypeName, opt => opt.MapFrom(src => src.TaskType.Title))
                .ForMember(f => f.WorkLocationName, opt => opt.MapFrom(src => src.WorkLocation != null ? src.WorkLocation.Title : string.Empty))
                .ForMember(f => f.TeamName, opt => opt.MapFrom(src => src.Team != null ? src.Team.Name : string.Empty))
                .ForMember(f => f.Status, opt => opt.MapFrom(src => TaskStatus.Rejected.ToString()));

            CreateMap<TaskAssignmentEntity, ParticipantTaskDetailsResponse>()
                .ForMember(f => f.ParticipantName, opt => opt.MapFrom(src => src.Participant != null ? src.Participant.FirstName + " " + src.Participant.LastName : null))
                .ForMember(f => f.ParticipantCpr, opt => opt.MapFrom(src => src.Participant!.Cpr))
                .ForMember(f => f.ParticipantAge, opt => opt.MapFrom(src => src.Participant!.Age))
                .ForMember(f => f.ParticipantDigitalPostStatus, opt => opt.MapFrom(src => !src.Participant!.ExemptDigitalPost))
                .ForMember(f => f.ParticipantPhoneNumber, opt => opt.MapFrom(src => src.Participant!.MobileNumber))
                .ForMember(f => f.ParticipantEmail, opt => opt.MapFrom(src => src.Participant!.Email))
                .ForMember(f => f.ParticipantAddress, opt => opt.MapFrom(src => string.Join(", ", (new List<string>() { src.Participant!.CoAddress, src.Participant!.StreetAddress, src.Participant!.PostalCode, src.Participant!.City }).Where(s => !string.IsNullOrEmpty(s)))))
                .ForMember(f => f.ParticipantSpecialDiets, opt => opt.MapFrom(src => string.Join(", ", src.Participant!.SpecialDiets!.Select(x => x.Title!))))
                .ForMember(f => f.ParticipantBirthDate, opt => opt.MapFrom(src => src.Participant!.Birthdate))
                .ForMember(f => f.ParticipantUserName, opt => opt.MapFrom(src => src.Participant!.User!.Name))
                .ForMember(f => f.AreaName, opt => opt.MapFrom(src => src.WorkLocation!.Area!.Name))
                .ForMember(f => f.TeamName, opt => opt.MapFrom(src => src.Team!.Name))
                .ForMember(f => f.TaskTypeName, opt => opt.MapFrom(src => src.TaskType!.Title))
                .ForMember(f => f.TaskDate, opt => opt.MapFrom(src => src.TaskDate))
                .ForMember(f => f.WorkLocation, opt => opt.MapFrom(src => src.WorkLocation.Title))
                .ForMember(f => f.VotingArea, opt => opt.MapFrom(src => src.WorkLocation!.VoteLocation.ToString()))
                .ForMember(f => f.TaskStartTime, opt => opt.MapFrom(src => src.TaskType!.StartTime))
                .ForMember(f => f.TaskPayment, opt => opt.MapFrom(src => src.TaskType!.Payment))
                .ForMember(f => f.TaskStatus, opt => opt.MapFrom(src => src.Accepted ? TaskStatus.Accepted.ToString() : (src.ParticipantId.HasValue ? TaskStatus.Unanswered.ToString() : TaskStatus.Available.ToString())))
                ;
        }
    }
}
