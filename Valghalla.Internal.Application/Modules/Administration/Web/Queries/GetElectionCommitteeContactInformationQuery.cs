using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Web;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Web.Queries
{
    public sealed record GetElectionCommitteeContactInformationQuery() : IQuery<Response>;
    internal sealed class GetElectionCommitteeContactInformationQueryHandler : IQueryHandler<GetElectionCommitteeContactInformationQuery>
    {
        private readonly IElectionCommitteeContactInformationQueryRepository webPageQueryRepository;

        public GetElectionCommitteeContactInformationQueryHandler(IElectionCommitteeContactInformationQueryRepository webPageQueryRepository)
        {
            this.webPageQueryRepository = webPageQueryRepository;
        }

        public async Task<Response> Handle(GetElectionCommitteeContactInformationQuery request, CancellationToken cancellationToken)
        {
            var result = await webPageQueryRepository.GetWebPageAsync(Constants.WebPages.WebPageName_ElectionCommitteeContactInformation, cancellationToken);

            if (result == null)
            {
                result = new ElectionCommitteeContactInformationPage();
            }

            return Response.Ok(result);
        }
    }
}
