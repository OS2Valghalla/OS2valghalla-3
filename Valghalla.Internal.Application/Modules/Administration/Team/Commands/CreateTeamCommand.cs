using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.AuditLog;
using Valghalla.Application.Queue;
using Valghalla.Application.Queue.Messages;
using Valghalla.Internal.Application.Modules.Administration.Team.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Team.Commands
{
    public sealed record CreateTeamCommand() : ICommand<Response<Guid>>
    {
        public string Name { get; init; } = null!;
        public string ShortName { get; init; } = null!;
        public string? Description { get; init; }
        public List<Guid> ResponsibleIds { get; init; }
    }
    public sealed class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
    {
        public CreateTeamCommandValidator(ITeamQueryRepository teamQueryRepository)
        {
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

    internal class CreateTeamCommandHandler : ICommandHandler<CreateTeamCommand, Response<Guid>>
    {
        private readonly ITeamCommandRepository teamCommandRepository;
        private readonly IParticipantSharedQueryRepository participantSharedQueryRepository;
        private readonly IAuditLogService auditLogService;
        private readonly IQueueService queueService;

        public CreateTeamCommandHandler(
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

        public async Task<Response<Guid>> Handle(CreateTeamCommand command, CancellationToken cancellationToken)
        {
            var teamId = await teamCommandRepository.CreateTeamAsync(command, cancellationToken);
            var participants = await participantSharedQueryRepository.GetPariticipantsAsync(new Shared.Participant.Queries.GetParticipantsSharedQuery()
            {
                Values = command.ResponsibleIds,
            }, cancellationToken);

            var auditLogs = participants.Select(i => new ParticipantTeamResponsibleAuditLog(true, command.Name, i.Id, i.FirstName, i.LastName, i.Birthdate));
            await auditLogService.AddAuditLogsAsync(auditLogs, cancellationToken);

            var cprNumbers = participants.Select(i => i.Cpr);
            await queueService.PublishAsync(new ExternalUserClearCacheMessage()
            {
                CprNumbers = cprNumbers
            }, cancellationToken);

            return Response.Ok(teamId);
        }
    }
}
