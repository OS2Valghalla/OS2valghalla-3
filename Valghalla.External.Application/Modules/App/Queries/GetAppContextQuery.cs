using Valghalla.Application.Abstractions.Messaging;
using Valghalla.External.Application.Modules.App.Interfaces;
using Valghalla.External.Application.Modules.App.Responses;

namespace Valghalla.External.Application.Modules.App.Queries
{
    public sealed record GetAppContextQuery() : IQuery<Response>;

    internal class GetAppContextQueryHandler : IQueryHandler<GetAppContextQuery>
    {
        private readonly IAppQueryRepository appQueryRepository;

        public GetAppContextQueryHandler(IAppQueryRepository appQueryRepository)
        {
            this.appQueryRepository = appQueryRepository;
        }

        public async Task<Response> Handle(GetAppContextQuery request, CancellationToken cancellationToken)
        {
            var webPage = await appQueryRepository.GetWebPageAsync(cancellationToken);
            var electionActivated = await appQueryRepository.CheckIfElectionIsActivatedAsync(cancellationToken);
            var isFAQPageActivated = await appQueryRepository.CheckIfFAQPageActivatedAsync(cancellationToken);

            var appContext = new AppContextResponse()
            {
                ElectionActivated = electionActivated,
                FAQPageActivated = isFAQPageActivated,
                WebPage = webPage,
            };

            return Response.Ok(appContext);
        }
    }
}
