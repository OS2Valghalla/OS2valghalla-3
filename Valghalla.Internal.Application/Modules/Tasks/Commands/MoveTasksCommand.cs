using FluentValidation;

using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;

namespace Valghalla.Internal.Application.Modules.Tasks.Commands
{
    public sealed record MoveTasksCommand() : ICommand<Response<bool>>
    {
        public string[]? TaskIds { get; set; }
        public Guid? TargetTeamId { get; set; }
        public Guid? SourceTeamId { get; set; }
    }
    public sealed class MoveTasksCommandValidator : AbstractValidator<MoveTasksCommand>
    {
        public MoveTasksCommandValidator()
        {
            RuleFor(x => x.SourceTeamId)
                .NotEmpty();

            RuleFor(x => x.TargetTeamId)
                .NotEmpty();
        }
    }
    internal class MoveTasksCommandHandler : ICommandHandler<MoveTasksCommand, Response<bool>>
    {
        private readonly IElectionWorkLocationTasksCommandRepository electionWorkLocationTasksCommandRepository;

        public MoveTasksCommandHandler(IElectionWorkLocationTasksCommandRepository electionWorkLocationTasksCommandRepository)
        {
            this.electionWorkLocationTasksCommandRepository = electionWorkLocationTasksCommandRepository;
        }

        public async Task<Response<bool>> Handle(MoveTasksCommand command, CancellationToken cancellationToken)
        {
            var result = await electionWorkLocationTasksCommandRepository.MoveElectionWorkLocationTasksAsync(command, cancellationToken);
          
            return Response.Ok(result);
        }

    }
}
