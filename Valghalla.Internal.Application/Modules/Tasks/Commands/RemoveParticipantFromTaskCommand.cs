using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;

namespace Valghalla.Internal.Application.Modules.Tasks.Commands
{
    public sealed record RemoveParticipantFromTaskCommand() : ICommand<Response>
    {
        public Guid TaskAssignmentId { get; set; }
        public Guid ElectionId { get; set; }
    }

    public sealed class RemoveParticipantFromTaskCommandValidator : AbstractValidator<RemoveParticipantFromTaskCommand>
    {
        public RemoveParticipantFromTaskCommandValidator()
        {
            RuleFor(x => x.TaskAssignmentId)
                .NotEmpty();

            RuleFor(x => x.ElectionId)
                .NotEmpty();
        }
    }

    internal class RemoveParticipantFromTaskCommandHandler : ICommandHandler<RemoveParticipantFromTaskCommand, Response>
    {
        private readonly IElectionWorkLocationTasksCommandRepository electionWorkLocationTasksCommandRepository;

        public RemoveParticipantFromTaskCommandHandler(IElectionWorkLocationTasksCommandRepository electionWorkLocationTasksCommandRepository)
        {
            this.electionWorkLocationTasksCommandRepository = electionWorkLocationTasksCommandRepository;
        }

        public async Task<Response> Handle(RemoveParticipantFromTaskCommand command, CancellationToken cancellationToken)
        {
            await electionWorkLocationTasksCommandRepository.RemoveParticipantFromTaskAsync(command, cancellationToken);
            return Response.Ok();
        }
    }
}
