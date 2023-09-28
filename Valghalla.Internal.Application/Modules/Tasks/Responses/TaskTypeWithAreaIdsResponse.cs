using Valghalla.Internal.Application.Modules.Shared.TaskType.Responses;

namespace Valghalla.Internal.Application.Modules.Tasks.Responses
{
    public sealed record TaskTypeWithAreaIdsResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public string ShortName { get; set; } = null!;
        public List<Guid> AreaIds { get; set; } = new List<Guid>();
    }
}
