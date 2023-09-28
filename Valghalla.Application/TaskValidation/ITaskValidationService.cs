namespace Valghalla.Application.TaskValidation
{
    public interface ITaskValidationService
    {
        TaskValidationResult Execute(EvaluatedTaskType taskType, EvaluatedParticipant participant, IEnumerable<TaskValidationRule> rules);
        Task<TaskValidationResult> ExecuteAsync(Guid taskTypeId, Guid electionId, Guid participantId, CancellationToken cancellationToken);
        Task<TaskValidationResult> ExecuteAsync(Guid taskTypeId, Guid electionId, string cpr, CancellationToken cancellationToken);
    }
}
