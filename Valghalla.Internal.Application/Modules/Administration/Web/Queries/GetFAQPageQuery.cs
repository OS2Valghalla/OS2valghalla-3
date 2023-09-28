using System.Text.Json;
using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Web;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Web.Queries
{
    public sealed record GetFAQPageQuery() : IQuery<Response>;
    internal sealed class GetFAQPageQueryHandler : IQueryHandler<GetFAQPageQuery>
    {
        private readonly IWebPageQueryRepository webPageQueryRepository;

        public GetFAQPageQueryHandler(IWebPageQueryRepository webPageQueryRepository)
        {
            this.webPageQueryRepository = webPageQueryRepository;
        }

        public async Task<Response> Handle(GetFAQPageQuery request, CancellationToken cancellationToken)
        {
            var result = await webPageQueryRepository.GetWebPageAsync(Constants.WebPages.WebPageName_FAQPage, cancellationToken);

            var pageResponse = new FAQPage()
            {
                PageContent = string.Empty,
                IsActivated = false
            };

            if (result != null)
            {
                pageResponse = JsonSerializer.Deserialize<FAQPage>(result.PageInfo);
            }

            return Response.Ok(pageResponse);
        }
    }
}
