using Valghalla.Application.Communication;
using Valghalla.Application.Tenant;

namespace Valghalla.Integration.Communication
{
    internal class CommunicationHelper : ICommunicationHelper
    {
        private readonly ITenantContextProvider tenantContextProvider;
        private readonly ICommunicationQueryRepository communicationQueryRepository;

        public CommunicationHelper(ITenantContextProvider tenantContextProvider, ICommunicationQueryRepository communicationQueryRepository)
        {
            this.tenantContextProvider = tenantContextProvider;
            this.communicationQueryRepository = communicationQueryRepository;
        }

        public async Task<bool> ValidateTaskInvitationAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var taskAssignment = await communicationQueryRepository.GetTaskAssignmentCommunicationInfoAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Task assignment no longer exists (taskAssignmentId = {taskAssignmentId})");

            if (!taskAssignment.Active) return false;

            if (!taskAssignment.ParticipantId.HasValue)
            {
                throw new Exception($"Task assignment has no participant assigned (taskAssignmentId = {taskAssignmentId})");
            }

            if (taskAssignment.ParticipantId.Value != participantId)
            {
                throw new Exception($"Task assignment's participant and target participant are not the same (taskAssignmentId = {taskAssignmentId})");
            }

            if (taskAssignment.InvitationSent)
            {
                throw new Exception($"Task assignment invitation already sent (taskAssignmentId = {taskAssignmentId})");
            }

            if (taskAssignment.Responsed)
            {
                throw new Exception($"Task assignment already responded (taskAssignmentId = {taskAssignmentId})");
            }

            return true;
        }

        public async Task<bool> ValidateRemovedFromTaskAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var taskAssignment = await communicationQueryRepository.GetTaskAssignmentCommunicationInfoAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Task assignment no longer exists (taskAssignmentId = {taskAssignmentId})");

            if (!taskAssignment.Active) return false;

            return true;
        }

        public async Task<bool> ValidateRemovedFromTaskByValidationAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var taskAssignment = await communicationQueryRepository.GetTaskAssignmentCommunicationInfoAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Task assignment no longer exists (taskAssignmentId = {taskAssignmentId})");

            if (!taskAssignment.Active) return false;

            return true;
        }

        public async Task<bool> ValidateTaskInvitationReminderAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var taskAssignment = await communicationQueryRepository.GetTaskAssignmentCommunicationInfoAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Task assignment no longer exists (taskAssignmentId = {taskAssignmentId})");

            if (!taskAssignment.Active) return false;

            if (!taskAssignment.ParticipantId.HasValue)
            {
                throw new Exception($"Task assignment has no participant assigned (taskAssignmentId = {taskAssignmentId})");
            }

            if (taskAssignment.ParticipantId.Value != participantId)
            {
                throw new Exception($"Task assignment's participant and target participant are not the same (taskAssignmentId = {taskAssignmentId})");
            }

            if (!taskAssignment.InvitationSent)
            {
                throw new Exception($"Task assignment invitation hasn't been sent (taskAssignmentId = {taskAssignmentId})");
            }

            if (taskAssignment.Responsed)
            {
                throw new Exception($"Task assignment already responded (taskAssignmentId = {taskAssignmentId})");
            }

            return true;
        }

        public async Task<bool> ValidateTaskReminderAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var taskAssignment = await communicationQueryRepository.GetTaskAssignmentCommunicationInfoAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Task assignment no longer exists (taskAssignmentId = {taskAssignmentId})");

            if (!taskAssignment.Active) return false;

            if (!taskAssignment.ParticipantId.HasValue)
            {
                throw new Exception($"Task assignment has no participant assigned (taskAssignmentId = {taskAssignmentId})");
            }

            if (taskAssignment.ParticipantId.Value != participantId)
            {
                throw new Exception($"Task assignment's participant and target participant are not the same (taskAssignmentId = {taskAssignmentId})");
            }

            if (!taskAssignment.InvitationSent)
            {
                throw new Exception($"Task assignment invitation hasn't been sent (taskAssignmentId = {taskAssignmentId})");
            }

            if (!taskAssignment.Responsed)
            {
                throw new Exception($"Task assignment hasn't been responded (taskAssignmentId = {taskAssignmentId})");
            }

            if (!taskAssignment.Accepted)
            {
                throw new Exception($"Task assignment hasn't been accepted (taskAssignmentId = {taskAssignmentId})");
            }

            if (taskAssignment.ReminderDate.HasValue)
            {
                throw new Exception($"Task assignment reminder has been sent (taskAssignmentId = {taskAssignmentId})");
            }

            return true;
        }

        public async Task<bool> ValidateTaskReminderParticipantAsync(Guid participantId, int templateType, CancellationToken cancellationToken)
        {
            var participant = await communicationQueryRepository.GetParticipantAsync(participantId, cancellationToken).ConfigureAwait(false);

            if (templateType is TemplateType.DigitalPost && participant.ExemptDigitalPost is true) return false;
            if (templateType is TemplateType.Email && participant.Email is null) return false;
            if (templateType is TemplateType.SMS && participant.MobileNumber is null) return false;

            return true;
        }

        public async Task<bool> ValidateTaskRegistrationAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var taskAssignment = await communicationQueryRepository.GetTaskAssignmentCommunicationInfoAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Task assignment no longer exists (taskAssignmentId = {taskAssignmentId})");

            if (!taskAssignment.Active) return false;

            if (!taskAssignment.ParticipantId.HasValue)
            {
                throw new Exception($"Task assignment has no participant assigned (taskAssignmentId = {taskAssignmentId})");
            }

            if (taskAssignment.ParticipantId.Value != participantId)
            {
                throw new Exception($"Task assignment's participant and target participant are not the same (taskAssignmentId = {taskAssignmentId})");
            }

            if (taskAssignment.RegistrationSent)
            {
                throw new Exception($"Task assignment registration already sent (taskAssignmentId = {taskAssignmentId})");
            }

            if (!taskAssignment.Responsed || !taskAssignment.Accepted)
            {
                throw new Exception($"Task assignment is not responsed or not accepted (taskAssignmentId = {taskAssignmentId})");
            }

            return true;
        }

        public async Task<bool> ValidateTaskInvitationRetractedAsync(Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var taskAssignment = await communicationQueryRepository.GetTaskAssignmentCommunicationInfoAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Task assignment no longer exists (taskAssignmentId = {taskAssignmentId})");

            if (!taskAssignment.Active) return false;

            return true;
        }

        public string ReplaceTokens(string template, CommunicationRelatedInfo info, bool htmlFormatLinks)
        {
            if (string.IsNullOrEmpty(template)) return string.Empty;

            var externalWebLink = tenantContextProvider.CurrentTenant.ExternalDomain;
            var contactLink = tenantContextProvider.CurrentTenant.ExternalDomain + "/kontakt-os";
            var invitationLink = tenantContextProvider.CurrentTenant.ExternalDomain + $"/opgaver/invitation/{info.HashValue}/{info.InvitationCode ?? Guid.Empty}";

            var externalWebLinkHTML = externalWebLink;
            var contactLinkHTML = contactLink;
            var invitationLinkHTML = invitationLink;

            if (htmlFormatLinks)
            {
                externalWebLinkHTML = "<a href=\"" + externalWebLink + "\">" + externalWebLink + "</a>";
                contactLinkHTML = "<a href=\"" + contactLink + "\">" + contactLink + "</a>";
                invitationLinkHTML = "<a href=\"" + invitationLink + "\">" + invitationLink + "</a>";
            }

            return template
                .Replace("!name", info.Participant.Name)
                .Replace("!election", info.ElectionTitle)
                .Replace("!work_location_address", info.WorkLocation.Address)
                .Replace("!work_location", info.WorkLocation.Title)
                .Replace("!task_type_description", info.TaskType.Description)
                .Replace("!task_type", info.TaskType.Title)
                .Replace("!task_date", $"{PadTimeValue(info.TaskDate.ToLocalTime().Day)}/{PadTimeValue(info.TaskDate.ToLocalTime().Month)}/{PadTimeValue(info.TaskDate.ToLocalTime().Year)}")
                .Replace("!task_start", $"{PadTimeValue(info.TaskType.StartTime.Hours)}:{PadTimeValue(info.TaskType.StartTime.Minutes)}")
                .Replace("!task_end", $"{PadTimeValue(info.TaskType.EndTime.Hours)}:{PadTimeValue(info.TaskType.EndTime.Minutes)}")
                .Replace("!payment", info.TaskType.Payment.HasValue ? $"{info.TaskType.Payment} kr." : string.Empty)
                .Replace("!days", (info.TaskDate - DateTime.UtcNow).Days.ToString())
                .Replace("!municipality", info.MunicipalityName)
                .Replace("!invitation", invitationLinkHTML)
                .Replace("!contact", contactLinkHTML)
                .Replace("!external_web", externalWebLinkHTML);
        }

        private static string PadTimeValue(int value) => value.ToString().PadLeft(2, '0');
    }
}
