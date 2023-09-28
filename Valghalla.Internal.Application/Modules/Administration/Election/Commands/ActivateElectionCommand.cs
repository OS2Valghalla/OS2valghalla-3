using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Confirmations;
using Valghalla.Application.Queue;
using Valghalla.Application.Queue.Messages;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Election.Commands
{
    public sealed record ActivateElectionCommand(Guid Id) : ConfirmationCommand<Response>;

    public sealed class ActivateElectionCommandValidator : AbstractValidator<ActivateElectionCommand>
    {
        public ActivateElectionCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    public sealed class ActivateElectionCommandConfirmator : Confirmator<ActivateElectionCommand>
    {
        public override string Title => "administration.election.activate_election_confirmation.title";
        public override string Message => "administration.election.activate_election_confirmation.content";

        public ActivateElectionCommandConfirmator()
        {
            MultipleMessageEnabled = true;

            WhenAsync((_, _) => Task.FromResult(true))
                .WithMessage("administration.election.activate_election_confirmation.sub_content");
        }
    }

    internal class ActivateElectionCommandHandler : ICommandHandler<ActivateElectionCommand, Response>
    {
        private readonly IElectionCommandRepository electionCommandRepository;
        private readonly IQueueService queueService;

        public ActivateElectionCommandHandler(
            IElectionCommandRepository electionCommandRepository,
            IQueueService queueService)
        {
            this.electionCommandRepository = electionCommandRepository;
            this.queueService = queueService;
        }

        public async Task<Response> Handle(ActivateElectionCommand command, CancellationToken cancellationToken)
        {
            await electionCommandRepository.ActivateElectionAsync(command, cancellationToken);

            await queueService.PublishJobAsync(new ElectionActivationJobMessage()
            {
                ElectionId = command.Id,
            }, cancellationToken);

            return Response.Ok();
        }
    }
}
