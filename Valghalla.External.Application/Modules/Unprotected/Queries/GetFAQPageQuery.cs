using System.Text.Json;
using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Web;
using Valghalla.External.Application.Modules.Web.Interfaces;

namespace Valghalla.External.Application.Modules.Unprotected.Queries
{
    public sealed record GetFAQPageQuery() : IQuery<Response>;

    internal class GetFAQPageQueryHandler : IQueryHandler<GetFAQPageQuery>
    {
        private readonly IWebPageQueryRepository webPageQueryRepository;

        public GetFAQPageQueryHandler(IWebPageQueryRepository webPageQueryRepository)
        {
            this.webPageQueryRepository = webPageQueryRepository;
        }

        public async Task<Response> Handle(GetFAQPageQuery request, CancellationToken cancellationToken)
        {
            var webPage = await webPageQueryRepository.GetWebPageAsync(Constants.WebPages.WebPageName_FAQPage, cancellationToken);

            var result = webPage != null ?
                JsonSerializer.Deserialize<FAQPage>(webPage.PageInfo) :
                new FAQPage();

            return Response.Ok(result);
        }
    }
}
