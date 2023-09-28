using System.Text.Json;
using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Web;
using Valghalla.External.Application.Modules.Web.Interfaces;

namespace Valghalla.External.Application.Modules.Unprotected.Queries
{
    public sealed record GetFrontPageQuery() : IQuery<Response>;

    internal class GetFrontPageQueryHandler : IQueryHandler<GetFrontPageQuery>
    {
        private readonly IWebPageQueryRepository webPageQueryRepository;

        public GetFrontPageQueryHandler(IWebPageQueryRepository webPageQueryRepository)
        {
            this.webPageQueryRepository = webPageQueryRepository;
        }

        public async Task<Response> Handle(GetFrontPageQuery request, CancellationToken cancellationToken)
        {
            var webPage = await webPageQueryRepository.GetWebPageAsync(Constants.WebPages.WebPageName_FrontPage, cancellationToken);

            var result = webPage != null ?
                JsonSerializer.Deserialize<FrontPage>(webPage.PageInfo) :
                new FrontPage();

            return Response.Ok(result);
        }
    }
}
