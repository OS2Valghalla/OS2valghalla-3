using Valghalla.Application.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.Election.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.Election.Queries
{
    public sealed record ElectionListingQueryForm : QueryForm<ElectionListingItemResponse, VoidQueryFormParameters>
    {
    }
}
