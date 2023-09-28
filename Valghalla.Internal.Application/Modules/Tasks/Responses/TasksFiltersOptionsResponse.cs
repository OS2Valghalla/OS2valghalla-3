using Valghalla.Internal.Application.Modules.Shared.Area.Responses;
using Valghalla.Internal.Application.Modules.Shared.Team.Responses;
using Valghalla.Internal.Application.Modules.Shared.WorkLocation.Responses;

namespace Valghalla.Internal.Application.Modules.Tasks.Responses
{
    public sealed record TasksFiltersOptionsResponse
    {
        public DateTime ElectionStartDate { get; init; }
        public DateTime ElectionEndDate { get; init; }
        public IList<AreaSharedResponse> Areas { get; init; } = new List<AreaSharedResponse>();
        public IList<TeamSharedResponse> Teams { get; init; } = new List<TeamSharedResponse>();
        public IList<WorkLocationWithTeamIdsResponse> WorkLocations { get; init; } = new List<WorkLocationWithTeamIdsResponse>();
        public IList<TaskTypeWithTeamIdsResponse> TaskTypes { get; init; } = new List<TaskTypeWithTeamIdsResponse>();        
    }
}
