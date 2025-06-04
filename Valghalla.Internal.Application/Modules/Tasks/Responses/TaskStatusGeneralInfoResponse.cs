namespace Valghalla.Internal.Application.Modules.Tasks.Responses
{
    public sealed record TaskStatusGeneralInfoResponse
    {       
        public int AssignedTasksCount { get; init; }
        public int MissingTasksCount { get; init; }
        public int AwaitingTasksCount { get; init; }
        public int AllTasksCount { get; init; }
        public int RejectedTasksCount { get; set; }
        public List<RejectedTasksInfoResponse> RejectedTasksInfoResponses { get; set; } = new List<RejectedTasksInfoResponse>();
    }
}
