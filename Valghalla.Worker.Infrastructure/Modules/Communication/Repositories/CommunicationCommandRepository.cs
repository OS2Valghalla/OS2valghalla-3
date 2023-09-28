using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Worker.Infrastructure.Modules.Communication.Repositories
{
    public interface ICommunicationCommandRepository
    {
        Task SetTaskAssignmentInvitationSentAsync(Guid taskAssignmentId, CancellationToken cancellationToken);
        Task SetTaskAssignmentRegistrationSentAsync(Guid taskAssignmentId, CancellationToken cancellationToken);
        Task SetTaskAssignmentCancellationSentAsync(Guid taskAssignmentId, CancellationToken cancellationToken);
    }

    internal class CommunicationCommandRepository : ICommunicationCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<TaskAssignmentEntity> taskAssignments;

        public CommunicationCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            taskAssignments = dataContext.Set<TaskAssignmentEntity>();
        }

        public async Task SetTaskAssignmentInvitationSentAsync(Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var entity = await taskAssignments
                .Where(i => i.Id == taskAssignmentId)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity != null && entity.ParticipantId.HasValue && !entity.Responsed && !entity.InvitationSent)
            {
                entity.InvitationSent = true;
                taskAssignments.Update(entity);

                await dataContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task SetTaskAssignmentRegistrationSentAsync(Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var entity = await taskAssignments
                .Where(i => i.Id == taskAssignmentId)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity != null && entity.ParticipantId.HasValue && entity.Responsed && entity.Accepted && !entity.RegistrationSent)
            {
                entity.RegistrationSent = true;
                taskAssignments.Update(entity);

                await dataContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task SetTaskAssignmentCancellationSentAsync(Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var entity = await taskAssignments
                .Where(i => i.Id == taskAssignmentId)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity != null && entity.ParticipantId.HasValue && !entity.Responsed && !entity.Accepted)
            {
                entity.ParticipantId = null;
                entity.InvitationCode = null;
                entity.InvitationSent = false;
                entity.RegistrationSent = false;
                taskAssignments.Update(entity);

                await dataContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
