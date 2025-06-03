using FluentValidation;

using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.AuditLog;
using Valghalla.Application.Confirmations;
using Valghalla.Application.Queue;
using Valghalla.Application.Queue.Messages;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Commands
{
    public sealed record DeleteWorkLocationTemplateCommand(Guid Id) : ConfirmationCommand<Response>;

    public sealed class DeleteWorkLocationTemplateCommandValidator : AbstractValidator<DeleteWorkLocationTemplateCommand>
    {
        public DeleteWorkLocationTemplateCommandValidator(IWorkLocationTemplateQueryRepository workLocationTemplateQueryRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            //RuleFor(x => x)
            //   .Must((command) => !workLocationTemplateQueryRepository.CheckIfWorkLocationTemplateUsedInActiveElectionAsync(command.Id, default).Result)
            //   .WithMessage("administration.work_location.error.work_location_used_in_active_election");
        }
    }

    public sealed class DeleteWorkLocationTemplateCommandConfirmator : Confirmator<DeleteWorkLocationTemplateCommand>
    {
        public override string Title => "administration.work_location.action.delete_confirmation.title";
        public override string Message => "administration.work_location.action.delete_confirmation.content_no_task";

        public DeleteWorkLocationTemplateCommandConfirmator(IWorkLocationTemplateQueryRepository workLocationQueryRepository)
        {
            //WhenAsync(workLocationQueryRepository.CheckIfWorkLocationTemplateHasTasksAsync)
            //    .WithMessage("administration.work_location.action.delete_confirmation.content_has_task");
        }
    }

    internal class DeleteWorkLocationTemplateCommandHandler : ICommandHandler<DeleteWorkLocationTemplateCommand, Response>
    {
        private readonly IWorkLocationTemplateCommandRepository workLocationTemplateCommandRepository;
        private readonly IParticipantSharedQueryRepository participantSharedQueryRepository;
        private readonly IAuditLogService auditLogService;
        private readonly IQueueService queueService;

        public DeleteWorkLocationTemplateCommandHandler(
            IWorkLocationTemplateCommandRepository workLocationCommandRepository,
            IParticipantSharedQueryRepository participantSharedQueryRepository,
            IAuditLogService auditLogService,
            IQueueService queueService)
        {
            workLocationTemplateCommandRepository = workLocationCommandRepository;
            this.participantSharedQueryRepository = participantSharedQueryRepository;
            this.auditLogService = auditLogService;
            this.queueService = queueService;
        }

        public async Task<Response> Handle(DeleteWorkLocationTemplateCommand command, CancellationToken cancellationToken)
        {
            var workLocationTemplateResponsibleIds = await workLocationTemplateCommandRepository.DeleteWorkLocationTemplateAsync(command, cancellationToken);
            //var participants = await participantSharedQueryRepository.GetPariticipantsAsync(new Shared.Participant.Queries.GetParticipantsSharedQuery()
            //{
            //    Values = workLocationTemplateResponsibleIds,
            //}, cancellationToken);

            //var auditLogs = participants.Select(i => new ParticipantWorkLocationResponsibleAuditLog(false, workLocationTemplateTitle, i.Id, i.FirstName, i.LastName, i.Birthdate));
            //await auditLogService.AddAuditLogsAsync(auditLogs, cancellationToken);

            //var cprNumbers = participants.Select(i => i.Cpr);
            //await queueService.PublishAsync(new ExternalUserClearCacheMessage()
            //{
            //    CprNumbers = cprNumbers
            //}, cancellationToken);

            return Response.Ok();
        }
    }        
}
