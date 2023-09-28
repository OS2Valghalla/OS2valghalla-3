using System.Text.Json;
using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Web;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Web.Queries
{
    public sealed record GetFrontPageQuery() : IQuery<Response>;
    internal sealed class GetFrontPageQueryHandler : IQueryHandler<GetFrontPageQuery>
    {
        private readonly IWebPageQueryRepository webPageQueryRepository;

        public GetFrontPageQueryHandler(IWebPageQueryRepository webPageQueryRepository)
        {
            this.webPageQueryRepository = webPageQueryRepository;
        }

        public async Task<Response> Handle(GetFrontPageQuery request, CancellationToken cancellationToken)
        {
            var result = await webPageQueryRepository.GetWebPageAsync(Constants.WebPages.WebPageName_FrontPage, cancellationToken);

            var pageResponse = new FrontPage()
            {
                PageContent = string.Empty,
                Title = string.Empty
            };

            if (result != null)
            {
                pageResponse = JsonSerializer.Deserialize<FrontPage>(result.PageInfo);
            }

            return Response.Ok(pageResponse);
        }
    }
}
