namespace Valghalla.Internal.Application.Modules.Administration.WorkLocation.Responses
{
    public sealed record WorkLocationResponsibleResponse
    {
        public Guid Id { get; init; }
        public Guid WorkLocationId { get; init; }
        public Guid ParticipantId { get; init; }
        public string? ParticipantFirstName { get; set; }
        public string? ParticipantLastName { get; set; }
        public string? ParticipantMobileNumber { get; set; }
        public string? ParticipantEmail { get; set; }
    }
}
