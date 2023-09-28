using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Confirmations;
using Valghalla.Application.Queue;
using Valghalla.Application.Queue.Messages;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Participant.Commands
{
    public sealed record DeleteParticipantCommand(Guid Id) : ConfirmationCommand<Response>;

    public sealed class DeleteParticipantCommandValidator : AbstractValidator<DeleteParticipantCommand>
    {
        public DeleteParticipantCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    public sealed class DeleteParticipantCommandConfirmator : Confirmator<DeleteParticipantCommand>
    {
        public override string Title => "participant.delete_participant_confirmation.title";
        public override string Message => "participant.delete_participant_confirmation.content";

        public DeleteParticipantCommandConfirmator(IParticipantGdprQueryRepository repository)  
        {
            MultipleMessageEnabled = true;

            WhenAsync(repository.CheckIfParticipantHasAssignedTasksInActiveElectionAsync)
                .WithMessage("participant.delete_participant_confirmation.has_assigned_tasks_in_active_election");

            WhenAsync(repository.CheckIfParticipantIsTeamResponsibleAsync)
                .WithMessage("participant.delete_participant_confirmation.is_team_responsible");

            WhenAsync(repository.CheckIfParticipantIsWorkLocationResponsibleAsync)
                .WithMessage("participant.delete_participant_confirmation.is_work_location_responsible");
        }
    }

    internal class DeleteParticipantCommandHandler : ICommandHandler<DeleteParticipantCommand, Response>
    {
        private readonly IParticipantGdprCommandRepository participantGdprCommandRepository;
        private readonly IQueueService queueService;

        public DeleteParticipantCommandHandler(IParticipantGdprCommandRepository participantGdprCommandRepository, IQueueService queueService)
        {
            this.participantGdprCommandRepository = participantGdprCommandRepository;
            this.queueService = queueService;
        }

        public async Task<Response> Handle(DeleteParticipantCommand command, CancellationToken cancellationToken)
        {
            var cprNumber = await participantGdprCommandRepository.DeleteParticipantAsync(command, cancellationToken);

            await queueService.PublishAsync(new ExternalUserClearCacheMessage()
            {
                CprNumbers = new[] { cprNumber }
            }, cancellationToken);

            return Response.Ok();
        }
    }
}
