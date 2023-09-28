using FluentValidation;
using Valghalla.Application.QueryEngine;
using Valghalla.Application.QueryEngine.Values;
using Valghalla.Internal.Application.Modules.Administration.Communication.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.Communication.Queries
{

    public sealed record CommunicationTemplateListingQueryFormParameters : QueryFormParameters
    {
    }

    public sealed class CommunicationTemplateListingQueryFormParametersValidator : AbstractValidator<CommunicationTemplateListingQueryFormParameters>
    {
        public CommunicationTemplateListingQueryFormParametersValidator()
        {
        }
    }

    public sealed record CommunicationTemplateListingQueryForm : QueryForm<CommunicationTemplateListingItemResponse, CommunicationTemplateListingQueryFormParameters>
    {
        public FreeTextSearchValue? Title { get; init; }
        public SingleSelectionFilterValue<int>? TemplateType { get; init; }
        public override Order? Order { get; init; } = new Order("title", false);

        public class TemplateTypes
        {
            public static readonly TemplateTypes DigitalPost = new(0, "communication.template_type.digital_post");
            public static readonly TemplateTypes Email = new(1, "communication.template_type.email");
            public static readonly TemplateTypes SMS = new(2, "communication.template_type.sms");

            public int Id { get; set; }
            public string Label { get; set; }

            private TemplateTypes(int id, string label) => (Id, Label) = (id, label);

            public static IEnumerable<TemplateTypes> GetAll() => new[] { DigitalPost, Email, SMS };
        }
    }

    public sealed class CommunicationTemplateListingQueryFormValidator : AbstractValidator<CommunicationTemplateListingQueryForm>
    {
        public CommunicationTemplateListingQueryFormValidator()
        {
        }
    }
}
