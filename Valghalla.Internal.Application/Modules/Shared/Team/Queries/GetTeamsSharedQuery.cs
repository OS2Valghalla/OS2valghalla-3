using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Shared.Team.Interfaces;

namespace Valghalla.Internal.Application.Modules.Shared.Team.Queries
{
    public sealed record GetTeamsSharedQuery(): IQuery<Response>;

    internal class GetTeamsSharedQueryHandler : IQueryHandler<GetTeamsSharedQuery>
    {
        private readonly ITeamSharedQueryRepository teamSharedQueryRepository;

        public GetTeamsSharedQueryHandler(ITeamSharedQueryRepository teamSharedQueryRepository)
        {
            this.teamSharedQueryRepository = teamSharedQueryRepository;
        }

        public async Task<Response> Handle(GetTeamsSharedQuery query, CancellationToken cancellationToken)
        {
            var result = await teamSharedQueryRepository.GetTeamsAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
