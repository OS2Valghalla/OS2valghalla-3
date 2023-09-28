using Valghalla.Internal.Application.Modules.Shared.Area.Responses;
using Valghalla.Internal.Application.Modules.Shared.TaskType.Responses;
using Valghalla.Internal.Application.Modules.Shared.Team.Responses;
using Valghalla.Internal.Application.Modules.Shared.WorkLocation.Responses;

namespace Valghalla.Internal.Application.Modules.Tasks.Responses
{
    public sealed record ElectionAreasGeneralInfoResponse
    {
        public DateTime ElectionStartDate { get; init; }
        public DateTime ElectionEndDate { get; init; }
        public IList<AreaSharedResponse> Areas { get; init; } = new List<AreaSharedResponse>();
        public IList<WorkLocationSharedResponse> WorkLocations { get; init; } = new List<WorkLocationSharedResponse>();
        public IList<TaskTypeWithAreaIdsResponse> TaskTypes { get; init; } = new List<TaskTypeWithAreaIdsResponse>();
        public IList<TeamSharedResponse> Teams { get; init; } = new List<TeamSharedResponse>();
    }
}
