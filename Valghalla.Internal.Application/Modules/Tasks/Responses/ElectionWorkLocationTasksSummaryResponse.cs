using Valghalla.Internal.Application.Modules.Shared.TaskType.Responses;
using Valghalla.Internal.Application.Modules.Shared.Team.Responses;

namespace Valghalla.Internal.Application.Modules.Tasks.Responses
{
    public sealed record ElectionWorkLocationTasksSummaryResponse
    {
        public DateTime ElectionStartDate { get; init; }
        public DateTime ElectionEndDate { get; init; }
        public DateTime ElectionDate { get; init; }
        public IList<TaskTypeSharedResponse> TaskTypes { get; init; } = new List<TaskTypeSharedResponse>();
        public IList<TeamSharedResponse> Teams { get; init; } = new List<TeamSharedResponse>();
        public IList<TeamTasksSummaryResponse> Tasks { get; init; } = new List<TeamTasksSummaryResponse>();
    }
}
