using Valghalla.Application.Communication;
using Valghalla.Application.Configuration;
using Valghalla.Application.Queue;
using Valghalla.Application.Queue.Messages;

namespace Valghalla.Integration.Communication
{
    internal class CommunicationService : ICommunicationService
    {
        private readonly IQueueService queueService;
        private readonly AppConfiguration appConfiguration;
        private readonly ICommunicationHelper communicationHelper;
        private readonly ICommunicationQueryRepository communicationQueryRepository;
        private readonly ICommunicationLogRepository communicationLogRepository;

        public CommunicationService(
            IQueueService queueService,
            ICommunicationHelper communicationHelper,
            ICommunicationQueryRepository communicationQueryRepository,
            ICommunicationLogRepository communicationLogRepository,
            AppConfiguration appConfiguration)
        {
            this.appConfiguration = appConfiguration;
            this.queueService = queueService;
            this.communicationHelper = communicationHelper;
            this.communicationQueryRepository = communicationQueryRepository;
            this.communicationLogRepository = communicationLogRepository;
        }

        public async Task SendTaskInvitationAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var valid = await communicationHelper.ValidateTaskInvitationAsync(participantId, taskAssignmentId, cancellationToken);

            if (!valid) return;

            var template = await communicationQueryRepository.GetTaskInvitationCommunicationTemplateAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Errors occurred when fetching task invitation communication template (taskAssignmentId = {taskAssignmentId})");

            var logId = await WriteCommunicationLogAsync(participantId, taskAssignmentId, template, cancellationToken);

            await queueService.PublishLoggedJobAsync(logId, new TaskInvitationJobMessage()
            {
                ParticipantId = participantId,
                TaskAssignmentId = taskAssignmentId
            }, cancellationToken);
        }

        public async Task SendRemovedFromTaskAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var valid = await communicationHelper.ValidateRemovedFromTaskAsync(participantId, taskAssignmentId, cancellationToken);

            if (!valid) return;

            var template = await communicationQueryRepository.GetRemovedFromTaskCommunicationTemplateAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Errors occurred when fetching removed from task communication template (taskAssignmentId = {taskAssignmentId})");

            var logId = await WriteCommunicationLogAsync(participantId, taskAssignmentId, template, cancellationToken);

            await queueService.PublishLoggedJobAsync(logId, new RemovedFromTaskJobMessage()
            {
                ParticipantId = participantId,
                TaskAssignmentId = taskAssignmentId
            }, cancellationToken);
        }

        public async Task SendRemovedFromTaskByValidationAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var valid = await communicationHelper.ValidateRemovedFromTaskByValidationAsync(participantId, taskAssignmentId, cancellationToken);

            if (!valid) return;

            var template = await communicationQueryRepository.GetRemovedFromTaskByValidationCommunicationTemplateAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Errors occurred when fetching removed from task by validation communication template (taskAssignmentId = {taskAssignmentId})");

            var logId = await WriteCommunicationLogAsync(participantId, taskAssignmentId, template, cancellationToken);

            await queueService.PublishLoggedJobAsync(logId, new RemovedFromTaskByValidationJobMessage()
            {
                ParticipantId = participantId,
                TaskAssignmentId = taskAssignmentId
            }, cancellationToken);
        }

        public async Task SendTaskRegistrationAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var valid = await communicationHelper.ValidateTaskRegistrationAsync(participantId, taskAssignmentId, cancellationToken);

            if (!valid) return;

            var template = await communicationQueryRepository.GetTaskRegistrationCommunicationTemplateAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Errors occurred when fetching task invitation communication template (taskAssignmentId = {taskAssignmentId})");

            var logId = await WriteCommunicationLogAsync(participantId, taskAssignmentId, template, cancellationToken);

            await queueService.PublishLoggedJobAsync(logId, new TaskRegistrationJobMessage()
            {
                ParticipantId = participantId,
                TaskAssignmentId = taskAssignmentId
            }, cancellationToken);
        }

        public async Task SendTaskCancellationAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var template = await communicationQueryRepository.GetTaskCancellationCommunicationTemplateAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Errors occurred when fetching task cancellation communication template (taskAssignmentId = {taskAssignmentId})");

            var logId = await WriteCommunicationLogAsync(participantId, taskAssignmentId, template, cancellationToken);

            await queueService.PublishLoggedJobAsync(logId, new TaskCancellationJobMessage()
            {
                ParticipantId = participantId,
                TaskAssignmentId = taskAssignmentId
            }, cancellationToken);
        }

        public async Task SendTaskInvitationRetractedAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var valid = await communicationHelper.ValidateTaskInvitationRetractedAsync(taskAssignmentId, cancellationToken);

            if (!valid) return;

            var template = await communicationQueryRepository.GetTaskRetractedInvitationCommunicationTemplateAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Errors occurred when fetching task retracted invitation communication template (taskAssignmentId = {taskAssignmentId})");

            var logId = await WriteCommunicationLogAsync(participantId, taskAssignmentId, template, cancellationToken);

            await queueService.PublishLoggedJobAsync(logId, new TaskInvitationRetractedJobMessage()
            {
                ParticipantId = participantId,
                TaskAssignmentId = taskAssignmentId
            }, cancellationToken);
        }

        public async Task SendGroupMessageAsync(IList<CommunicationTaskParticipantInfo> tasks, int templateType, string templateSubject, string templateContent, List<Guid> fileReferenceIds, CancellationToken cancellationToken)
        {
            foreach (var task in tasks)
            {
                var info = await (task.IsRejectedTask ? communicationQueryRepository.GetRejectedTaskInfoAsync(task.TaskId, cancellationToken) : communicationQueryRepository.GetCommunicationRelatedInfoAsync(task.ParticipantId, task.TaskId, cancellationToken))
                    ?? (task.IsRejectedTask ? throw new Exception($"Errors occurred when fetching rejected task related information (taskId = {task.TaskId})") : throw new Exception($"Errors occurred when fetching assigned task related information (taskId = {task.TaskId})"));

                var logId = await WriteCommunicationLogAsync(info, task.ParticipantId, CommunicationLogSendType.Manual, templateType, templateSubject, templateContent, cancellationToken);

                await queueService.PublishLoggedJobAsync(logId, new SendGroupMessageJobMessage()
                {
                    ParticipantId = task.ParticipantId,
                    TaskId = task.TaskId,
                    IsRejectedTask = task.IsRejectedTask,
                    TemplateType = templateType,
                    TemplateSubject = templateSubject,
                    TemplateContent = templateContent,
                    TemplateFileReferenceIds = fileReferenceIds
                }, cancellationToken);
            }
        }

        private async Task<Guid> WriteCommunicationLogAsync(Guid participantId, Guid taskAssignmentId, CommunicationTemplate template, CancellationToken cancellationToken)
        {
            return await WriteCommunicationLogAsync(participantId, taskAssignmentId, CommunicationLogSendType.Automatic, template.TemplateType, template.Subject, template.Content, cancellationToken);
        }

        private async Task<Guid> WriteCommunicationLogAsync(Guid participantId, Guid taskAssignmentId, CommunicationLogSendType sendType, int templateType, string templateSubject, string templateContent, CancellationToken cancellationToken)
        {
            var info = await communicationQueryRepository.GetCommunicationRelatedInfoAsync(participantId, taskAssignmentId, cancellationToken);

            if (info == null)
            {
                throw new Exception($"Errors occurred when fetching assigned task related information (taskId = {taskAssignmentId})");
            }

            return await WriteCommunicationLogAsync(info, participantId, sendType, templateType, templateSubject, templateContent, cancellationToken);
        }

        private async Task<Guid> WriteCommunicationLogAsync(CommunicationRelatedInfo info, Guid participantId, CommunicationLogSendType sendType, int templateType, string templateSubject, string templateContent, CancellationToken cancellationToken)
        {
            bool htmlFormatLinks = true;
            if (templateType == TemplateType.SMS)
                htmlFormatLinks = false;

            var subject = communicationHelper.ReplaceTokens(templateSubject, info, htmlFormatLinks);
            var content = communicationHelper.ReplaceTokens(templateContent, info, htmlFormatLinks);

            if (templateType == TemplateType.DigitalPost)
            {
                return await communicationLogRepository.SetCommunicationLogAsync(
                    participantId,
                    CommunicationLogMessageType.DigitalPost,
                    sendType,
                    content,
                    subject,
                    cancellationToken);
            }
            else if (templateType == TemplateType.Email)
            {
                return await communicationLogRepository.SetCommunicationLogAsync(
                    participantId,
                    CommunicationLogMessageType.Email,
                    sendType,
                    content,
                    subject,
                    cancellationToken);
            }
            else if (templateType == TemplateType.SMS)
            {
                var shortContent = content.Length > 100 ? content.Substring(0, 100) : content;

                return await communicationLogRepository.SetCommunicationLogAsync(
                    participantId,
                    CommunicationLogMessageType.Sms,
                    sendType,
                    content,
                    shortContent,
                    cancellationToken);
            }
            else
            {
                throw new Exception("Invalid communication type");
            }
        }

        public async Task SendTaskInvitationReminderAsync(Guid participantId, Guid taskAssignmentId, DateTime? invitationDate, DateTime? invitationReminderDate, DateTime taskDate, CancellationToken cancellationToken)
        {
            if (!invitationDate.HasValue || DateTime.UtcNow.Date >= taskDate.Date) return;

            TimeSpan difference;

            if (!invitationReminderDate.HasValue)
                difference = DateTime.UtcNow.Date - invitationDate.Value.Date;
            else
                difference = DateTime.UtcNow.Date - invitationReminderDate.Value.Date;
            // send every 4 days
            if (difference.Days >= 4)
            {
                var template = await communicationQueryRepository.GetTaskInvitationReminderCommunicationTemplateAsync(taskAssignmentId, cancellationToken) ?? throw new Exception($"Errors occurred when fetching task invitation reminder communication template (taskAssignmentId = {taskAssignmentId})");

                var logId = await WriteCommunicationLogAsync(participantId, taskAssignmentId, template, cancellationToken);

                await queueService.PublishLoggedJobAsync(logId, new TaskSendInvitationReminderJobMessage()
                {
                    ParticipantId = participantId,
                    TaskAssignmentId = taskAssignmentId
                }, cancellationToken);
            }
        }

        public async Task SendTaskReminderAsync(Guid participantId, Guid taskAssignmentId, DateTime? reminderDate, DateTime taskDate, CancellationToken cancellationToken)
        {
            if (reminderDate.HasValue || DateTime.UtcNow.Date >= taskDate.Date) return;

            TimeSpan difference = taskDate.Date - DateTime.UtcNow.Date;
            if (!int.TryParse(appConfiguration.TaskReminderDay, out int day))
                day = 5;
            // send before 5 days
            if (difference.Days <= day)
            {
                var template = await communicationQueryRepository.GetTaskReminderCommunicationTemplateAsync(taskAssignmentId, cancellationToken) ?? throw new Exception($"Errors occurred when fetching task reminder communication template (taskAssignmentId = {taskAssignmentId})");

                var logId = await WriteCommunicationLogAsync(participantId, taskAssignmentId, template, cancellationToken);

                await queueService.PublishLoggedJobAsync(logId, new TaskSendReminderJobMessage()
                {
                    ParticipantId = participantId,
                    TaskAssignmentId = taskAssignmentId
                }, cancellationToken);
            }
        }
    }
}
