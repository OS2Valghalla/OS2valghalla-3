using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Election.Commands
{
    public sealed record UpdateElectionCommand() : ICommand<Response>
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public int LockPeriod { get; init; }
    }

    public sealed class UpdateElectionCommandValidator : AbstractValidator<UpdateElectionCommand>
    {
        public UpdateElectionCommandValidator(IElectionQueryRepository electionQueryRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Title)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();

            RuleFor(x => x.LockPeriod)
                .GreaterThan(0);

            RuleFor(x => x)
               .Must((command) => !electionQueryRepository.CheckIfElectionExistsAsync(command, default).Result)
               .WithMessage("administration.election.error.election_exist");
        }
    }

    internal class UpdateElectionCommandHandler : ICommandHandler<UpdateElectionCommand, Response>
    {
        private readonly IElectionCommandRepository electionCommandRepository;

        public UpdateElectionCommandHandler(IElectionCommandRepository electionCommandRepository)
        {
            this.electionCommandRepository = electionCommandRepository;
        }

        public async Task<Response> Handle(UpdateElectionCommand command, CancellationToken cancellationToken)
        {
            await electionCommandRepository.UpdateElectionAsync(command, cancellationToken);
            return Response.Ok();
        }
    }

}
