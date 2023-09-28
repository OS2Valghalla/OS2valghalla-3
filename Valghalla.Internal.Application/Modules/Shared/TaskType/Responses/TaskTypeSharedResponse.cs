namespace Valghalla.Internal.Application.Modules.Shared.TaskType.Responses
{
    public sealed record TaskTypeSharedResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public string ShortName { get; init; } = null!;
    }
}
