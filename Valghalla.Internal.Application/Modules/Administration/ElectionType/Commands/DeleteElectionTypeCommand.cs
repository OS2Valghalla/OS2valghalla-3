using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Confirmations;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.ElectionType.Commands
{
    public sealed record DeleteElectionTypeCommand(Guid Id) : ConfirmationCommand<Response> { }

    public sealed class DeleteElectionTypeCommandValidator : AbstractValidator<DeleteElectionTypeCommand>
    {
        public DeleteElectionTypeCommandValidator(IElectionTypeQueryRepository electionQueryRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x)
               .Must((command) => electionQueryRepository.CheckIfElectionTypeCanBeDeletedAsync(command.Id, default).Result)
               .WithMessage("administration.election_type.error.existing_election_connection_when_delete");
        }
    }

    public sealed class DeleteElectionTypeCommandConfirmator : Confirmator<DeleteElectionTypeCommand>
    {
        public override string Title => "administration.election_type.action.delete_confirmation.title";
        public override string Message => "administration.election_type.action.delete_confirmation.content";
    }

    internal class DeleteElectionTypeCommandHandler : ICommandHandler<DeleteElectionTypeCommand, Response>
    {
        private readonly IElectionTypeCommandRepository electionTypeCommandRepository;

        public DeleteElectionTypeCommandHandler(IElectionTypeCommandRepository electionTypeCommandRepository)
        {
            this.electionTypeCommandRepository = electionTypeCommandRepository;
        }

        public async Task<Response> Handle(DeleteElectionTypeCommand command, CancellationToken cancellationToken)
        {
            await electionTypeCommandRepository.DeleteElectionTypeAsync(command, cancellationToken);
            return Response.Ok();
        }
    }
}
