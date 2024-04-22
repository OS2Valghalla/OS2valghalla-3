using Valghalla.Application.Communication;
using Valghalla.Application.Configuration;
using Valghalla.Application.DigitalPost;
using Valghalla.Application.Mail;
using Valghalla.Application.SMS;
using Valghalla.Application.Storage;
using Valghalla.Worker.Exceptions;
using Valghalla.Worker.Infrastructure.Modules.Communication.Repositories;

namespace Valghalla.Worker.Services
{
    internal interface ITaskCommunicationService
    {
        Task SendTaskInvitationAsync(Guid logId, Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task SendRemovedFromTaskByValidationAsync(Guid logId, Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task SendRemovedFromTaskAsync(Guid logId, Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task SendTaskReminderAsync(Guid logId, Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task SendTaskInvitationReminderAsync(Guid logId, Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task SendTaskRegistrationAsync(Guid logId, Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task SendTaskCancellationAsync(Guid logId, Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task SendTaskInvitationRetractedAsync(Guid logId, Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task SendGroupMessageAsync(Guid logId, Guid participantId, Guid taskAssignmentId, bool isRejectedTask, int templateType, string templateSubject, string templateContent, List<Guid> fileReferenceIds, CancellationToken cancellationToken);
    }

    internal class TaskCommunicationService : ITaskCommunicationService
    {
        private readonly AppConfiguration appConfiguration;
        private readonly IFileStorageService fileStorageService;

        private readonly ITextMessageService textMessageService;
        private readonly IMailMessageService mailMessageService;
        private readonly IDigitalPostService digitalPostService;

        private readonly ICommunicationHelper communicationHelper;
        private readonly ICommunicationQueryRepository communicationQueryRepository;
        private readonly ICommunicationCommandRepository communicationCommandRepository;
        private readonly ICommunicationLogRepository communicationLogRepository;

        record CommunicationContent
        {
            public string Subject { get; init; } = null!;
            public string Content { get; init; } = null!;
        }

        public TaskCommunicationService(
            AppConfiguration appConfiguration,
            IFileStorageService fileStorageService,
            ITextMessageService textMessageService,
            IMailMessageService mailMessageService,
            IDigitalPostService digitalPostService,
            ICommunicationHelper communicationHelper,
            ICommunicationQueryRepository communicationQueryRepository,
            ICommunicationCommandRepository communicationCommandRepository,
            ICommunicationLogRepository communicationLogRepository)
        {
            this.appConfiguration = appConfiguration;
            this.fileStorageService = fileStorageService;
            this.textMessageService = textMessageService;
            this.mailMessageService = mailMessageService;
            this.digitalPostService = digitalPostService;
            this.communicationHelper = communicationHelper;
            this.communicationQueryRepository = communicationQueryRepository;
            this.communicationCommandRepository = communicationCommandRepository;
            this.communicationLogRepository = communicationLogRepository;
        }

        public async Task SendTaskInvitationAsync(Guid logId, Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var valid = await communicationHelper.ValidateTaskInvitationAsync(participantId, taskAssignmentId, cancellationToken);

            if (!valid) return;

            var template = await communicationQueryRepository.GetTaskInvitationCommunicationTemplateAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Errors occurred when fetching task invitation communication template (taskAssignmentId = {taskAssignmentId})");

            await SendAsync(logId, participantId, taskAssignmentId, template, cancellationToken);
            await communicationCommandRepository.SetTaskAssignmentInvitationSentAsync(taskAssignmentId, cancellationToken);
        }

        public async Task SendRemovedFromTaskAsync(Guid logId, Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var valid = await communicationHelper.ValidateRemovedFromTaskAsync(participantId, taskAssignmentId, cancellationToken);

            if (!valid) return;

            var template = await communicationQueryRepository.GetRemovedFromTaskCommunicationTemplateAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Errors occurred when fetching removed from task communication template (taskAssignmentId = {taskAssignmentId})");

            await SendAsync(logId, participantId, taskAssignmentId, template, cancellationToken);
        }

        public async Task SendRemovedFromTaskByValidationAsync(Guid logId, Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var valid = await communicationHelper.ValidateRemovedFromTaskByValidationAsync(participantId, taskAssignmentId, cancellationToken);

            if (!valid) return;

            var template = await communicationQueryRepository.GetRemovedFromTaskByValidationCommunicationTemplateAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Errors occurred when fetching removed from task by validation communication template (taskAssignmentId = {taskAssignmentId})");

            await SendAsync(logId, participantId, taskAssignmentId, template, cancellationToken);
        }

        public async Task SendTaskInvitationReminderAsync(Guid logId, Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var valid = await communicationHelper.ValidateTaskInvitationReminderAsync(participantId, taskAssignmentId, cancellationToken);

            if (!valid) return;

            var template = await communicationQueryRepository.GetTaskInvitationReminderCommunicationTemplateAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Errors occurred when fetching task invitation reminder communication template (taskAssignmentId = {taskAssignmentId})");

            await SendAsync(logId, participantId, taskAssignmentId, template, cancellationToken);
            await communicationCommandRepository.SetTaskAssignmentInvitationReminderSentAsync(taskAssignmentId, cancellationToken);
        }

        public async Task SendTaskReminderAsync(Guid logId, Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var valid = await communicationHelper.ValidateTaskReminderAsync(participantId, taskAssignmentId, cancellationToken);

            if (!valid) return;

            var template = await communicationQueryRepository.GetTaskReminderCommunicationTemplateAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Errors occurred when fetching task reminder communication template (taskAssignmentId = {taskAssignmentId})");

            await SendAsync(logId, participantId, taskAssignmentId, template, cancellationToken);
            await communicationCommandRepository.SetTaskAssignmentReminderSentAsync(taskAssignmentId, cancellationToken);
        }

        public async Task SendTaskRegistrationAsync(Guid logId, Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var valid = await communicationHelper.ValidateTaskRegistrationAsync(participantId, taskAssignmentId, cancellationToken);

            if (!valid) return;

            var template = await communicationQueryRepository.GetTaskRegistrationCommunicationTemplateAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Errors occurred when fetching task registration communication template (taskAssignmentId = {taskAssignmentId})");

            await SendAsync(logId, participantId, taskAssignmentId, template, cancellationToken);
            await communicationCommandRepository.SetTaskAssignmentRegistrationSentAsync(taskAssignmentId, cancellationToken);
        }
        public async Task SendTaskCancellationAsync(Guid logId, Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var template = await communicationQueryRepository.GetTaskCancellationCommunicationTemplateAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Errors occurred when fetching task cancellation communication template (taskAssignmentId = {taskAssignmentId})");

            await SendAsync(logId, participantId, taskAssignmentId, template, cancellationToken);
            await communicationCommandRepository.SetTaskAssignmentCancellationSentAsync(taskAssignmentId, cancellationToken);
        }

        public async Task SendTaskInvitationRetractedAsync(Guid logId, Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var valid = await communicationHelper.ValidateTaskInvitationRetractedAsync(taskAssignmentId, cancellationToken);

            if (!valid) return;

            var template = await communicationQueryRepository.GetTaskRetractedInvitationCommunicationTemplateAsync(taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Errors occurred when fetching task retracted invitation communication template (taskAssignmentId = {taskAssignmentId})");

            await SendAsync(logId, participantId, taskAssignmentId, template, cancellationToken);
        }

        public async Task SendGroupMessageAsync(Guid logId, Guid participantId, Guid taskAssignmentId, bool isRejectedTask, int templateType, string templateSubject, string templateContent, List<Guid> fileReferenceIds, CancellationToken cancellationToken)
        {
            var info = await (isRejectedTask ? communicationQueryRepository.GetRejectedTaskInfoAsync(taskAssignmentId, cancellationToken) : communicationQueryRepository.GetCommunicationRelatedInfoAsync(participantId, taskAssignmentId, cancellationToken))
                ?? (isRejectedTask ? throw new Exception($"Errors occurred when fetching rejected task related information (taskId = {taskAssignmentId})") : throw new Exception($"Errors occurred when fetching assigned task related information (taskId = {taskAssignmentId})"));

            bool htmlFormatLinks = true;
            if (templateType == TemplateType.SMS)
                htmlFormatLinks = false;

            var subject = communicationHelper.ReplaceTokens(templateSubject, info, htmlFormatLinks);
            var content = communicationHelper.ReplaceTokens(templateContent, info, htmlFormatLinks);

            if (templateType == TemplateType.DigitalPost)
            {
                await SendByDigitalPostAsync(logId, subject, content, fileReferenceIds, info, cancellationToken);
            }
            else if (templateType == TemplateType.Email)
            {
                await SendByMailAsync(logId, subject, content, fileReferenceIds, info, cancellationToken);
            }
            else if (templateType == TemplateType.SMS)
            {
                await SendBySmsAsync(logId, content, info, cancellationToken);
            }
            else
            {
                throw new Exception("Invalid communication type");
            }
        }

        private async Task SendAsync(Guid logId, Guid participantId, Guid taskAssignmentId, CommunicationTemplate template, CancellationToken cancellationToken)
        {
            var info = await communicationQueryRepository.GetCommunicationRelatedInfoAsync(participantId, taskAssignmentId, cancellationToken)
                ?? throw new Exception($"Errors occurred when fetching task related information (taskAssignmentId = {taskAssignmentId})");

            bool htmlFormatLinks = true;
            if (templateType == TemplateType.SMS)
                htmlFormatLinks = false;

            var subject = communicationHelper.ReplaceTokens(templateSubject, info, htmlFormatLinks);
            var content = communicationHelper.ReplaceTokens(templateContent, info, htmlFormatLinks);

            if (template.TemplateType == TemplateType.DigitalPost)
            {
                await SendByDigitalPostAsync(logId, subject, content, template.FileRefIds, info, cancellationToken);
            }
            else if (template.TemplateType == TemplateType.Email)
            {
                await SendByMailAsync(logId, subject, content, template.FileRefIds, info, cancellationToken);
            }
            else if (template.TemplateType == TemplateType.SMS)
            {
                await SendBySmsAsync(logId, content, info, cancellationToken);
            }
            else
            {
                throw new Exception("Invalid communication type");
            }
        }

        private async Task SendByMailAsync(Guid logId, string subject, string content, IEnumerable<Guid> fileRefIds, CommunicationRelatedInfo info, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(info.Participant.Email))
            {
                throw new Exception($"Participant does not have email yet (Name: {info.Participant.Name}, ID: {info.Participant.Id})");
            }

            try
            {
                var attachments = new List<(Stream, string)>();

                foreach (var fileRefId in fileRefIds)
                {
                    var (stream, fileName) = await fileStorageService.DownloadAsync(fileRefId, cancellationToken);
                    attachments.Add((stream, fileName));
                }

                var message = new MailDataWithAttachments()
                {
                    To = new[] { info.Participant.Email },
                    Subject = subject,
                    Body = content,
                    Attachments = attachments
                };

                await mailMessageService.SendWithAttachmentsAsync(message, cancellationToken);
                await communicationLogRepository.UpdateCommunicationLogSuccessAsync(logId, content, subject, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ExternalException(content, subject, ex);
            }
        }

        private async Task SendBySmsAsync(Guid logId, string content, CommunicationRelatedInfo info, CancellationToken cancellationToken)
        {
            var shortContent = content.Length > 100 ? content.Substring(0, 100) : content;

            if (string.IsNullOrEmpty(info.Participant.MobileNumber))
            {
                throw new Exception($"Participant does not have mobile number yet (Name: {info.Participant.Name}, ID: {info.Participant.Id})");
            }

            try
            {
                var message = new TextMessage(appConfiguration.SmsSender, content, new[] { info.Participant.MobileNumber });

                await textMessageService.SendTextMessageAsync(message, cancellationToken);
                await communicationLogRepository.UpdateCommunicationLogSuccessAsync(logId, content, shortContent, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ExternalException(content, shortContent, ex);
            }
        }

        private async Task SendByDigitalPostAsync(Guid logId, string subject, string content, IEnumerable<Guid> fileRefIds, CommunicationRelatedInfo info, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(info.Participant.Cpr))
            {
                throw new Exception($"Participant does not have CPR number (Name: {info.Participant.Name}, ID: {info.Participant.Id})");
            }

            if (info.Participant.ExemptDigitalPost)
            {
                throw new Exception($"Participant does not have Digital Post (Name: {info.Participant.Name}, ID: {info.Participant.Id})");
            }

            try
            {
                var attachments = new List<(Stream, string)>();

                foreach (var fileRefId in fileRefIds)
                {
                    var (stream, fileName) = await fileStorageService.DownloadAsync(fileRefId, cancellationToken);
                    attachments.Add((stream, fileName));
                }

                var message = new DigitalPostMessage()
                {
                    Cpr = info.Participant.Cpr,
                    Label = subject,
                    Content = content,
                    Attachments = attachments
                };

                await digitalPostService.SendAsync(logId, message, cancellationToken);
                await communicationLogRepository.UpdateCommunicationLogSuccessAsync(logId, content, subject, cancellationToken);
            }
            catch (DigitalPostException ex)
            {
                throw new ExternalException(content, subject, ex.Details, ex);
            }
            catch (Exception ex)
            {
                throw new ExternalException(content, subject, ex);
            }
        }
    }
}
