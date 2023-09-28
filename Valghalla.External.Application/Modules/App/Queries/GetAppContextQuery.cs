using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.App.Interfaces;
using Valghalla.External.Application.Modules.App.Responses;

namespace Valghalla.External.Application.Modules.App.Queries
{
    public sealed record GetAppContextQuery() : IQuery<Response>;

    internal class GetAppContextQueryHandler : IQueryHandler<GetAppContextQuery>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IAppQueryRepository appQueryRepository;

        public GetAppContextQueryHandler(
            IUserContextProvider userContextProvider,
            IAppQueryRepository appQueryRepository)
        {
            this.userContextProvider = userContextProvider;
            this.appQueryRepository = appQueryRepository;
        }

        public async Task<Response> Handle(GetAppContextQuery request, CancellationToken cancellationToken)
        {
            var user = userContextProvider.CurrentUser;
            var webPage = await appQueryRepository.GetWebPageAsync(cancellationToken);
            var electionActivated = await appQueryRepository.CheckIfElectionIsActivatedAsync(cancellationToken);
            var isFAQPageActivated = await appQueryRepository.CheckIfFAQPageActivatedAsync(cancellationToken);

            var appContext = new AppContextResponse()
            {
                ElectionActivated = electionActivated,
                FAQPageActivated = isFAQPageActivated,
                User = user == null ? null : new UserResponse()
                {
                    Id = user.UserId,
                    RoleIds = user.RoleIds,
                    Name = user.Name,
                },
                WebPage = webPage,
            };

            return Response.Ok(appContext);
        }
    }
}
