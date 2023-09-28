namespace Valghalla.Internal.Application.Modules.Tasks.Requests
{
    public sealed record TasksDistributingRequest
    {
        public DateTime TasksDate { get; set; }
        public Guid TeamId { get; set; }
        public Guid TaskTypeId { get; set; }
        public int TasksCount { get; set; }
    }

    public sealed record ElectionWorkLocationTasksDistributingRequest
    {
        public Guid ElectionId { get; set; }
        public Guid WorkLocationId { get; set; }
        public IList<TasksDistributingRequest> DistributingTasks { get; set; } = new List<TasksDistributingRequest>();

    }
}
