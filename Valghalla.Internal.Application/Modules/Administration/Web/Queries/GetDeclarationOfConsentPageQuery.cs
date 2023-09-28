using System.Text.Json;
using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Web;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Web.Queries
{
    public sealed record GetDeclarationOfConsentPageQuery() : IQuery<Response>;
    internal sealed class GetDeclarationOfConsentPageQueryHandler : IQueryHandler<GetDeclarationOfConsentPageQuery>
    {
        private readonly IWebPageQueryRepository webPageQueryRepository;

        public GetDeclarationOfConsentPageQueryHandler(IWebPageQueryRepository webPageQueryRepository)
        {
            this.webPageQueryRepository = webPageQueryRepository;
        }

        public async Task<Response> Handle(GetDeclarationOfConsentPageQuery request, CancellationToken cancellationToken)
        {
            var result = await webPageQueryRepository.GetWebPageAsync(Constants.WebPages.WebPageName_DeclarationOfConsentPage, cancellationToken);

            var pageResponse = new DeclarationOfConsentPage()
            {
                PageContent = string.Empty,
                IsActivated = false
            };

            if (result != null)
            {
                pageResponse = JsonSerializer.Deserialize<DeclarationOfConsentPage>(result.PageInfo);
            }

            return Response.Ok(pageResponse);
        }
    }
}
