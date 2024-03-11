namespace Valghalla.Application.Communication
{
    public sealed record TaskAssignmentCommunicationInfo
    {
        public Guid Id { get; init; }
        public Guid? ParticipantId { get; init; }
        public bool InvitationSent { get; init; }
        public bool RegistrationSent { get; init; }
        public bool Responsed { get; init; }
        public bool Accepted { get; init; }
        public bool Active { get; init; }
        public DateTime? ReminderDate { get; set; }
    }
}
