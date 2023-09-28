using Valghalla.Application.QueryEngine;

namespace Valghalla.External.Application.Modules.Tasks.Responses
{
    public sealed record TaskOverviewFilterOptions
    {
        public IList<DateTime> TaskDates { get; init; } = new List<DateTime>();
        public IEnumerable<SelectOption<Guid>> WorkLocations { get; init; } = Enumerable.Empty<SelectOption<Guid>>();
        public IEnumerable<SelectOption<Guid>> TaskTypes { get; init; } = Enumerable.Empty<SelectOption<Guid>>();
        public IEnumerable<SelectOption<Guid>> Teams { get; init; } = Enumerable.Empty<SelectOption<Guid>>();
    }
}
