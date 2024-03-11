using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Participant.Commands;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Participant
{
    internal class ParticipantGdprCommandRepository : IParticipantGdprCommandRepository
    {
        private readonly DataContext dataContext;

        private readonly DbSet<ParticipantEntity> participants;
        private readonly DbSet<UserEntity> users;
        private readonly DbSet<ParticipantEventLogEntity> participantEventLogs;
        private readonly DbSet<TeamMemberEntity> teamMembers;
        private readonly DbSet<TeamResponsibleEntity> teamResponsibles;
        private readonly DbSet<SpecialDietParticipantEntity> specialDietParticipants;
        private readonly DbSet<TaskAssignmentEntity> taskAssignments;
        private readonly DbSet<WorkLocationResponsibleEntity> workLocationResponsibles;

        public ParticipantGdprCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;

            participants = dataContext.Set<ParticipantEntity>();
            users = dataContext.Set<UserEntity>();
            participantEventLogs = dataContext.Set<ParticipantEventLogEntity>();
            teamMembers = dataContext.Set<TeamMemberEntity>();
            teamResponsibles = dataContext.Set<TeamResponsibleEntity>();
            specialDietParticipants = dataContext.Set<SpecialDietParticipantEntity>();
            taskAssignments = dataContext.Set<TaskAssignmentEntity>();
            workLocationResponsibles = dataContext.Set<WorkLocationResponsibleEntity>();
        }

        public async Task<string> DeleteParticipantAsync(DeleteParticipantCommand command, CancellationToken cancellationToken)
        {
            var entity = await participants
                .Include(i => i.User)
                .Include(i => i.ParticipantEventLogs)
                .Include(i => i.SpecialDietParticipants)
                .Include(i => i.WorkLocationResponsibles)
                .Include(i => i.TeamResponsibles)
                .Include(i => i.TeamMembers)
                .Include(i => i.TaskAssignments)
                .Where(i => i.Id == command.Id)
                .SingleAsync(cancellationToken);

            var cprNumber = entity.Cpr;

            participantEventLogs.RemoveRange(entity.ParticipantEventLogs);
            specialDietParticipants.RemoveRange(entity.SpecialDietParticipants);
            workLocationResponsibles.RemoveRange(entity.WorkLocationResponsibles);
            teamResponsibles.RemoveRange(entity.TeamResponsibles);
            teamMembers.RemoveRange(entity.TeamMembers);
            users.Remove(entity.User);

            foreach (var task in entity.TaskAssignments)
            {
                task.ParticipantId = null;
                task.Responsed = false;
                task.Accepted = false;
                task.InvitationCode = null;
                task.InvitationSent = false;
                task.RegistrationSent = false;
                task.InvitationDate = null;
                task.InvitationReminderDate = null;
                task.TaskReminderDate = null;
            }

            taskAssignments.UpdateRange(entity.TaskAssignments);

            await dataContext.SaveChangesAsync(cancellationToken);

            return cprNumber;
        }

        public async Task<IEnumerable<string>> DeleteParticipantsAsync(BulkDeleteParticipantsCommand command, CancellationToken cancellationToken)
        {
            var entities = await participants
                .Include(i => i.User)
                .Include(i => i.ParticipantEventLogs)
                .Include(i => i.SpecialDietParticipants)
                .Include(i => i.WorkLocationResponsibles)
                .Include(i => i.TeamResponsibles)
                .Include(i => i.TeamMembers)
                .Include(i => i.TaskAssignments)
                .Where(i => command.ParticipantIds.Contains(i.Id))
                .ToArrayAsync(cancellationToken);

            var cprNumbers = entities.Select(i => i.Cpr).ToArray();

            participantEventLogs.RemoveRange(entities.SelectMany(i => i.ParticipantEventLogs));
            specialDietParticipants.RemoveRange(entities.SelectMany(i => i.SpecialDietParticipants));
            workLocationResponsibles.RemoveRange(entities.SelectMany(i => i.WorkLocationResponsibles));
            teamResponsibles.RemoveRange(entities.SelectMany(i => i.TeamResponsibles));
            teamMembers.RemoveRange(entities.SelectMany(i => i.TeamMembers));
            users.RemoveRange(entities.Select(i => i.User));

            var taskAssignmentEntities = entities.SelectMany(i => i.TaskAssignments);

            foreach (var task in taskAssignmentEntities)
            {
                task.ParticipantId = null;
                task.Responsed = false;
                task.Accepted = false;
                task.InvitationCode = null;
                task.InvitationSent = false;
                task.RegistrationSent = false;
                task.InvitationDate = null;
                task.InvitationReminderDate = null;
                task.TaskReminderDate = null;
            }

            taskAssignments.UpdateRange(taskAssignmentEntities);

            await dataContext.SaveChangesAsync(cancellationToken);

            return cprNumbers;
        }
    }
}
