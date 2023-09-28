using System.Text.Json;
using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Web;
using Valghalla.External.Application.Modules.Web.Interfaces;

namespace Valghalla.External.Application.Modules.Web.Queries
{
    public sealed record GetDeclarationOfConsentPageQuery() : IQuery<Response>;

    internal class GetDeclarationOfConsentPageQueryHandler : IQueryHandler<GetDeclarationOfConsentPageQuery>
    {
        private readonly IWebPageQueryRepository webPageQueryRepository;

        public GetDeclarationOfConsentPageQueryHandler(IWebPageQueryRepository webPageQueryRepository)
        {
            this.webPageQueryRepository = webPageQueryRepository;
        }

        public async Task<Response> Handle(GetDeclarationOfConsentPageQuery request, CancellationToken cancellationToken)
        {
            var webPage = await webPageQueryRepository.GetWebPageAsync(Constants.WebPages.WebPageName_DeclarationOfConsentPage, cancellationToken);
            
            var result = webPage != null ?
                JsonSerializer.Deserialize<DeclarationOfConsentPage>(webPage.PageInfo) :
                new DeclarationOfConsentPage();

            return Response.Ok(result);
        }
    }
}
