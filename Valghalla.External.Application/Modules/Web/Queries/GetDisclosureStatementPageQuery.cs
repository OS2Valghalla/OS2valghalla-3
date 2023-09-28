using System.Text.Json;
using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Web;
using Valghalla.External.Application.Modules.Web.Interfaces;

namespace Valghalla.External.Application.Modules.Web.Queries
{
    public sealed record GetDisclosureStatementPageQuery(): IQuery<Response>;

    internal class GetDisclosureStatementPageQueryHandler : IQueryHandler<GetDisclosureStatementPageQuery>
    {
        private readonly IWebPageQueryRepository webPageQueryRepository;

        public GetDisclosureStatementPageQueryHandler(IWebPageQueryRepository webPageQueryRepository)
        {
            this.webPageQueryRepository = webPageQueryRepository;
        }

        public async Task<Response> Handle(GetDisclosureStatementPageQuery request, CancellationToken cancellationToken)
        {
            var webPage = await webPageQueryRepository.GetWebPageAsync(Constants.WebPages.WebPageName_DisclosureStatementPage, cancellationToken);
            var result = webPage != null ?
                JsonSerializer.Deserialize<DisclosureStatementPage>(webPage.PageInfo) :
                new DisclosureStatementPage();

            return Response.Ok(result);
        }
    }
}
