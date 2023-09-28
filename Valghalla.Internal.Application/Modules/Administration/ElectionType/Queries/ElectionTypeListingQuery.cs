using Valghalla.Application.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.ElectionType.Queries
{
    public sealed record ElectionTypeListingQueryForm : QueryForm<ElectionTypeResponse, VoidQueryFormParameters>
    {
    }
}
