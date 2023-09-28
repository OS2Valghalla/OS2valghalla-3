namespace Valghalla.Internal.Application.Modules.Participant.Responses
{
    public record ParticipantTaskResponse
    {
        public Guid Id { get; set; }
        public Guid ElectionId { get; set; }
        public string? ElectionName { get; set; }
        public Guid WorkLocationId { get; set; }
        public string? WorkLocationName { get; set; }
        public Guid TaskTypeId { get; set; }
        public string? TaskTypeName { get; set; }
        public Guid TeamId { get; set; }
        public string? TeamName { get; set; }
        public DateTime TaskDate { get; set; }
        public string Status { get; set; }
    }
}
