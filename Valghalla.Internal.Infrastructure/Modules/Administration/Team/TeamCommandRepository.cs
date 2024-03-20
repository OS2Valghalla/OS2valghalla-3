using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Team.Commands;
using Valghalla.Internal.Application.Modules.Administration.Team.Interfaces;
using Valghalla.Internal.Application.Modules.Team.Commands;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.Team
{
    internal class TeamCommandRepository : ITeamCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<TeamEntity> teams;
        private readonly DbSet<TeamLinkEntity> teamLinks;
        private readonly DbSet<TeamResponsibleEntity> teamResponsibles;
        private readonly DbSet<TeamMemberEntity> teamMembers;
        private readonly DbSet<WorkLocationTeamEntity> workLocationTeams;
        private readonly DbSet<TaskAssignmentEntity> taskAssignments;

        public TeamCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            teams = dataContext.Set<TeamEntity>();
            teamLinks = dataContext.Set<TeamLinkEntity>();
            teamResponsibles = dataContext.Set<TeamResponsibleEntity>();
            teamMembers = dataContext.Set<TeamMemberEntity>();
            workLocationTeams = dataContext.Set<WorkLocationTeamEntity>();
            taskAssignments = dataContext.Set<TaskAssignmentEntity>();
        }
        public async Task<string> CreateTeamLinkAsync(CreateTeamLinkCommand command, CancellationToken cancellationToken)
        {
            var existingEntity = await teamLinks.FirstOrDefaultAsync(x => x.Value == command.Value);
            if (existingEntity != null)
            {
                return existingEntity.HashValue;
            }

            var entity = new TeamLinkEntity()
            {
                Id = Guid.NewGuid(),
                HashValue = command.HashValue,
                Value = command.Value
            };

            await teamLinks.AddAsync(entity, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);

            return entity.HashValue;
        }

        public async Task<Guid> CreateTeamAsync(CreateTeamCommand command, CancellationToken cancellationToken)
        {
            var teamId = Guid.NewGuid();
            var entity = new TeamEntity()
            {
                Id = teamId,
                Name = command.Name,
                ShortName = command.ShortName,
                Description = command.Description
            };

            var teamResponsibleEntities = command.ResponsibleIds.Select(participantId => new TeamResponsibleEntity()
            {
                TeamId = teamId,
                ParticipantId = participantId
            });

            await teams.AddAsync(entity, cancellationToken);
            await teamResponsibles.AddRangeAsync(teamResponsibleEntities, cancellationToken);

            await dataContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task<(IEnumerable<Guid>, string)> DeleteTeamAsync(DeleteTeamCommand command, CancellationToken cancellationToken)
        {
            var entity = await teams
                    .Include(i => i.TeamResponsibles)
                    .Include(i => i.TeamMembers)
                    .Include(i => i.WorkLocationTeams)
                    .SingleAsync(i => i.Id == command.Id, cancellationToken);

            var teamName = entity.Name;

            var teamResponsibleIds = entity.TeamResponsibles
                .Select(i => i.ParticipantId)
                .ToArray();

            var existingTaskAssignments = await taskAssignments.Where(x => x.TeamId == command.Id).ToListAsync(cancellationToken);

            taskAssignments.RemoveRange(existingTaskAssignments);
            teamResponsibles.RemoveRange(entity.TeamResponsibles);
            teamMembers.RemoveRange(entity.TeamMembers);
            workLocationTeams.RemoveRange(entity.WorkLocationTeams);
            teams.Remove(entity);

            await dataContext.SaveChangesAsync(cancellationToken);

            return (teamResponsibleIds, teamName);
        }

        public async Task<(IEnumerable<Guid>, IEnumerable<Guid>)> UpdateTeamAsync(UpdateTeamCommand command, CancellationToken cancellationToken)
        {
            var entity = await teams.SingleAsync(i => i.Id == command.Id, cancellationToken);
            var existingResponsibles = await teamResponsibles.Where(x => x.TeamId == command.Id).ToListAsync(cancellationToken);

            entity.Description = command.Description;
            entity.Name = command.Name;
            entity.ShortName = command.ShortName;

            teams.Update(entity);

            var teamResponsibleEntitiesToDelete = existingResponsibles
                .Where(r => !command.ResponsibleIds.Contains(r.ParticipantId))
                .ToArray();

            var teamResponsibleEntitiesToAdd = command.ResponsibleIds
                .Where(participantId => !existingResponsibles.Any(i => i.ParticipantId == participantId))
                .Select(participantId => new TeamResponsibleEntity
                {
                    TeamId = command.Id,
                    ParticipantId = participantId
                })
                .ToArray();

            var teamResponsibleIdsToDelete = teamResponsibleEntitiesToDelete.Select(i => i.ParticipantId);
            var teamResponsibleIdsToAdd = teamResponsibleEntitiesToAdd.Select(i => i.ParticipantId);

            teamResponsibles.RemoveRange(teamResponsibleEntitiesToDelete);
            await teamResponsibles.AddRangeAsync(teamResponsibleEntitiesToAdd, cancellationToken);

            await dataContext.SaveChangesAsync(cancellationToken);

            return (teamResponsibleIdsToAdd, teamResponsibleIdsToDelete);
        }
    }
}
