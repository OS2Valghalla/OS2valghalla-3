using Valghalla.Application.Storage;

namespace Valghalla.External.Application.Modules.Tasks.Responses
{
    public sealed record TaskPreviewResponse
    {
        public string HashValue { get; init; } = null!;
        public DateTime TaskDate { get; init; }
        public TaskPreviewTeam Team { get; init; } = null!;
        public TaskPreviewTaskType TaskType { get; init; } = null!;
        public TaskPreviewWorkLocation WorkLocation { get; init; } = null!;
    }

    public sealed record TaskDetailsResponse
    {
        public string HashValue { get; init; } = null!;
        public Guid TaskAssignmentId { get; init; }
        public DateTime TaskDate { get; init; }
        public bool Accepted { get; init; }
        public bool IsLocked { get; init; }
        public TaskPreviewTeam Team { get; init; } = null!;
        public TaskTypeIncludeFiles TaskType { get; init; } = null!;
        public TaskPreviewWorkLocation WorkLocation { get; init; } = null!;
    }

    public sealed record TaskPreviewTeam
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
    }

    public sealed record TaskPreviewTaskType
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
        public int? Payment { get; init; }
    }

    public sealed record TaskTypeIncludeFiles
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
        public int? Payment { get; init; }
        public IEnumerable<FileReferenceInfo> FileReferences { get; init; } = Enumerable.Empty<FileReferenceInfo>();
    }

    public sealed record TaskPreviewWorkLocation
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public string Address { get; init; } = null!;
        public string PostalCode { get; init; } = null!;
        public string City { get; init; } = null!;
    }
}
