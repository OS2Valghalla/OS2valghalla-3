using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Communication;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Queries;

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
        private readonly ICommunicationService communicationService;
        private readonly IElectionWorkLocationTasksCommandRepository electionWorkLocationTasksCommandRepository;
        private readonly IElectionWorkLocationTasksQueryRepository electionWorkLocationTasksQueryRepository;

        public RemoveParticipantFromTaskCommandHandler(
            ICommunicationService communicationService,
            IElectionWorkLocationTasksCommandRepository electionWorkLocationTasksCommandRepository,
            IElectionWorkLocationTasksQueryRepository electionWorkLocationTasksQueryRepository)
        {
            this.communicationService = communicationService;
            this.electionWorkLocationTasksCommandRepository = electionWorkLocationTasksCommandRepository;
            this.electionWorkLocationTasksQueryRepository = electionWorkLocationTasksQueryRepository;
        }

        public async Task<Response> Handle(RemoveParticipantFromTaskCommand command, CancellationToken cancellationToken)
        {
            var query = new GetTaskAssignmentQuery(command.TaskAssignmentId, command.ElectionId);
            var taskAssignment = await electionWorkLocationTasksQueryRepository.GetTaskAssignmentAsync(query, cancellationToken);

            if (!taskAssignment.ParticipantId.HasValue) throw new Exception("Task assignment has no participant!");

            await electionWorkLocationTasksCommandRepository.RemoveParticipantFromTaskAsync(command, cancellationToken);

            if (taskAssignment.Accepted && taskAssignment.Responsed)
            {
                await communicationService.SendRemovedFromTaskAsync(taskAssignment.ParticipantId.Value, taskAssignment.Id, cancellationToken);
            }
            else
            {
                await communicationService.SendTaskInvitationRetractedAsync(taskAssignment.ParticipantId.Value, taskAssignment.Id, cancellationToken);
            }

            return Response.Ok();
        }
    }
}
