namespace Valghalla.Application.TaskValidation
{
    public sealed record EvaluatedTaskType
    {
        public Guid Id { get; init; }
        public bool ValidationNotRequired { get; init; }
    }
}
