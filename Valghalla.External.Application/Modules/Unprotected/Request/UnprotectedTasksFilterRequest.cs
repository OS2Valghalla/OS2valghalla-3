namespace Valghalla.External.Application.Modules.Unprotected.Request
{
    public sealed record UnprotectedTasksFilterRequest
    {
        public DateTime? TaskDate { get; set; }
        public List<Guid>? WorkLocationIds { get; set; }
    }
}
