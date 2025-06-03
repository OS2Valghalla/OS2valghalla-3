namespace Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Responses
{
    public sealed record WorkLocationTemplateResponsibleResponse
    {
        public Guid Id { get; init; }
        public Guid WorkLocationTemplateId { get; init; }
        public Guid ParticipantId { get; init; }
        public string? ParticipantFirstName { get; set; }
        public string? ParticipantLastName { get; set; }
        public string? ParticipantMobileNumber { get; set; }
        public string? ParticipantEmail { get; set; }
    }
}
