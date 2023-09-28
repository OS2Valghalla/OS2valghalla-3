using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.Team.Interfaces;
using Valghalla.External.Application.Modules.Team.Responses;

namespace Valghalla.External.Infrastructure.Modules.Team
{
    internal class TeamQueryRepository: ITeamQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<ParticipantEntity> participants;
        private readonly IQueryable<TeamEntity> teams;
        private readonly IQueryable<TaskAssignmentEntity> tasks;

        public TeamQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            participants = dataContext.Set<ParticipantEntity>().AsNoTracking();
            teams = dataContext.Set<TeamEntity>().AsNoTracking();
            tasks = dataContext.Set<TaskAssignmentEntity>().AsNoTracking();
        }

        public async Task<IList<TeamResponse>> GetMyTeamsAsync(Guid participantId, CancellationToken cancellationToken)
        {
            var participant = await participants
                .Where(i => i.Id == participantId)
                .SingleAsync(cancellationToken);

            var teamEntities = await teams.Include(i => i.TeamResponsibles)
                .Where(i => i.TeamResponsibles.Any(x => x.ParticipantId == participant.Id))
                .OrderBy(i => i.Name).ToListAsync(cancellationToken);

            return teamEntities.Select(mapper.Map<TeamResponse>).ToList();
        }

        public async Task<IList<TeamMemberResponse>> GetTeamMembersAsync(Guid teamId, Guid participantId, CancellationToken cancellationToken)
        {
            var responsible = await participants
                .Where(i => i.Id == participantId)
                .SingleAsync(cancellationToken);

            var teamEntities = await teams.Include(i => i.TeamResponsibles)
                .Where(i => i.TeamResponsibles.Any(x => x.ParticipantId == responsible.Id))
                .OrderBy(i => i.Name).ToListAsync(cancellationToken);

            if (!teamEntities.Any(t => t.Id == teamId)) return new List<TeamMemberResponse>();

            var participantEntities = await participants.Include(i => i.TeamMembers)
                .Where(i => i.TeamMembers.Any(i => i.TeamId == teamId) && i.Id != responsible.Id)
                .OrderBy(i => i.FirstName).ThenBy(i => i.LastName)
                .ToListAsync(cancellationToken);

            var teamMembers = participantEntities.Select(mapper.Map<TeamMemberResponse>).ToList();
            foreach(var teamMember in teamMembers)
            {
                teamMember.AssignedTasksCount = tasks.Count(t => t.TeamId == teamId && t.ParticipantId == teamMember.Id && t.Accepted && t.TaskDate >= DateTime.Today);
                teamMember.CanBeRemoved = !tasks.Include(i => i.Election).Any(t => t.TeamId == teamId && t.ParticipantId == teamMember.Id && t.Accepted && (t.TaskDate < DateTime.Today || t.Election.ElectionEndDate < DateTime.Today));
            }

            return teamMembers;
        }
    }
}
