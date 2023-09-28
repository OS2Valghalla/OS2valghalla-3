using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.ElectionType.Commands
{
    public sealed record CreateElectionTypeCommand(string Title, List<Guid> ValidationRuleIds) : ICommand<Response<Guid>>;

    public sealed class CreateElectionTypeCommandValidator : AbstractValidator<CreateElectionTypeCommand>
    {
        public CreateElectionTypeCommandValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();

        }
    }

    internal class CreateElectionTypeCommandHandler : ICommandHandler<CreateElectionTypeCommand, Response<Guid>>
    {
        private readonly IElectionTypeCommandRepository electionTypeCommandRepository;

        public CreateElectionTypeCommandHandler(IElectionTypeCommandRepository electionTypeCommandRepository)
        {
            this.electionTypeCommandRepository = electionTypeCommandRepository;
        }

        public async Task<Response<Guid>> Handle(CreateElectionTypeCommand command, CancellationToken cancellationToken)
        {
            var id = await electionTypeCommandRepository.CreateElectionTypeAsync(command, cancellationToken);
            return Response.Ok(id);
        }
    }
}
