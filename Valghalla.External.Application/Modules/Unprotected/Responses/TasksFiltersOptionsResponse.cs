using Valghalla.External.Application.Modules.Shared.WorkLocation.Responses;
using Valghalla.External.Application.Modules.Unprotected.Request;

namespace Valghalla.External.Application.Modules.Unprotected.Responses
{
    public sealed record TasksFiltersOptionsResponse
    {
        public IList<DateTime> TaskDates { get; init; } = new List<DateTime>();
        public IList<WorkLocationSharedResponse> WorkLocations { get; init; } = new List<WorkLocationSharedResponse>();
        public UnprotectedTasksFilterRequest TasksFilter { get; init; } = null!;
    }
}
