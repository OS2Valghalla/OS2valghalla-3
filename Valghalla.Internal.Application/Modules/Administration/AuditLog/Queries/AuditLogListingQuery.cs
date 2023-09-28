using Valghalla.Application.QueryEngine;
using Valghalla.Application.QueryEngine.Values;
using Valghalla.Internal.Application.Modules.Administration.AuditLog.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.AuditLog.Queries
{
    public sealed record AuditLogListingQueryFormParameters : QueryFormParameters
    {

    }

    public sealed record AuditLogListingQueryForm : QueryForm<AuditLogListingItemResponse, AuditLogListingQueryFormParameters>
    {
        public MultipleSelectionFilterValue<string>? EventType { get; init; }
        public MultipleSelectionFilterValue<string>? EventTable { get; init; }
    }
}
