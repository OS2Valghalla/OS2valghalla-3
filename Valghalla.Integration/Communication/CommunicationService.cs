using Valghalla.Application.Communication;
using Valghalla.Application.Queue;
using Valghalla.Application.Queue.Messages;

namespace Valghalla.Integration.Communication
{
    internal class CommunicationService : ICommunicationService
    {
        private readonly IQueueService queueService;
        private readonly ICommunicationHelper communicationHelper;
        private readonly ICommunicationQueryRepository communicationQueryRepository;
        private readonly ICommunicationLogRepository communicationLogRepository;

        public CommunicationService(
            IQueueService queueService,
            ICommunicationHelper communicationHelper,
            ICommunicationQueryRepository communicationQueryRepository,
            ICommunicationLogRepository communicationLogRepository)
        {
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

            var logId = await WriteCommunicationLogAsync(participantId, taskAssignmentId, template, cancellationToken, isRejectedTask: true);

            await queueService.PublishLoggedJobAsync(logId, new TaskCancellationJobMessage()
            {
                ParticipantId = participantId,
                TaskAssignmentId = taskAssignmentId
            }, cancellationToken);
        }

        public async Task SendGroupMessageAsync(IList<CommunicationTaskParticipantInfo> tasks, int templateType, string templateSubject, string templateContent, List<Guid> fileReferenceIds, CancellationToken cancellationToken)
        {
            foreach (var task in tasks)
            {
                var info = await (task.IsRejectedTask ? communicationQueryRepository.GetRejectedTaskInfoAsync(task.TaskId, cancellationToken) : communicationQueryRepository.GetTaskAssignmentInfoAsync(task.TaskId, cancellationToken))
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

        private async Task<Guid> WriteCommunicationLogAsync(Guid participantId, Guid taskAssignmentId, CommunicationTemplate template, CancellationToken cancellationToken, bool isRejectedTask = false)
        {
            return await WriteCommunicationLogAsync(participantId, taskAssignmentId, CommunicationLogSendType.Automatic, template.TemplateType, template.Subject, template.Content, cancellationToken, isRejectedTask: isRejectedTask);
        }

        private async Task<Guid> WriteCommunicationLogAsync(Guid participantId, Guid taskAssignmentId, CommunicationLogSendType sendType, int templateType, string templateSubject, string templateContent, CancellationToken cancellationToken, bool isRejectedTask = false)
        {
            var info = await (isRejectedTask ? communicationQueryRepository.GetTaskAssignmentInfoAsync(taskAssignmentId, participantId, cancellationToken) : communicationQueryRepository.GetTaskAssignmentInfoAsync(taskAssignmentId, cancellationToken))
                ?? (isRejectedTask ? throw new Exception($"Errors occurred when fetching rejected task related information (taskId = {taskAssignmentId})") : throw new Exception($"Errors occurred when fetching assigned task related information (taskId = {taskAssignmentId})"));

            return await WriteCommunicationLogAsync(info, participantId, sendType, templateType, templateSubject, templateContent, cancellationToken);
        }

        private async Task<Guid> WriteCommunicationLogAsync(CommunicationRelatedInfo info, Guid participantId, CommunicationLogSendType sendType, int templateType, string templateSubject, string templateContent, CancellationToken cancellationToken)
        {
            var subject = communicationHelper.ReplaceTokens(templateSubject, info);
            var content = communicationHelper.ReplaceTokens(templateContent, info);

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
    }
}
