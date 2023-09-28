using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.AuditLog;
using Valghalla.Application.Confirmations;
using Valghalla.Application.Queue;
using Valghalla.Application.Queue.Messages;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.WorkLocation.Commands
{
    public sealed record DeleteWorkLocationCommand(Guid Id) : ConfirmationCommand<Response>;

    public sealed class DeleteWorkLocationCommandValidator : AbstractValidator<DeleteWorkLocationCommand>
    {
        public DeleteWorkLocationCommandValidator(IWorkLocationQueryRepository workLocationQueryRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x)
               .Must((command) => !workLocationQueryRepository.CheckIfWorkLocationUsedInActiveElectionAsync(command.Id, default).Result)
               .WithMessage("administration.work_location.error.work_location_used_in_active_election");
        }
    }

    public sealed class DeleteWorkLocationCommandConfirmator : Confirmator<DeleteWorkLocationCommand>
    {
        public override string Title => "administration.work_location.action.delete_confirmation.title";
        public override string Message => "administration.work_location.action.delete_confirmation.content_no_task";

        public DeleteWorkLocationCommandConfirmator(IWorkLocationQueryRepository workLocationQueryRepository)
        {
            WhenAsync(workLocationQueryRepository.CheckIfWorkLocationHasTasksAsync)
                .WithMessage("administration.work_location.action.delete_confirmation.content_has_task");
        }
    }

    internal class DeleteWorkLocationCommandHandler : ICommandHandler<DeleteWorkLocationCommand, Response>
    {
        private readonly IWorkLocationCommandRepository workLocationCommandRepository;
        private readonly IParticipantSharedQueryRepository participantSharedQueryRepository;
        private readonly IAuditLogService auditLogService;
        private readonly IQueueService queueService;

        public DeleteWorkLocationCommandHandler(
            IWorkLocationCommandRepository workLocationCommandRepository,
            IParticipantSharedQueryRepository participantSharedQueryRepository,
            IAuditLogService auditLogService,
            IQueueService queueService)
        {
            this.workLocationCommandRepository = workLocationCommandRepository;
            this.participantSharedQueryRepository = participantSharedQueryRepository;
            this.auditLogService = auditLogService;
            this.queueService = queueService;
        }

        public async Task<Response> Handle(DeleteWorkLocationCommand command, CancellationToken cancellationToken)
        {
            var (workLocationResponsibleIds, workLocationTitle) = await workLocationCommandRepository.DeleteWorkLocationAsync(command, cancellationToken);
            var participants = await participantSharedQueryRepository.GetPariticipantsAsync(new Shared.Participant.Queries.GetParticipantsSharedQuery()
            {
                Values = workLocationResponsibleIds,
            }, cancellationToken);

            var auditLogs = participants.Select(i => new ParticipantWorkLocationResponsibleAuditLog(false, workLocationTitle, i.Id, i.FirstName, i.LastName, i.Birthdate));
            await auditLogService.AddAuditLogsAsync(auditLogs, cancellationToken);

            var cprNumbers = participants.Select(i => i.Cpr);
            await queueService.PublishAsync(new ExternalUserClearCacheMessage()
            {
                CprNumbers = cprNumbers
            }, cancellationToken);

            return Response.Ok();
        }
    }        
}
