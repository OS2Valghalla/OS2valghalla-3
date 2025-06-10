namespace Valghalla.Internal.API.Requests.Administration.TaskType
{
    public class UpdateTaskTypeRequest
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public string ShortName { get; init; } = null!;
        public string Description { get; init; } = null!;
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
        public int? Payment { get; init; }
        public bool ValidationNotRequired { get; init; }
        public bool Trusted { get; init; }
        public bool SendingReminderEnabled { get; init; } = true;
        public IEnumerable<Guid> FileReferenceIds { get; init; } = Enumerable.Empty<Guid>();
        public Guid NewElectionId { get; set; }
        public Guid ElectionId { get; set; }
        public Guid NewWorkLocationId { get; set; }
        public Guid WorkLocationId { get; set; }
        public Guid NewTaskTypeTemplateId { get; set; }
        public Guid TaskTypeTemplateId { get; set; }

    }
}
