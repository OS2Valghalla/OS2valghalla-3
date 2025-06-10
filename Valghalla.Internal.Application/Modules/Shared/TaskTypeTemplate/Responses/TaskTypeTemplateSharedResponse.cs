namespace Valghalla.Internal.Application.Modules.Shared.TaskTypeTemplate.Responses
{
    public sealed record TaskTypeTemplateSharedResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public string ShortName { get; init; } = null!;
    }
}
