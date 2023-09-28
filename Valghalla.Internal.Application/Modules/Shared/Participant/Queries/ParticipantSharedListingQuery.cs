using Valghalla.Application.QueryEngine;
using Valghalla.Application.QueryEngine.Values;
using Valghalla.Internal.Application.Modules.Shared.Participant.Responses;

namespace Valghalla.Internal.Application.Modules.Shared.Participant.Queries
{
    public sealed record ParticipantSharedListingQueryFormParameters : QueryFormParameters
    {

    }

    public sealed record ParticipantSharedListingQueryForm : QueryForm<ParticipantSharedListingItemResponse, ParticipantSharedListingQueryFormParameters>
    {
        public DateTimeFilterValue? Birthdate { get; init; }
        public MultipleSelectionFilterValue<Guid>? Teams { get; init; }
    }
}
