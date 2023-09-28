using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Confirmations;
using Valghalla.Application.Queue;
using Valghalla.Application.Queue.Messages;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Participant.Commands
{
    public sealed record BulkDeleteParticipantsCommand(IEnumerable<Guid> ParticipantIds) : ConfirmationCommand<Response>;

    public sealed class BulkDeleteParticipantsCommandValidator : AbstractValidator<BulkDeleteParticipantsCommand>
    {
        public BulkDeleteParticipantsCommandValidator()
        {
            RuleFor(x => x.ParticipantIds)
                .NotEmpty();
        }
    }

    public sealed class BulkDeleteParticipantsCommandConfirmator : Confirmator<BulkDeleteParticipantsCommand>
    {
        public override string Title => "participant.bulk_delete_participants_confirmation.title";
        public override string Message => "participant.bulk_delete_participants_confirmation.content";

        public BulkDeleteParticipantsCommandConfirmator(IParticipantGdprQueryRepository repository)
        {
            MultipleMessageEnabled = true;

            WhenAsync(repository.CheckIfParticipantsContainAssignedTasksInActiveElectionAsync)
                .WithMessage("participant.bulk_delete_participants_confirmation.contain_assigned_tasks_in_active_election");

            WhenAsync(repository.CheckIfParticipantsAreTeamResponsiblesAsync)
                .WithMessage("participant.bulk_delete_participants_confirmation.contain_team_responsible");

            WhenAsync(repository.CheckIfParticipantsAreWorkLocationResponsiblesAsync)
                .WithMessage("participant.bulk_delete_participants_confirmation.contain_work_location_responsible");
        }
    }

    internal class BulkDeleteParticipantsCommandHandler : ICommandHandler<BulkDeleteParticipantsCommand, Response>
    {
        private readonly IParticipantGdprCommandRepository participantGdprCommandRepository;
        private readonly IQueueService queueService;

        public BulkDeleteParticipantsCommandHandler(IParticipantGdprCommandRepository participantGdprCommandRepository, IQueueService queueService)
        {
            this.participantGdprCommandRepository = participantGdprCommandRepository;
            this.queueService = queueService;
        }

        public async Task<Response> Handle(BulkDeleteParticipantsCommand command, CancellationToken cancellationToken)
        {
            var cprNumbers = await participantGdprCommandRepository.DeleteParticipantsAsync(command, cancellationToken);

            await queueService.PublishAsync(new ExternalUserClearCacheMessage()
            {
                CprNumbers = cprNumbers
            }, cancellationToken);

            return Response.Ok();
        }
    }
}
