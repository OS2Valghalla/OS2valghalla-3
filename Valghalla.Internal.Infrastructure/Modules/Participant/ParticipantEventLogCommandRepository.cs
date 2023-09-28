using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Participant.Commands;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Participant
{
    internal class ParticipantEventLogCommandRepository : IParticipantEventLogCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<ParticipantEventLogEntity> participantEventLogs;

        public ParticipantEventLogCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            participantEventLogs = dataContext.Set<ParticipantEventLogEntity>();
        }

        public async Task<Guid> CreateParticipantEventLogAsync(CreateParticipantEventLogCommand command, CancellationToken cancellationToken)
        {
            var entity = new ParticipantEventLogEntity()
            {
                Id = Guid.NewGuid(),
                ParticipantId = command.ParticipantId,
                Text = command.Text,
            };

            await participantEventLogs.AddAsync(entity, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task DeleteParticipantEventLogAsync(DeleteParticipantEventLogCommand command, CancellationToken cancellationToken)
        {
            var entity = await participantEventLogs
                .Where(i => i.Id == command.Id)
                .SingleAsync(cancellationToken);

            participantEventLogs.Remove(entity);
            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateParticipantEventLogAsync(UpdateParticipantEventLogCommand command, CancellationToken cancellationToken)
        {
            var entity = await participantEventLogs
                .Where(i => i.Id == command.Id)
                .SingleAsync(cancellationToken);

            entity.Text = command.Text;

            participantEventLogs.Update(entity);

            await dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
