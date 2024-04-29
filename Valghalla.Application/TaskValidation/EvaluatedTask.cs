namespace Valghalla.Application.TaskValidation
{
    public sealed record EvaluatedTask
    {
        public Guid TaskAssignmentId { get; init; }
        public Guid TaskTypeId { get; init; }
        public DateTime TaskDate { get; init; }
        public bool ValidationNotRequired { get; init; }
    }
}
