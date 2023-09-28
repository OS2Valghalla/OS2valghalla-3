using Valghalla.Application.QueryEngine;
using Valghalla.Application.QueryEngine.Values;
using Valghalla.Internal.Application.Modules.Participant.Responses;

namespace Valghalla.Internal.Application.Modules.Participant.Queries
{
    public sealed record ParticipantListingQueryFormParameters : QueryFormParameters
    {

    }

    public sealed record ParticipantListingQueryForm : QueryForm<ParticipantListingItemResponse, ParticipantListingQueryFormParameters>
    {
        public DateTimeFilterValue? Birthdate { get; init; }
        public MultipleSelectionFilterValue<Guid>? Teams { get; init; }
        public BooleanFilterValue? DigitalPost { get; init; }
        public BooleanFilterValue? AssignedTask { get; init; }
        public MultipleSelectionFilterValue<Guid>? TaskTypes { get; init; }
    }
}
