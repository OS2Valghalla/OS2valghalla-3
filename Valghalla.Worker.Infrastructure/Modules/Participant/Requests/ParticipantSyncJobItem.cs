using Valghalla.Application.CPR;

namespace Valghalla.Worker.Infrastructure.Modules.Participant.Requests
{
    public sealed record ParticipantSyncJobItem
    {
        public Guid ParticipantId { get; init; }
        public ParticipantPersonalRecord Record { get; init; } = null!;
    }
}
