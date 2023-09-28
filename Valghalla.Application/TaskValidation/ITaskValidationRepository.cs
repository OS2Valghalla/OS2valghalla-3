namespace Valghalla.Application.TaskValidation
{
    public interface ITaskValidationRepository
    {
        Task<IEnumerable<TaskValidationRule>> GetValidationRules(Guid electionId, CancellationToken cancellationToken);
        Task<EvaluatedTaskType> GetEvaluatedTaskType(Guid taskTypeId, CancellationToken cancellationToken);
        Task<EvaluatedTaskType> GetEvaluatedTaskTypeByTaskId(Guid taskId, CancellationToken cancellationToken);
        Task<EvaluatedParticipant> GetEvaluatedParticipant(Guid participantId, CancellationToken cancellationToken);
    }
}
