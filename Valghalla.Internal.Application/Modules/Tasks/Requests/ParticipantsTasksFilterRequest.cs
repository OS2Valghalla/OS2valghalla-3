namespace Valghalla.Internal.Application.Modules.Tasks.Requests
{
    public sealed record ParticipantsTasksFilterRequest
    {
        public List<Guid>? TeamIds { get; set; }        
        public List<Guid>? AreaIds { get; set; }
        public List<Guid>? WorkLocationIds { get; set; }
        public List<Guid>? TaskTypeIds { get; set; }
        public List<DateTime>? TaskDates { get; set; }
        public Valghalla.Application.Enums.TaskStatus? TaskStatus { get; set; }
    }
}
