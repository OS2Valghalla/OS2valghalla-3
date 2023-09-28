using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Confirmations;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Election.Commands
{
    public sealed record DeleteElectionCommand(Guid Id) : ConfirmationCommand<Response> { }

    public sealed class DeleteElectionCommandValidator : AbstractValidator<DeleteElectionCommand>
    {
        public DeleteElectionCommandValidator(IElectionQueryRepository electionQueryRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x)
               .Must((command) => !electionQueryRepository.CheckIfElectionIsActiveAsync(command.Id, default).Result)
               .WithMessage("administration.election.messages.cannot_delete_active_election");
        }
    }

    public sealed class DeleteElectionCommandConfirmator : Confirmator<DeleteElectionCommand>
    {
        public override string Title => "administration.election.delete_election_confirmation.title";
        public override string Message => "administration.election.delete_election_confirmation.content";
    }

    internal class DeleteElectionCommandHandler : ICommandHandler<DeleteElectionCommand, Response>
    {
        private readonly IElectionCommandRepository electionCommandRepository;

        public DeleteElectionCommandHandler(IElectionCommandRepository electionCommandRepository)
        {
            this.electionCommandRepository = electionCommandRepository;
        }

        public async Task<Response> Handle(DeleteElectionCommand command, CancellationToken cancellationToken)
        {
            await electionCommandRepository.DeleteElectionAsync(command, cancellationToken);
            return Response.Ok();
        }
    }
}
