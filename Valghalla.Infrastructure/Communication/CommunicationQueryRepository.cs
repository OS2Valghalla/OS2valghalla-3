using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Valghalla.Application.Communication;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Infrastructure.Communication
{
    internal class CommunicationQueryRepository : ICommunicationQueryRepository
    {
        private readonly IMapper mapper;

        private readonly IQueryable<ElectionEntity> elections;
        private readonly IQueryable<TaskTypeEntity> taskTypes;
        private readonly IQueryable<ParticipantEntity> participants;
        private readonly IQueryable<TaskAssignmentEntity> taskAssignments;
        private readonly IQueryable<RejectedTaskAssignmentEntity> rejectedTaskAssignments;
        private readonly IQueryable<CommunicationTemplateEntity> communicationTemplates;
        private readonly IQueryable<ElectionCommitteeContactInformationEntity> electionCommitteeContacts;

        public CommunicationQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            elections = dataContext.Set<ElectionEntity>().AsNoTracking();
            taskTypes = dataContext.Set<TaskTypeEntity>().AsNoTracking();
            participants = dataContext.Set<ParticipantEntity>().AsNoTracking();
            taskAssignments = dataContext.Set<TaskAssignmentEntity>().AsNoTracking();
            rejectedTaskAssignments = dataContext.Set<RejectedTaskAssignmentEntity>().AsNoTracking();
            communicationTemplates = dataContext.Set<CommunicationTemplateEntity>().AsNoTracking();
            electionCommitteeContacts = dataContext.Set<ElectionCommitteeContactInformationEntity>().AsNoTracking();
        }

        public async Task<TaskAssignmentCommunicationInfo?> GetTaskAssignmentCommunicationInfoAsync(Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var entity = await taskAssignments
                .Include(i => i.Election)
                .Where(i => i.Id == taskAssignmentId)
                .SingleOrDefaultAsync(cancellationToken);

            return mapper.Map<TaskAssignmentCommunicationInfo>(entity);
        }

        public async Task<CommunicationRelatedInfo?> GetCommunicationRelatedInfoAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var entity = await taskAssignments
                .Include(i => i.Election)
                .Include(i => i.WorkLocation)
                .Include(i => i.TaskType)
                .Where(i => i.Id == taskAssignmentId)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null) return null;

            var participantEntity = await participants
                .Where(i => i.Id == participantId)
                .Select(i => new { i.Id, i.FirstName, i.LastName, i.MobileNumber, i.Email, i.Cpr, i.ExemptDigitalPost })
                .SingleOrDefaultAsync(cancellationToken);

            if (participantEntity == null) return null;

            var municipalityName = await GetMunicipalityNameAsync(cancellationToken);

            return new CommunicationRelatedInfo()
            {
                HashValue = entity.HashValue,
                Participant = new CommunicationParticipantInfo()
                {
                    Id = participantEntity.Id,
                    Name = participantEntity.FirstName + " " + participantEntity.LastName,
                    MobileNumber = participantEntity.MobileNumber,
                    Email = participantEntity.Email,
                    Cpr = participantEntity.Cpr,
                    ExemptDigitalPost = participantEntity.ExemptDigitalPost,
                },
                MunicipalityName = municipalityName,
                ElectionTitle = entity.Election.Title,
                WorkLocation = new CommunicationWorkLocationInfo()
                {
                    Title = entity.WorkLocation.Title,
                    Address = entity.WorkLocation.Address,
                },
                TaskType = new CommunicationTaskTypeInfo()
                {
                    Title = entity.TaskType.Title,
                    Description = entity.TaskType.Description,
                    Payment = entity.TaskType.Payment,
                    StartTime = entity.TaskType.StartTime,
                    EndTime = entity.TaskType.EndTime,
                },
                TaskDate = entity.TaskDate,
                InvitationCode = entity.InvitationCode
            };
        }

        public async Task<CommunicationRelatedInfo?> GetRejectedTaskInfoAsync(Guid rejectedTaskId, CancellationToken cancellationToken)
        {
            var entity = await rejectedTaskAssignments
                .Include(i => i.Election)
                .Include(i => i.WorkLocation)
                .Include(i => i.TaskType)
                .Include(i => i.Participant)
                .Where(i => i.Id == rejectedTaskId)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null) return null;

            var municipalityName = await GetMunicipalityNameAsync(cancellationToken);

            return new CommunicationRelatedInfo()
            {
                Participant = new CommunicationParticipantInfo()
                {
                    Id = entity.Participant.Id,
                    Name = entity.Participant.FirstName + " " + entity.Participant.LastName,
                    MobileNumber = entity.Participant.MobileNumber,
                    Email = entity.Participant.Email,
                    Cpr = entity.Participant.Cpr,
                    ExemptDigitalPost = entity.Participant.ExemptDigitalPost
                },
                MunicipalityName = municipalityName,
                ElectionTitle = entity.Election.Title,
                WorkLocation = new CommunicationWorkLocationInfo()
                {
                    Title = entity.WorkLocation.Title,
                    Address = entity.WorkLocation.Address,
                },
                TaskType = new CommunicationTaskTypeInfo()
                {
                    Title = entity.TaskType.Title,
                    Description = entity.TaskType.Description,
                    Payment = entity.TaskType.Payment,
                    StartTime = entity.TaskType.StartTime,
                    EndTime = entity.TaskType.EndTime,
                },
                TaskDate = entity.TaskDate
            };
        }

        public async Task<CommunicationTemplate?> GetTaskInvitationCommunicationTemplateAsync(Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            return await GetCommunicationTemplateAsync(CommunicationType.TaskInvitation, taskAssignmentId, cancellationToken);
        }

        public async Task<CommunicationTemplate?> GetRemovedFromTaskCommunicationTemplateAsync(Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            return await GetCommunicationTemplateAsync(CommunicationType.RemovedFromTask, taskAssignmentId, cancellationToken);
        }

        public async Task<CommunicationTemplate?> GetRemovedFromTaskByValidationCommunicationTemplateAsync(Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            return await GetCommunicationTemplateAsync(CommunicationType.RemovedByValidation, taskAssignmentId, cancellationToken);
        }

        public async Task<CommunicationTemplate?> GetTaskRegistrationCommunicationTemplateAsync(Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            return await GetCommunicationTemplateAsync(CommunicationType.TaskRegistration, taskAssignmentId, cancellationToken);
        }

        public async Task<CommunicationTemplate?> GetTaskCancellationCommunicationTemplateAsync(Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            return await GetCommunicationTemplateAsync(CommunicationType.TaskCancellation, taskAssignmentId, cancellationToken);
        }

        public async Task<CommunicationTemplate?> GetTaskInvitationReminderCommunicationTemplateAsync(Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            return await GetCommunicationTemplateAsync(CommunicationType.TaskInvitationReminder, taskAssignmentId, cancellationToken);
        }

        public async Task<CommunicationTemplate?> GetTaskReminderCommunicationTemplateAsync(Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            return await GetCommunicationTemplateAsync(CommunicationType.TaskReminder, taskAssignmentId, cancellationToken);
        }

        public async Task<CommunicationTemplate?> GetTaskRetractedInvitationCommunicationTemplateAsync(Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            return await GetCommunicationTemplateAsync(CommunicationType.TaskRetractedInvitation, taskAssignmentId, cancellationToken);
        }
        public async Task<CommunicationTemplate?> GetCommunicationTemplateAsync(Guid templateId, CancellationToken cancellationToken)
        {
            var communicationTemplate = await communicationTemplates
                .Include(i => i.CommunicationTemplateFileReferences)
                .Where(i => i.Id == templateId)
                .SingleOrDefaultAsync(cancellationToken);

            return mapper.Map<CommunicationTemplate>(communicationTemplate);
        }
        public async Task<CommunicationParticipantInfo> GetParticipantAsync(Guid participantId, CancellationToken cancellationToken)
        {
            var query = await participants
                .Where(i => i.Id == participantId)
                .Select(i => new { i.Id, i.FirstName, i.LastName, i.MobileNumber, i.Email, i.Cpr, i.ExemptDigitalPost })
                .SingleOrDefaultAsync(cancellationToken);

            if (query == null) return null!;
            return new CommunicationParticipantInfo
            {
                Id = query.Id,
                Name = query.FirstName + " " + query.LastName,
                MobileNumber = query.MobileNumber,
                Email = query.Email,
                Cpr = query.Cpr,
                ExemptDigitalPost = query.ExemptDigitalPost
            };

        }

        private async Task<string> GetMunicipalityNameAsync(CancellationToken cancellationToken)
        {
            var contactInfo = await electionCommitteeContacts
                .Select(i => new { i.MunicipalityName })
                .FirstOrDefaultAsync(cancellationToken);

            if (contactInfo == null || string.IsNullOrEmpty(contactInfo.MunicipalityName))
            {
                return string.Empty;
            }

            return contactInfo.MunicipalityName;
        }

        private async Task<CommunicationTemplate?> GetCommunicationTemplateAsync(CommunicationType type, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var taskAssginment = await taskAssignments
                .Where(i => i.Id == taskAssignmentId)
                .Select(i => new { i.Id, i.TaskTypeId, i.ElectionId })
                .SingleOrDefaultAsync(cancellationToken);

            if (taskAssginment == null) return null;

            var taskType = await taskTypes
                .Include(i => i.ElectionTaskTypeCommunicationTemplates)
                .Where(i => i.Id == taskAssginment.TaskTypeId)
                .Select(i => new { i.Id, i.ElectionTaskTypeCommunicationTemplates })
                .SingleOrDefaultAsync(cancellationToken);

            if (taskType == null) return null;

            Guid? templateId = null;
            var taskTypeTemplateBinding = taskType.ElectionTaskTypeCommunicationTemplates.SingleOrDefault(i => i.ElectionId == taskAssginment.ElectionId);

            if (taskTypeTemplateBinding != null)
            {
                templateId = GetTemplateId(type, taskTypeTemplateBinding);
            }

            if (templateId == null)
            {
                var election = await elections
                    .Where(i => i.Id == taskAssginment.ElectionId)
                    .SingleOrDefaultAsync(cancellationToken);

                if (election == null) return null;

                templateId = GetTemplateId(type, election);
            }

            var communicationTemplate = await communicationTemplates
                .Include(i => i.CommunicationTemplateFileReferences)
                .Where(i => i.Id == templateId)
                .SingleOrDefaultAsync(cancellationToken);

            return mapper.Map<CommunicationTemplate>(communicationTemplate);
        }

        private static Guid? GetTemplateId(CommunicationType type, ElectionTaskTypeCommunicationTemplateEntity binding) =>
            type switch
            {
                CommunicationType.TaskInvitation => binding.InvitationCommunicationTemplateId,
                CommunicationType.TaskRegistration => binding.ConfirmationOfRegistrationCommunicationTemplateId,
                CommunicationType.TaskCancellation => binding.ConfirmationOfCancellationCommunicationTemplateId,
                CommunicationType.TaskInvitationReminder => binding.InvitationReminderCommunicationTemplateId,
                CommunicationType.TaskReminder => binding.TaskReminderCommunicationTemplateId,
                CommunicationType.TaskRetractedInvitation => binding.RetractedInvitationCommunicationTemplateId,
                CommunicationType.RemovedFromTask => binding.RemovedFromTaskCommunicationTemplateId,
                CommunicationType.RemovedByValidation => binding.RemovedByValidationCommunicationTemplateId,
                _ => null
            };

        private static Guid? GetTemplateId(CommunicationType type, ElectionEntity election) =>
            type switch
            {
                CommunicationType.TaskInvitation => election.InvitationCommunicationTemplateId,
                CommunicationType.TaskRegistration => election.ConfirmationOfRegistrationCommunicationTemplateId,
                CommunicationType.TaskCancellation => election.ConfirmationOfCancellationCommunicationTemplateId,
                CommunicationType.TaskInvitationReminder => election.InvitationReminderCommunicationTemplateId,
                CommunicationType.TaskReminder => election.TaskReminderCommunicationTemplateId,
                CommunicationType.TaskRetractedInvitation => election.RetractedInvitationCommunicationTemplateId,
                CommunicationType.RemovedFromTask => election.RemovedFromTaskCommunicationTemplateId,
                CommunicationType.RemovedByValidation => election.RemovedByValidationCommunicationTemplateId,
                _ => null
            };
    }
}
