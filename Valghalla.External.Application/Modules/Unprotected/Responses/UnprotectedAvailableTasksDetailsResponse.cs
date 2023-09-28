namespace Valghalla.External.Application.Modules.Unprotected.Responses
{
    public sealed record UnprotectedAvailableTasksDetailsResponse
    {
        public Guid WorkLocationId { get; set; }
        public string? WorkLocationName { get; set; }
        public string? WorkLocationAddress { get; set; }
        public string? WorkLocationPostalCode { get; set; }
        public string? WorkLocationCity { get; set; }
        public Guid TaskTypeId { get; set; }
        public string? TaskTypeName { get; set; }
        public string? TaskTypeDescription { get; set; }
        public TimeSpan? TaskTypeStartTime { get; set; }
        public DateTime TaskDate { get; set; }
        public int AvailableTasksCount { get; set; }
        public string HashValue { get; set; } = null!;
    }
}
