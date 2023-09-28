using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Confirmations;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Election.Commands
{
    public sealed record DeactivateElectionCommand(Guid Id) : ConfirmationCommand<Response>;

    public sealed class DeactivateElectionCommandValidator : AbstractValidator<DeactivateElectionCommand>
    {
        public DeactivateElectionCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    public sealed class DeactivateElectionCommandConfirmator : Confirmator<DeactivateElectionCommand>
    {
        public override string Title => "administration.election.deactivate_election_confirmation.title";
        public override string Message => "administration.election.deactivate_election_confirmation.content";

        public DeactivateElectionCommandConfirmator()
        {
            MultipleMessageEnabled = true;

            WhenAsync((_, _) => Task.FromResult(true))
                .WithMessage("administration.election.deactivate_election_confirmation.sub_content");
        }
    }

    internal class DeactivateElectionCommandHandler : ICommandHandler<DeactivateElectionCommand, Response>
    {
        private readonly IElectionCommandRepository electionCommandRepository;

        public DeactivateElectionCommandHandler(IElectionCommandRepository electionCommandRepository)
        {
            this.electionCommandRepository = electionCommandRepository;
        }

        public async Task<Response> Handle(DeactivateElectionCommand command, CancellationToken cancellationToken)
        {
            await electionCommandRepository.DeactivateElectionAsync(command, cancellationToken);

            return Response.Ok();
        }
    }
}
