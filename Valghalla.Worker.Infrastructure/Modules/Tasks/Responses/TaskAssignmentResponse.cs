namespace Valghalla.Worker.Infrastructure.Modules.Tasks.Responses
{
    public sealed record TaskAssignmentResponse
    {
        public Guid Id { get; init; }
        public Guid ParticipantId { get; init; }
        public Guid TaskTypeId { get; init; }
        public Guid ElectionId { get; init; }
        public DateTime? InvitationDate { get; set; }
        public DateTime? InvitationReminderDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public DateTime TaskDate { get; set; }
        public bool Responsed { get; set; }
        public bool Accepted { get; set; }
    }
}
