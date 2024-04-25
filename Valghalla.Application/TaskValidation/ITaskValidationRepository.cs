namespace Valghalla.Application.TaskValidation
{
    public interface ITaskValidationRepository
    {
        Task<IEnumerable<TaskValidationRule>> GetValidationRules(Guid electionId, CancellationToken cancellationToken);
        Task<EvaluatedTask> GetEvaluatedTask(Guid taskAssignmentId, CancellationToken cancellationToken);
        Task<EvaluatedParticipant> GetEvaluatedParticipant(Guid participantId, CancellationToken cancellationToken);
    }
}
