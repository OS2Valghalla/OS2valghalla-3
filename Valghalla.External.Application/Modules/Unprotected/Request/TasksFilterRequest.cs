namespace Valghalla.External.Application.Modules.Unprotected.Request
{
    public sealed record TasksFilterRequest
    {
        public Guid ElectionId { get; set; }
        public Guid TeamId { get; set; }
        public DateTime? TaskDate { get; set; }
        public List<Guid>? AreaIds { get; set; }
        public List<Guid>? WorkLocationIds { get; set; }
        public List<Guid>? TaskTypeIds { get; set; }
        public bool? TrustedTaskType { get; set; }
    }
}
