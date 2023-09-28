using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.AuditLog;
using Valghalla.Application.Queue;
using Valghalla.Application.Queue.Messages;
using Valghalla.Internal.Application.Modules.Administration.Team.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Team.Commands
{
    public sealed record UpdateTeamCommand() : ICommand<Response>
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string ShortName { get; init; } = string.Empty;
        public string? Description { get; init; }
        public List<Guid> ResponsibleIds { get; init; }
    }

    public sealed class UpdateTeamCommandValidator : AbstractValidator<UpdateTeamCommand>
    {
        public UpdateTeamCommandValidator(ITeamQueryRepository teamQueryRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();

            RuleFor(x => x.ShortName)
               .NotEmpty();

            RuleFor(x => x)
                .Must((command) => !teamQueryRepository.CheckIfTeamExistsAsync(command, default).Result)
                .WithMessage("administration.teams.error.team_exist");
        }
    }

    internal class UpdateTeamCommandHandler : ICommandHandler<UpdateTeamCommand, Response>
    {
        private readonly ITeamCommandRepository teamCommandRepository;
        private readonly IParticipantSharedQueryRepository participantSharedQueryRepository;
        private readonly IAuditLogService auditLogService;
        private readonly IQueueService queueService;

        public UpdateTeamCommandHandler(
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

        public async Task<Response> Handle(UpdateTeamCommand command, CancellationToken cancellationToken)
        {
            var (addedResponsibleIds, removedResponsibleIds) = await teamCommandRepository.UpdateTeamAsync(command, cancellationToken);

            var addedResponsibles = await participantSharedQueryRepository.GetPariticipantsAsync(new Shared.Participant.Queries.GetParticipantsSharedQuery()
            {
                Values = addedResponsibleIds,
            }, cancellationToken);

            var removedResponsibles = await participantSharedQueryRepository.GetPariticipantsAsync(new Shared.Participant.Queries.GetParticipantsSharedQuery()
            {
                Values = removedResponsibleIds,
            }, cancellationToken);

            var addedResponsibleAuditLogs = addedResponsibles
                .Select(i => new ParticipantTeamResponsibleAuditLog(true, command.Name, i.Id, i.FirstName, i.LastName, i.Birthdate));

            var removedResponsibleAuditLogs = removedResponsibles
                .Select(i => new ParticipantTeamResponsibleAuditLog(false, command.Name, i.Id, i.FirstName, i.LastName, i.Birthdate));

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
