using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Worker.Infrastructure.Modules.Tasks.Repositories
{
    public interface ITaskAssignmentCommandRepository
    {
        Task UnassignTaskAssignmentsAsync(IEnumerable<Guid> taskAssignmentIds, CancellationToken cancellationToken);
    }

    internal class TaskAssignmentCommandRepository : ITaskAssignmentCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<TaskAssignmentEntity> taskAssignments;

        public TaskAssignmentCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            taskAssignments = dataContext.Set<TaskAssignmentEntity>();
        }

        public async Task UnassignTaskAssignmentsAsync(IEnumerable<Guid> taskAssignmentIds, CancellationToken cancellationToken)
        {
            var entities = await taskAssignments
                .Where(i => taskAssignmentIds.Contains(i.Id))
                .ToArrayAsync(cancellationToken);

            foreach (var entity in entities)
            {
                entity.ParticipantId = null;
                entity.Accepted = false;
                entity.Responsed = false;
                entity.InvitationCode = null;
                entity.InvitationSent = false;
                entity.RegistrationSent = false;
                entity.InvitationDate = null;
                entity.InvitationReminderDate = null;
                entity.TaskReminderDate = null;
            }

            taskAssignments.UpdateRange(entities);

            await dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
