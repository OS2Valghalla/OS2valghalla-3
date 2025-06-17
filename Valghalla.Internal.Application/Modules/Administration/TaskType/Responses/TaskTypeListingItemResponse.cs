namespace Valghalla.Internal.Application.Modules.Administration.TaskType.Responses
{
    public class TaskTypeListingItemResponse
    {
        public Guid Id { get; init; }
        public Guid ElectionId { get; set; }
        public string Title { get; init; } = null!;
        public string ShortName { get; init; } = null!;
        public bool Trusted { get; init; }
        public Guid AreaId { get; init; }
        public string AreaName { get; init; } = string.Empty;
        public string TaskTypeTemplateId { get; set; } = string.Empty;
        public string ElectionTitle { get; set; } = string.Empty;
        public string TaskTypeTemplateTitle { get; set; } = string.Empty;
        public string WorkLocationTitle { get; set; } = string.Empty;
        public Guid WorkLocationId { get; set; }
    }
}
