namespace Valghalla.Internal.API.Requests.Administration.Communication
{
    public sealed record GetParticipantsForSendingGroupMessageRequest
    {
        public Guid ElectionId { get; init; }
        public ParticipantsForSendingGroupMessageFilterRequest Filters { get; init; } = null!;
    }

    public sealed record ParticipantsForSendingGroupMessageFilterRequest
    {
        public List<Guid>? TeamIds { get; set; }
        public List<Guid>? WorkLocationIds { get; set; }
        public List<Guid>? TaskTypeIds { get; set; }
        public List<DateTime>? TaskDates { get; set; }
        public List<Valghalla.Application.Enums.TaskStatus>? TaskStatuses { get; set; }
    }
}
