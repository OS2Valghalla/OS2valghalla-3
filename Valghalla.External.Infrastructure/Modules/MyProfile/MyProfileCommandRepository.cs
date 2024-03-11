using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.MyProfile.Commands;
using Valghalla.External.Application.Modules.MyProfile.Interfaces;

namespace Valghalla.External.Infrastructure.Modules.MyProfile
{
    internal class MyProfileCommandRepository : IMyProfileCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<ParticipantEntity> participants;
        private readonly DbSet<ParticipantEventLogEntity> participantEventLogs;
        private readonly DbSet<SpecialDietParticipantEntity> specialDietParticipants;
        private readonly DbSet<WorkLocationResponsibleEntity> workLocationResponsibles;
        private readonly DbSet<TeamResponsibleEntity> teamResponsibles;
        private readonly DbSet<TeamMemberEntity> teamMembers;
        private readonly DbSet<UserEntity> users;
        private readonly DbSet<TaskAssignmentEntity> taskAssignments;

        public MyProfileCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            participants = dataContext.Set<ParticipantEntity>();
            participantEventLogs = dataContext.Set<ParticipantEventLogEntity>();
            specialDietParticipants = dataContext.Set<SpecialDietParticipantEntity>();
            workLocationResponsibles = dataContext.Set<WorkLocationResponsibleEntity>();
            teamResponsibles = dataContext.Set<TeamResponsibleEntity>();
            teamMembers = dataContext.Set<TeamMemberEntity>();
            users = dataContext.Set<UserEntity>();
            taskAssignments = dataContext.Set<TaskAssignmentEntity>();
        }

        public async Task UpdateMyProfileAsync(Guid participantId, UpdateMyProfileCommand command, CancellationToken cancellationToken)
        {
            var participantEntity = await participants
                .Where(i => i.Id == participantId)
                .SingleAsync(cancellationToken);

            var specialDietParticipantEntities = await specialDietParticipants
                .Where(i => i.ParticipantId == participantEntity.Id)
                .ToArrayAsync(cancellationToken);

            participantEntity.MobileNumber = command.MobileNumber;
            participantEntity.Email = command.Email;

            var specialDietParticipantEntitiesToAdd = command.SpecialDietIds
                .Where(specialDietId => !specialDietParticipantEntities.Any(i => i.SpecialDietId == specialDietId))
                .Select(specialDietId => new SpecialDietParticipantEntity()
                {
                    ParticipantId = participantEntity.Id,
                    SpecialDietId = specialDietId,
                });

            var specialDietParticipantEntitiesToRemove = specialDietParticipantEntities
                .Where(i => !command.SpecialDietIds.Any(specialDietId => specialDietId == i.SpecialDietId));

            participants.Update(participantEntity);

            if (specialDietParticipantEntitiesToRemove.Any())
            {
                specialDietParticipants.RemoveRange(specialDietParticipantEntitiesToRemove);
            }

            if (specialDietParticipantEntitiesToAdd.Any())
            {
                await specialDietParticipants.AddRangeAsync(specialDietParticipantEntitiesToAdd, cancellationToken);
            }

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteMyProfileAsync(Guid participantId, CancellationToken cancellationToken)
        {
            var participantEntity = await participants
                .Include(i => i.User)
                .Include(i => i.ParticipantEventLogs)
                .Include(i => i.SpecialDietParticipants)
                .Include(i => i.WorkLocationResponsibles)
                .Include(i => i.TeamResponsibles)
                .Include(i => i.TeamMembers)
                .Include(i => i.TaskAssignments)
                .Where(i => i.Id == participantId)
                .SingleAsync(cancellationToken);

            participantEventLogs.RemoveRange(participantEntity.ParticipantEventLogs);
            specialDietParticipants.RemoveRange(participantEntity.SpecialDietParticipants);
            workLocationResponsibles.RemoveRange(participantEntity.WorkLocationResponsibles);
            teamResponsibles.RemoveRange(participantEntity.TeamResponsibles);
            teamMembers.RemoveRange(participantEntity.TeamMembers);
            users.RemoveRange(participantEntity.User);

            foreach (var task in participantEntity.TaskAssignments)
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

            taskAssignments.UpdateRange(participantEntity.TaskAssignments);

            await dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
