using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.AuditLog;
using Valghalla.Application.Confirmations;
using Valghalla.Application.Queue;
using Valghalla.Application.Queue.Messages;
using Valghalla.Internal.Application.Modules.Administration.Team.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Team.Commands
{
    public sealed record DeleteTeamCommand(Guid Id) : ConfirmationCommand<Response>;

    public sealed class DeleteTeamCommandValidator : AbstractValidator<DeleteTeamCommand>
    {
        public DeleteTeamCommandValidator(ITeamQueryRepository teamQueryRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x)
               .Must((command) => !teamQueryRepository.CheckIfTeamUsedInActiveElectionAsync(command.Id, default).Result)
               .WithMessage("administration.teams.error.team_used_in_active_election");

            RuleFor(x => x)
               .Must((command) => !teamQueryRepository.CheckIfTeamHasAbandonedParticipantsAsync(command.Id, default).Result)
               .WithMessage("administration.teams.error.team_has_abandoned_participants");
        }
    }

    public sealed class DeleteTeamCommandConfirmator : Confirmator<DeleteTeamCommand>
    {
        public override string Title => "administration.teams.action.delete_confirmation.title";
        public override string Message => "administration.teams.action.delete_confirmation.content_no_task";

        public DeleteTeamCommandConfirmator(ITeamQueryRepository teamQueryRepository)
        {
            WhenAsync(teamQueryRepository.CheckIfTeamHasTasksAsync)
                .WithMessage("administration.teams.action.delete_confirmation.content_has_task");
        }
    }

    internal class DeleteTeamCommandHandler : ICommandHandler<DeleteTeamCommand, Response>
    {
        private readonly ITeamCommandRepository teamCommandRepository;
        private readonly IParticipantSharedQueryRepository participantSharedQueryRepository;
        private readonly IAuditLogService auditLogService;
        private readonly IQueueService queueService;

        public DeleteTeamCommandHandler(
            ITeamCommandRepository teamCommandRepository,
            IParticipantSharedQueryRepository participantSharedQueryRepository,
            IAuditLogService auditLogService,
            IQueueService queueService)
        {
            this.teamCommandRepository = teamCommandRepository;
            this.participantSharedQueryRepository = participantSharedQueryRepository;
            this.auditLogService = auditLogService;
            this.queueService = queueService;
        }

        public async Task<Response> Handle(DeleteTeamCommand command, CancellationToken cancellationToken)
        {
            var (teamResponsibleIds, teamName) = await teamCommandRepository.DeleteTeamAsync(command, cancellationToken);
            var participants = await participantSharedQueryRepository.GetPariticipantsAsync(new Shared.Participant.Queries.GetParticipantsSharedQuery()
            {
                Values = teamResponsibleIds,
            }, cancellationToken);

            var auditLogs = participants.Select(i => new ParticipantTeamResponsibleAuditLog(false, teamName, i.Id, i.FirstName, i.LastName, i.Birthdate));
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
