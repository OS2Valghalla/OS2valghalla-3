using Valghalla.Application.Storage;

namespace Valghalla.Internal.Application.Modules.Administration.TaskType.Responses
{
    public class TaskTypeDetailResponse
    {
        public Guid Id { get; set; }
        public string Title { get; init; } = null!;
        public string ShortName { get; init; } = null!;
        public string Description { get; init; } = null!;
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
        public int? Payment { get; init; }
        public bool ValidationNotRequired { get; init; }
        public bool Trusted { get; init; }
        public bool SendingReminderEnabled { get; init; }
        public IEnumerable<FileReferenceInfo> FileReferences { get; init; } = Enumerable.Empty<FileReferenceInfo>();
    }
}
