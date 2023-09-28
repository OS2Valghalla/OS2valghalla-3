namespace Valghalla.External.API.Requests.Tasks
{
    public sealed record GetTaskOverviewRequest
    {
        public DateTime? TaskDate { get; init; }
        public Guid? TaskTypeId { get; init; }
        public Guid? WorkLocationId { get; init; }
        public Guid? TeamId { get; init; }
    }
}
