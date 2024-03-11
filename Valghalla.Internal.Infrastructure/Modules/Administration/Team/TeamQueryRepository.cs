using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Team.Commands;
using Valghalla.Internal.Application.Modules.Administration.Team.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Team.Queries;
using Valghalla.Internal.Application.Modules.Administration.Team.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.Team
{
    internal class TeamQueryRepository : ITeamQueryRepository
    {
        private readonly IQueryable<TeamEntity> teams;
        private readonly IQueryable<TeamMemberEntity> teamMembers;
        private readonly IMapper mapper;

        public TeamQueryRepository(DataContext dataContext, IMapper mapper)
        {
            teams = dataContext.Set<TeamEntity>().AsNoTracking();
            teamMembers = dataContext.Set<TeamMemberEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<bool> CheckIfTeamExistsAsync(CreateTeamCommand command, CancellationToken cancellationToken)
        {
            var name = command.Name.Trim().ToLower();
            return await teams.AnyAsync(i => i.Name.ToLower() == name, cancellationToken);
        }

        public async Task<bool> CheckIfTeamExistsAsync(UpdateTeamCommand command, CancellationToken cancellationToken)
        {
            var name = command.Name.Trim().ToLower();
            return await teams.AnyAsync(i => i.Id != command.Id && i.Name.ToLower() == name, cancellationToken);
        }

        public async Task<bool> CheckIfTeamUsedInActiveElectionAsync(Guid id, CancellationToken cancellationToken)
        {
            return await teams.Include(x => x.WorkLocationTeams).ThenInclude(x => x.WorkLocation).ThenInclude(x => x.Elections).AnyAsync(x => x.Id == id && x.WorkLocationTeams.Any(t => t.WorkLocation.Elections.Any(e => e.Active)), cancellationToken);
        }

        public async Task<TeamDetailResponse?> GetTeamAsync(GetTeamQuery query, CancellationToken cancellationToken)
        {
            var entity = await teams.Include(t => t.TeamResponsibles).Include(x => x.WorkLocationTeams).ThenInclude(t => t.TaskAssignments).SingleOrDefaultAsync(i => i.Id == query.TeamId, cancellationToken);
            if (entity == null) return null;
            return mapper.Map<TeamDetailResponse>(entity);
        }

        public async Task<bool> CheckIfTeamHasTasksAsync(DeleteTeamCommand command, CancellationToken cancellationToken)
        {
            return await teams.Include(x => x.WorkLocationTeams).ThenInclude(x => x.TaskAssignments).AnyAsync(i => i.Id == command.Id && i.WorkLocationTeams.Any(t => t.TaskAssignments.Any()), cancellationToken);
        }

        public async Task<IList<ListTeamsItemResponse>> GetAllTeamsAsync(CancellationToken cancellationToken)
        {
            var entities = await teams.OrderBy(i => i.Name)
              .ToListAsync(cancellationToken);

            return entities.Select(mapper.Map<ListTeamsItemResponse>).ToList();
        }

        public async Task<bool> CheckIfTeamHasAbandonedParticipantsAsync(Guid id, CancellationToken cancellationToken)
        {
            var team = await teams
                .Include(i => i.TeamMembers)
                .Where(i => i.Id == id)
                .Select(i => new { i.Id, i.TeamMembers })
                .SingleAsync(cancellationToken);

            var participantIds = team.TeamMembers.Select(i => i.ParticipantId).ToArray();

            var relatedTeamMembers = await teamMembers
                .Where(i => participantIds.Contains(i.ParticipantId))
                .ToArrayAsync(cancellationToken);

            var invalid = participantIds.Any(participantId => !relatedTeamMembers.Any(i => i.ParticipantId == participantId && i.TeamId != id));

            return invalid;
        }
    }
}
