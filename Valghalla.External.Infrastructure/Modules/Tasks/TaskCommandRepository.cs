using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.Tasks.Commands;
using Valghalla.External.Application.Modules.Tasks.Interfaces;

namespace Valghalla.External.Infrastructure.Modules.Tasks
{
    internal class TaskCommandRepository : ITaskCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<TaskAssignmentEntity> taskAssignments;
        private readonly DbSet<RejectedTaskAssignmentEntity> rejectedTaskAssignments;

        public TaskCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            taskAssignments = dataContext.Set<TaskAssignmentEntity>();
            rejectedTaskAssignments = dataContext.Set<RejectedTaskAssignmentEntity>();
        }

        public async Task AcceptTaskAsync(AcceptTaskCommand command, Guid participantId, CancellationToken cancellationToken)
        {
            var queryable = taskAssignments
                .Include(i => i.TaskType)
                .Where(i =>
                    i.HashValue == command.HashValue &&
                    i.Responsed == false);

            if (command.InvitationCode.HasValue)
            {
                queryable = queryable.Where(i =>
                    i.ParticipantId == participantId &&
                    i.InvitationCode == command.InvitationCode);
            }
            else
            {
                queryable = queryable.Where(i => i.ParticipantId == null);
            }

            var entity = await queryable.FirstAsync(cancellationToken);

            entity.ParticipantId = participantId;
            entity.Responsed = true;
            entity.Accepted = true;
            entity.InvitationCode = null;
            entity.InvitationSent = true;
            entity.RegistrationSent = false;

            taskAssignments.Update(entity);

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task RejectTaskAsync(RejectTaskCommand command, Guid participantId, CancellationToken cancellationToken)
        {
            var queryable = taskAssignments
                .Include(i => i.TaskType)
                .Where(i =>
                    i.HashValue == command.HashValue &&
                    i.Responsed == false);

            if (command.InvitationCode.HasValue)
            {
                queryable = queryable.Where(i =>
                    i.ParticipantId == participantId &&
                    i.InvitationCode == command.InvitationCode);
            }
            else
            {
                queryable = queryable.Where(i => i.ParticipantId == participantId);
            }

            var entity = await queryable.SingleAsync(cancellationToken);

            var rejectedTaskAssignmentEntity = new RejectedTaskAssignmentEntity()
            {
                Id = Guid.NewGuid(),
                ElectionId = entity.ElectionId,
                WorkLocationId = entity.WorkLocationId,
                TeamId = entity.TeamId,
                TaskTypeId = entity.TaskTypeId,
                TaskDate = entity.TaskDate,
                ParticipantId = entity.ParticipantId!.Value
            };

            entity.Responsed = false;
            entity.Accepted = false;
            entity.ParticipantId = null;
            entity.InvitationCode = null;
            entity.InvitationSent = false;
            entity.RegistrationSent = false;
            entity.InvitationDate = null;
            entity.InvitationReminderDate = null;
            entity.TaskReminderDate = null;

            taskAssignments.Update(entity);
            await rejectedTaskAssignments.AddAsync(rejectedTaskAssignmentEntity, cancellationToken);

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UnregisterTaskAsync(UnregisterTaskCommand command, Guid participantId, CancellationToken cancellationToken)
        {
            var entity = await taskAssignments
                .Where(i =>
                    i.Id == command.TaskAssignmentId &&
                    i.ParticipantId == participantId)
                .SingleAsync(cancellationToken);

            var rejectedTaskAssignmentEntity = new RejectedTaskAssignmentEntity()
            {
                Id = Guid.NewGuid(),
                ElectionId = entity.ElectionId,
                WorkLocationId = entity.WorkLocationId,
                TeamId = entity.TeamId,
                TaskTypeId = entity.TaskTypeId,
                TaskDate = entity.TaskDate,
                ParticipantId = entity.ParticipantId!.Value
            };

            entity.Responsed = false;
            entity.Accepted = false;
            entity.ParticipantId = null;
            entity.InvitationCode = null;
            entity.InvitationSent = false;
            entity.RegistrationSent = false;
            entity.InvitationDate = null;
            entity.InvitationReminderDate = null;
            entity.TaskReminderDate = null;

            taskAssignments.Update(entity);
            await rejectedTaskAssignments.AddAsync(rejectedTaskAssignmentEntity, cancellationToken);

            await dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
