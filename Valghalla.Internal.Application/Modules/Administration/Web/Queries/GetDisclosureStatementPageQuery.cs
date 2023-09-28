using System.Text.Json;
using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Web;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Web.Queries
{
    public sealed record GetDisclosureStatementPageQuery() : IQuery<Response>;
    internal sealed class GetDisclosureStatementPageQueryHandler : IQueryHandler<GetDisclosureStatementPageQuery>
    {
        private readonly IWebPageQueryRepository webPageQueryRepository;

        public GetDisclosureStatementPageQueryHandler(IWebPageQueryRepository webPageQueryRepository)
        {
            this.webPageQueryRepository = webPageQueryRepository;
        }

        public async Task<Response> Handle(GetDisclosureStatementPageQuery request, CancellationToken cancellationToken)
        {
            var result = await webPageQueryRepository.GetWebPageAsync(Constants.WebPages.WebPageName_DisclosureStatementPage, cancellationToken);

            var pageResponse = new DisclosureStatementPage()
            {
                PageContent = string.Empty
            };

            if (result != null)
            {
                pageResponse = JsonSerializer.Deserialize<DisclosureStatementPage>(result.PageInfo);
            }

            return Response.Ok(pageResponse);
        }
    }
}
