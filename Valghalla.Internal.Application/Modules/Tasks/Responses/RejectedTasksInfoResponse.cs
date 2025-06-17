namespace Valghalla.Internal.Application.Modules.Tasks.Responses
{
    public sealed record RejectedTasksInfoResponse
    {
        public Guid TaskId { get; set; }
        public Guid TaskTypeId { get; set; }
        public Guid TeamId { get; set; }
        public Guid ParticipantId { get; set; }
        public DateTime TasksDate { get; set; }
    }
}