using FluentValidation;
using Valghalla.Application.QueryEngine;
using Valghalla.Application.QueryEngine.Values;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.WorkLocation.Queries
{
    public sealed record WorkLocationListingQueryFormParameters : QueryFormParameters
    {
    }

    public sealed class WorkLocationListingQueryFormParametersValidator : AbstractValidator<WorkLocationListingQueryFormParameters>
    {
        public WorkLocationListingQueryFormParametersValidator()
        {
        }
    }

    public sealed record WorkLocationListingQueryForm : QueryForm<WorkLocationResponse, WorkLocationListingQueryFormParameters>
    {
        public FreeTextSearchValue? Title { get; init; }
        public FreeTextSearchValue? PostalCode { get; init; }
        public SingleSelectionFilterValue<Guid>? Area { get; init; }
        public override Order? Order { get; init; } = new Order("title", false);
    }

    public sealed class WorkLocationListingQueryFormValidator : AbstractValidator<WorkLocationListingQueryForm>
    {
        public WorkLocationListingQueryFormValidator()
        {
        }
    }
}
