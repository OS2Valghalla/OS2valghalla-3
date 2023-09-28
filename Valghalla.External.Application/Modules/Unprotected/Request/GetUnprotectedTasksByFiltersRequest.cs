namespace Valghalla.External.Application.Modules.Unprotected.Request
{
    public sealed record GetUnprotectedTasksByFiltersRequest
    {
        public string HashValue { get; init; } = null!;
        public UnprotectedTasksFilterRequest TasksFilter { get; init; } = null!;
    }
}
