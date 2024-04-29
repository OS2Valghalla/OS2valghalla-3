namespace Valghalla.Application.TaskValidation
{
    public interface ITaskValidationService
    {
        TaskValidationResult Execute(EvaluatedTask taskAssignment, EvaluatedParticipant participant, IEnumerable<TaskValidationRule> rules);
        Task<TaskValidationResult> ExecuteAsync(Guid taskAssignmentId, Guid electionId, Guid participantId, CancellationToken cancellationToken);
        Task<TaskValidationResult> ExecuteAsync(Guid taskAssignmentId, Guid electionId, string cpr, CancellationToken cancellationToken);
    }
}
