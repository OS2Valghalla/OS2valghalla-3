using Valghalla.Application.Communication;
using Valghalla.Application.QueryEngine;
using Valghalla.Application.QueryEngine.Values;

namespace Valghalla.Internal.Application.Modules.Communication.CommunicationLog.Queries
{
    public sealed record CommunicationLogListingQueryFormParameters : QueryFormParameters
    {

    }

    public sealed record CommunicationLogListingQueryForm : QueryForm<CommunicationLogListingItem, CommunicationLogListingQueryFormParameters>
    {
        public MultipleSelectionFilterValue<int>? Status { get; init; }
    }
}
