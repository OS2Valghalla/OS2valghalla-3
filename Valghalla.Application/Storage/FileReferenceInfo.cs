using System.Text.Json.Serialization;

namespace Valghalla.Application.Storage
{
    public sealed record FileReferenceInfo
    {
        public Guid Id { get; init; }
        public string FileName { get; init; } = null!;

        public DateTime CreatedAt { get; init; }
        public Guid CreatedBy { get; init; }
        public DateTime? ChangedAt { get; init; }
        public Guid? ChangedBy { get; init; }
    }
}
