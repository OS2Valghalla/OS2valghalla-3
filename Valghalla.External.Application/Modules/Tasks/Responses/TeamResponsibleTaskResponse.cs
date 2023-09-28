namespace Valghalla.External.Application.Modules.Tasks.Responses
{
    public sealed record TeamResponsibleTaskResponse
    {
        public int TotalUnansweredTasksCount { get; set; }
        public int TotalAcceptedTasksCount { get; set; }
        public int TotalTasksCount { get; set; }
        public IList<TeamResponsibleTaskDetailsResponse> Tasks { get; set; } = new List<TeamResponsibleTaskDetailsResponse>();
    }

    public sealed record TeamResponsibleTaskDetailsResponse
    {
        public Guid WorkLocationId { get; set; }
        public string? WorkLocationName { get; set; }
        public string? WorkLocationAddress { get; set; }
        public string? WorkLocationPostalCode { get; set; }
        public string? WorkLocationCity { get; set; }
        public Guid TaskTypeId { get; set; }
        public string? TaskTypeName { get; set; }
        public bool TrustedTaskType { get; set; }
        public string? TaskTypeDescription { get; set; }
        public TimeSpan? TaskTypeStartTime { get; set; }
        public DateTime TaskDate { get; set; }
        public int UnansweredTasksCount { get; set; }
        public int AcceptedTasksCount { get; set; }
        public int AllTasksCount { get; set; }
        public string HashValue { get; set; } = null!;
    }
}
