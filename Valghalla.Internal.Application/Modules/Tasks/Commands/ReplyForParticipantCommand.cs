using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Communication;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;

namespace Valghalla.Internal.Application.Modules.Tasks.Commands
{
    public sealed record ReplyForParticipantCommand() : ICommand<Response>
    {
        public Guid TaskAssignmentId { get; set; }
        public Guid ElectionId { get; set; }
        public bool Accepted { get; set; }
        public string? MobileNumber { get; init; }
        public string? Email { get; init; }
        public IEnumerable<Guid> SpecialDietIds { get; init; } = Enumerable.Empty<Guid>();
    }

    public sealed class ReplyForParticipantCommandValidator : AbstractValidator<ReplyForParticipantCommand>
    {
        public ReplyForParticipantCommandValidator()
        {
            RuleFor(x => x.TaskAssignmentId)
                .NotEmpty();

            RuleFor(x => x.ElectionId)
                .NotEmpty();
        }
    }

    internal class ReplyForParticipantCommandHandler : ICommandHandler<ReplyForParticipantCommand, Response>
    {
        private readonly ICommunicationService communicationService;
        private readonly IElectionWorkLocationTasksCommandRepository electionWorkLocationTasksCommandRepository;

        public ReplyForParticipantCommandHandler(ICommunicationService communicationService, IElectionWorkLocationTasksCommandRepository electionWorkLocationTasksCommandRepository)
        {
            this.communicationService = communicationService;
            this.electionWorkLocationTasksCommandRepository = electionWorkLocationTasksCommandRepository;
        }

        public async Task<Response> Handle(ReplyForParticipantCommand command, CancellationToken cancellationToken)
        {
            var participantId = await electionWorkLocationTasksCommandRepository.ReplyForParticipantAsync(command, cancellationToken);

            if (command.Accepted && participantId.HasValue)
            {
                await communicationService.SendTaskRegistrationAsync(participantId.Value, command.TaskAssignmentId, cancellationToken);
            }

            return Response.Ok();
        }
    }
}
