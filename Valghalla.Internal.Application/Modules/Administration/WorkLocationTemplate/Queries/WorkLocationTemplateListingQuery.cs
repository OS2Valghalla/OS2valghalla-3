using FluentValidation;
using Valghalla.Application.QueryEngine;
using Valghalla.Application.QueryEngine.Values;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Queries
{
    public sealed record WorkLocationTemplateListingQueryFormParameters : QueryFormParameters
    {
    }

    public sealed class WorkLocationTemplateListingQueryFormParametersValidator : AbstractValidator<WorkLocationTemplateListingQueryFormParameters>
    {
        public WorkLocationTemplateListingQueryFormParametersValidator()
        {
        }
    }

    public sealed record WorkLocationTemplateListingQueryForm : QueryForm<WorkLocationTemplateResponse, WorkLocationTemplateListingQueryFormParameters>
    {
        public FreeTextSearchValue? Title { get; init; }
        public FreeTextSearchValue? PostalCode { get; init; }
        public SingleSelectionFilterValue<Guid>? Area { get; init; }
        public override Order? Order { get; init; } = new Order("title", false);
    }

    public sealed class WorkLocationTemplateListingQueryFormValidator : AbstractValidator<WorkLocationTemplateListingQueryForm>
    {
        public WorkLocationTemplateListingQueryFormValidator()
        {
        }
    }
}
