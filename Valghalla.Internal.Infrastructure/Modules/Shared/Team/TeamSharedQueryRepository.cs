using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Shared.Team.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Team.Queries;
using Valghalla.Internal.Application.Modules.Shared.Team.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Shared.Team
{
    internal class TeamSharedQueryRepository : ITeamSharedQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<TeamEntity> teams;

        public TeamSharedQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            teams = dataContext.Set<TeamEntity>().AsNoTracking();
        }

        public async Task<IEnumerable<TeamSharedResponse>> GetTeamsAsync(GetTeamsSharedQuery query, CancellationToken cancellationToken)
        {
            var entities = await teams
                .OrderBy(i => i.Name)
                .ToArrayAsync(cancellationToken);

            return entities.Select(mapper.Map<TeamSharedResponse>);
        }
    }
}
