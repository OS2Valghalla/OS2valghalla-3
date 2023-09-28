namespace Valghalla.External.Application.Modules.WorkLocation.Responses
{
    public sealed record WorkLocationParticipantDetailsResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public int Age { get; set; }
        public string TaskTypes { get; set; } = string.Empty;
        public string Teams { get; set; } = string.Empty;
    }
}
