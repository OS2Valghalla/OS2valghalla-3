using Valghalla.Internal.Application.Modules.Tasks.Commands;

namespace Valghalla.Internal.Application.Modules.Tasks.Interfaces
{
    public interface IElectionWorkLocationTasksCommandRepository
    {
        Task DistributeElectionWorkLocationTasksAsync(DistributeElectionWorkLocationTasksCommand command, CancellationToken cancellationToken);
        Task<bool> AssignParticipantToTaskAsync(AssignParticipantToTaskCommand command, CancellationToken cancellationToken);
        Task<bool> AssignCreatingParticipantToTaskAsync(AssignParticipantToTaskCommand command, IEnumerable<Guid> teamIds, CancellationToken cancellationToken);
        Task RemoveParticipantFromTaskAsync(RemoveParticipantFromTaskCommand command, CancellationToken cancellationToken);
        Task<Guid?> ReplyForParticipantAsync(ReplyForParticipantCommand command, CancellationToken cancellationToken);
    }
}
