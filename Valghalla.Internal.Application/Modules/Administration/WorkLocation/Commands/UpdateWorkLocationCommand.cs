using FluentValidation;

using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.AuditLog;
using Valghalla.Application.Queue;
using Valghalla.Application.Queue.Messages;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Commands;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.WorkLocation.Commands
{
    public sealed record UpdateWorkLocationCommand() : ICommand<Response>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid AreaId { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public int VoteLocation { get; set; }
        public List<Guid> TaskTypeIds { get; init; } = new List<Guid>();
        public List<Guid> TaskTypeTemplateIds { get; init; } = new List<Guid>();
        public List<Guid> TeamIds { get; init; } = new List<Guid>();
        public List<Guid> ResponsibleIds { get; init; } = new List<Guid>();
    }

    public sealed class UpdateWorkLocationCommandValidator : AbstractValidator<UpdateWorkLocationCommand>
    {
        public UpdateWorkLocationCommandValidator(IWorkLocationQueryRepository buildingQueryRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Title)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();

            RuleFor(x => x.AreaId)
                .NotEmpty();

            RuleFor(x => x.Address)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();

            RuleFor(x => x.PostalCode)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();

            RuleFor(x => x.City)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();
            
        }
    }

    internal class UpdateWorkLocationCommandHandler : ICommandHandler<UpdateWorkLocationCommand, Response>
    {
        private readonly IWorkLocationCommandRepository workLocationCommandRepository;
        private readonly IParticipantSharedQueryRepository participantSharedQueryRepository;
        private readonly ITaskTypeTemplateQueryRepository taskTypeTemplateQueryRepository;
        private readonly ITaskTypeCommandRepository taskTypeCommandRepository;
        private readonly ITaskTypeQueryRepository taskTypeQueryRepository;
        private readonly IAuditLogService auditLogService;
        private readonly IQueueService queueService;

        public UpdateWorkLocationCommandHandler(
            IWorkLocationCommandRepository workLocationCommandRepository,
            IParticipantSharedQueryRepository participantSharedQueryRepository,
            IAuditLogService auditLogService,
            IQueueService queueService,
            ITaskTypeTemplateQueryRepository taskTypeTemplateQueryRepository,
            ITaskTypeCommandRepository taskTypeCommandRepository,
            ITaskTypeQueryRepository taskTypeQueryRepository)
        {
            this.workLocationCommandRepository = workLocationCommandRepository;
            this.participantSharedQueryRepository = participantSharedQueryRepository;
            this.auditLogService = auditLogService;
            this.queueService = queueService;
            this.taskTypeTemplateQueryRepository = taskTypeTemplateQueryRepository;
            this.taskTypeCommandRepository = taskTypeCommandRepository;
            this.taskTypeQueryRepository = taskTypeQueryRepository;
        }

        public async Task<Response> Handle(UpdateWorkLocationCommand command, CancellationToken cancellationToken)
        {
            foreach (var templateId in command.TaskTypeTemplateIds)
            {

                var (taskTypeExists, existingTaskTypeId) = await taskTypeQueryRepository.CheckIfTaskTypeExistsAsync(templateId, command.Id, cancellationToken);

                if (taskTypeExists)
                {
                    command.TaskTypeIds.Add(existingTaskTypeId!.Value);
                }
                else
                {
                    var template = await taskTypeTemplateQueryRepository.GetTaskTypeTemplateAsync(templateId, cancellationToken);

                    var taskTypeId = await taskTypeCommandRepository.CreateTaskTypeAsync(
                        new CreateTaskTypeCommand()
                        {
                            Title = template.Title,
                            ShortName = template.ShortName,
                            Description = template.Description,
                            StartTime = template.StartTime,
                            EndTime = template.EndTime,
                            Payment = template.Payment,
                            ValidationNotRequired = template.ValidationNotRequired,
                            Trusted = template.Trusted,
                            SendingReminderEnabled = template.SendingReminderEnabled,
                            FileReferenceIds = template.FileReferences.Select(i => i.Id),
                            ElectionId = Guid.Empty,
                            WorkLocationId = command.Id,
                            TaskTypeTemplateId = template.Id
                        }, cancellationToken);

                    command.TaskTypeIds.Add(taskTypeId);
                }

            }
            var (addedResponsibleIds, removedResponsibleIds) = await workLocationCommandRepository.UpdateWorkLocationAsync(command, cancellationToken);

            var addedResponsibles = await participantSharedQueryRepository.GetPariticipantsAsync(new Shared.Participant.Queries.GetParticipantsSharedQuery()
            {
                Values = addedResponsibleIds,
            }, cancellationToken);

            var removedResponsibles = await participantSharedQueryRepository.GetPariticipantsAsync(new Shared.Participant.Queries.GetParticipantsSharedQuery()
            {
                Values = removedResponsibleIds,
            }, cancellationToken);

            var addedResponsibleAuditLogs = addedResponsibles
                .Select(i => new ParticipantWorkLocationResponsibleAuditLog(true, command.Title, i.Id, i.FirstName, i.LastName, i.Birthdate));

            var removedResponsibleAuditLogs = removedResponsibles
                .Select(i => new ParticipantWorkLocationResponsibleAuditLog(false, command.Title, i.Id, i.FirstName, i.LastName, i.Birthdate));

            await auditLogService.AddAuditLogsAsync(addedResponsibleAuditLogs.Concat(removedResponsibleAuditLogs), cancellationToken);

            var cprNumbers = addedResponsibles.Concat(removedResponsibles).Select(i => i.Cpr);
            await queueService.PublishAsync(new ExternalUserClearCacheMessage()
            {
                CprNumbers = cprNumbers,
            }, cancellationToken);

            return Response.Ok();
        }
    }
}
