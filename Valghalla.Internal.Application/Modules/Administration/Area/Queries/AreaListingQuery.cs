using FluentValidation;
using Valghalla.Application.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.Area.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.Area.Queries
{
    public sealed record AreaListingQueryForm : QueryForm<AreaListingItemResponse, VoidQueryFormParameters>
    {
    }

    public sealed class AreaListingQueryFormValidator : AbstractValidator<AreaListingQueryForm>
    {
        public AreaListingQueryFormValidator()
        {
        }
    }
}
