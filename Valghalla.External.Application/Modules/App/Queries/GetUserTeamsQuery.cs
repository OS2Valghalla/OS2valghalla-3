using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.App.Interfaces;

namespace Valghalla.External.Application.Modules.App.Queries
{
    public sealed record GetUserTeamsQuery() : IQuery<Response>;

    internal class GetUserTeamsQueryHandler : IQueryHandler<GetUserTeamsQuery>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IAppQueryRepository appQueryRepository;

        public GetUserTeamsQueryHandler(IUserContextProvider userContextProvider, IAppQueryRepository appQueryRepository)
        {
            this.userContextProvider = userContextProvider;
            this.appQueryRepository = appQueryRepository;
        }

        public async Task<Response> Handle(GetUserTeamsQuery query, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;
            var items = await appQueryRepository.GetUserTeamsAsync(participantId, cancellationToken);
            return Response.Ok(items);
        }
    }
}
