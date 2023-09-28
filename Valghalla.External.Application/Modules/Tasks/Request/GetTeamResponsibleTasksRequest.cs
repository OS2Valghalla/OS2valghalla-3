namespace Valghalla.External.Application.Modules.Tasks.Request
{
    public sealed record GetTeamResponsibleTasksRequest
    {
        public DateTime? TaskDate { get; set; }
        public Guid TeamId { get; set; }
        public Guid? WorkLocationId { get; set; }
        public Guid? TaskTypeId { get; set; }
    }
}
