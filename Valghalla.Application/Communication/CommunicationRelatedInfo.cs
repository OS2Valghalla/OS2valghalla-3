namespace Valghalla.Application.Communication
{
    public sealed record CommunicationRelatedInfo
    {
        public string HashValue { get; init; } = null!;
        public string MunicipalityName { get; init; } = null!;
        public string ElectionTitle { get; init; } = null!;
        public CommunicationParticipantInfo Participant { get; init; } = null!;
        public CommunicationWorkLocationInfo WorkLocation { get; init; } = null!;
        public CommunicationTaskTypeInfo TaskType { get; init; } = null!;
        public DateTime TaskDate { get; init; }
        public Guid? InvitationCode { get; init; }
    }

    public sealed record CommunicationParticipantInfo
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string? MobileNumber { get; init; }
        public string? Email { get; init; }
        public string Cpr { get; init; } = null!;
        public bool ExemptDigitalPost { get; init; }
    }

    public sealed record CommunicationWorkLocationInfo
    {
        public string Title { get; init; } = null!;
        public string Address { get; init; } = null!;
    }

    public sealed record CommunicationTaskTypeInfo
    {
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public int? Payment { get; init; }
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
    }
}
