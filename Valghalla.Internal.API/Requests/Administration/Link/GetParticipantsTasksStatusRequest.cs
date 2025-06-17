using Valghalla.Internal.Application.Modules.Tasks.Requests;

namespace Valghalla.Internal.API.Requests.Administration.Link
{
    public sealed record GetParticipantsTasksStatusRequest
    {
        public Guid ElectionId { get; init; }
        public ParticipantsTasksFilterRequest TasksFilter { get; init; } = null!;
    }
}
