namespace Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Responses
{
    public class TaskTypeTemplateListingItemResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public string ShortName { get; init; } = null!;
        public bool Trusted { get; init; }
    }
}
