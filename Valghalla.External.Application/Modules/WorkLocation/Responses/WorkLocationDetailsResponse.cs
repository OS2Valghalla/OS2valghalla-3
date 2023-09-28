namespace Valghalla.External.Application.Modules.WorkLocation.Responses
{
    public sealed record WorkLocationDetailsResponse
    {
        public int AcceptedTasksCount { get; set; }
        public int AllTasksCount { get; set; }
        public IList<WorkLocationTaskTypeDetailsResponse> TaskTypes { get; set; } = new List<WorkLocationTaskTypeDetailsResponse>();
        public IList<WorkLocationParticipantDetailsResponse> Participants { get; set; } = new List<WorkLocationParticipantDetailsResponse>();
    }
}
