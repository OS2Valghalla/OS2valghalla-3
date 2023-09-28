using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.Team.Commands;
using Valghalla.External.Application.Modules.Team.Interfaces;

namespace Valghalla.External.Infrastructure.Modules.Team
{
    internal class TeamCommandRepository: ITeamCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<ParticipantEntity> participants;
        private readonly DbSet<TeamEntity> teams;
        private readonly DbSet<TeamMemberEntity> teamMembers;
        private readonly DbSet<TaskAssignmentEntity> taskAssignments;
        private readonly DbSet<RejectedTaskAssignmentEntity> rejectedTaskAssignments;
        private readonly DbSet<TeamLinkEntity> teamLinks;
        private readonly DbSet<UserEntity> users;
        private readonly DbSet<ParticipantEventLogEntity> participantEventLogs;
        private readonly DbSet<SpecialDietParticipantEntity> specialDietParticipants;
        private readonly DbSet<WorkLocationResponsibleEntity> workLocationResponsibles;
        private readonly DbSet<TeamResponsibleEntity> teamResponsibles;

        public TeamCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            teams = dataContext.Set<TeamEntity>();
            participants = dataContext.Set<ParticipantEntity>();            
            teamMembers = dataContext.Set<TeamMemberEntity>();
            taskAssignments = dataContext.Set<TaskAssignmentEntity>();
            rejectedTaskAssignments = dataContext.Set<RejectedTaskAssignmentEntity>();
            teamLinks = dataContext.Set<TeamLinkEntity>();
            users = dataContext.Set<UserEntity>();
            participantEventLogs = dataContext.Set<ParticipantEventLogEntity>();
            specialDietParticipants = dataContext.Set<SpecialDietParticipantEntity>();
            workLocationResponsibles = dataContext.Set<WorkLocationResponsibleEntity>();
            teamResponsibles = dataContext.Set<TeamResponsibleEntity>();
        }

        public async Task RemoveTeamMemberAsync(Guid teamId, Guid participantId, Guid teamResponsibleId, CancellationToken cancellationToken)
        {
            var responsible = await participants
                .Where(i => i.Id == teamResponsibleId)
                .SingleAsync(cancellationToken);

            var teamMemberEntities = await teamMembers.Where(i => i.ParticipantId == participantId).ToListAsync(cancellationToken);

            if (!teamMemberEntities.Any(i => i.TeamId == teamId)) return;

            var teamEntities = await teams.Include(i => i.TeamResponsibles)
                .Where(i => i.TeamResponsibles.Any(x => x.ParticipantId == responsible.Id))
                .OrderBy(i => i.Name).ToListAsync(cancellationToken);            

            if (!teamEntities.Any(t => t.Id == teamId)) return;

            var tasks = await taskAssignments.Include(i => i.Election)
                .Where(i => i.TeamId == teamId && i.ParticipantId == participantId)
                .ToListAsync(cancellationToken);

            var canBeRemoved = !tasks.Any(t => t.TeamId == teamId && t.ParticipantId == participantId && t.Accepted && (t.TaskDate < DateTime.Today || t.Election.ElectionEndDate < DateTime.Today));
            if (!canBeRemoved) return;


            foreach (var task in tasks)
            {
                if (teamMemberEntities.Count() > 1)
                {
                    var rejectedTaskAssignmentEntity = new RejectedTaskAssignmentEntity()
                    {
                        Id = Guid.NewGuid(),
                        ElectionId = task.ElectionId,
                        WorkLocationId = task.WorkLocationId,
                        TeamId = task.TeamId,
                        TaskTypeId = task.TaskTypeId,
                        TaskDate = task.TaskDate,
                        ParticipantId = participantId
                    };
                    await rejectedTaskAssignments.AddAsync(rejectedTaskAssignmentEntity, cancellationToken);
                }

                task.Accepted = false;
                task.Responsed = false;
                task.ParticipantId = null;
                task.InvitationCode = null;
                task.InvitationSent = false;
                task.RegistrationSent = false;
            }

            taskAssignments.UpdateRange(tasks);

            teamMembers.Remove(teamMemberEntities.First(i => i.TeamId == teamId));

            if (teamMemberEntities.Count() == 1)
            {
                var participantEntity = await participants
                    .Include(i => i.User)
                    .Include(i => i.ParticipantEventLogs)
                    .Include(i => i.SpecialDietParticipants)
                    .Include(i => i.WorkLocationResponsibles)
                    .Include(i => i.TeamResponsibles)
                    .Where(i => i.Id == participantId)
                    .SingleAsync(cancellationToken);

                participantEventLogs.RemoveRange(participantEntity.ParticipantEventLogs);
                specialDietParticipants.RemoveRange(participantEntity.SpecialDietParticipants);
                workLocationResponsibles.RemoveRange(participantEntity.WorkLocationResponsibles);
                teamResponsibles.RemoveRange(participantEntity.TeamResponsibles);
                users.Remove(participantEntity.User);
            }

            await dataContext.SaveChangesAsync(cancellationToken);
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
    }
}
