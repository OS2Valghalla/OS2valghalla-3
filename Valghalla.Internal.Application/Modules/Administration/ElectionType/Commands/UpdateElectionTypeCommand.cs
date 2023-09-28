using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.ElectionType.Commands
{
    public sealed record UpdateElectionTypeCommand(Guid Id, string Title, List<Guid> ValidationRuleIds) : ICommand<Response>;

    public sealed class UpdateElectionTypeCommandValidator : AbstractValidator<UpdateElectionTypeCommand>
    {
        public UpdateElectionTypeCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Title)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();
        }
    }

    internal class UpdateElectionTypeCommandHandler : ICommandHandler<UpdateElectionTypeCommand, Response>
    {
        private readonly IElectionTypeCommandRepository electionTypeCommandRepository;

        public UpdateElectionTypeCommandHandler(IElectionTypeCommandRepository electionTypeCommandRepository)
        {
            this.electionTypeCommandRepository = electionTypeCommandRepository;
        }

        public async Task<Response> Handle(UpdateElectionTypeCommand command, CancellationToken cancellationToken)
        {
            await electionTypeCommandRepository.UpdateElectionTypeAsync(command, cancellationToken);
            return Response.Ok();
        }
    }
}
