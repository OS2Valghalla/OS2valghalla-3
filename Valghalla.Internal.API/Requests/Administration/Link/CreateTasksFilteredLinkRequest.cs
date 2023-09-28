using Valghalla.Internal.Application.Modules.Tasks.Requests;

namespace Valghalla.Internal.API.Requests.Administration.Link
{
    public sealed record CreateTasksFilteredLinkRequest
    {
        public Guid ElectionId { get; init; }
        public TasksFilterRequest TasksFilter { get; init; } = null!;
    }
}
