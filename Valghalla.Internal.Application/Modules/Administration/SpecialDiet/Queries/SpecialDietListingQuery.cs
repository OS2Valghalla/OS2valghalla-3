using FluentValidation;
using Valghalla.Application.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Queries
{
    public sealed record SpecialDietListingQueryFormParameters : QueryFormParameters
    {
    }

    public sealed class SpecialDietListingQueryFormParametersValidator : AbstractValidator<SpecialDietListingQueryFormParameters>
    {
        public SpecialDietListingQueryFormParametersValidator()
        {
        }
    }

    public sealed record SpecialDietListingQueryForm : QueryForm<SpecialDietResponse, SpecialDietListingQueryFormParameters>
    {
        public override Order? Order { get; init; } = new Order("title", false);
    }

    public sealed class SpecialDietListingQueryFormValidator : AbstractValidator<SpecialDietListingQueryForm>
    {
        public SpecialDietListingQueryFormValidator()
        {
        }
    }
}
