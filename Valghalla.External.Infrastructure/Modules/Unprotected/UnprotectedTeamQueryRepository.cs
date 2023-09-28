using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.Team.Responses;
using Valghalla.External.Application.Modules.Unprotected.Interfaces;
using Valghalla.External.Application.Modules.Unprotected.Queries;

namespace Valghalla.External.Infrastructure.Modules.Unprotected
{
    internal class UnprotectedTeamQueryRepository: IUnprotectedTeamQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<TeamEntity> teams;
        private readonly IQueryable<TeamLinkEntity> teamLinks;

        public UnprotectedTeamQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            teams = dataContext.Set<TeamEntity>().AsNoTracking();
            teamLinks = dataContext.Set<TeamLinkEntity>().AsNoTracking();
        }

        public async Task<TeamResponse> GetTeamByTeamLinkAsync(GetTeamByTeamLinkQuery query, CancellationToken cancellationToken)
        {
            var teamLink = await teamLinks.FirstOrDefaultAsync(x => x.HashValue == query.HashValue);
            if (teamLink == null)
            {
                return null;
            }

            var team = await teams
               .FirstOrDefaultAsync(i => i.Id == new Guid(teamLink.Value!), cancellationToken);

            if (team == null) { return null; }

            return mapper.Map<TeamResponse>(team);
        }
    }
}
