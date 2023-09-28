using Microsoft.EntityFrameworkCore;
using Valghalla.Application.Communication;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Infrastructure.Communication
{
    internal class CommunicationLogRepository : ICommunicationLogRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<CommunicationLogEntity> communicationLogs;

        public CommunicationLogRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            communicationLogs = dataContext.Set<CommunicationLogEntity>();
        }

        public async Task<Guid> SetCommunicationLogAsync(Guid participantId, CommunicationLogMessageType messageType, CommunicationLogSendType sendType, string message, string shortMessage, CancellationToken cancellationToken)
        {
            var entity = new CommunicationLogEntity()
            {
                Id = Guid.NewGuid(),
                ParticipantId = participantId,
                MessageType = messageType.Value,
                SendType = sendType.Value,
                Message = message,
                ShortMessage = shortMessage,
                Status = CommunicationLogStatus.Awaiting.Value
            };

            await communicationLogs.AddAsync(entity, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task UpdateCommunicationLogSuccessAsync(Guid communicationLogId, string message, string shortMessage, CancellationToken cancellationToken)
        {
            var entity = await communicationLogs
                .Where(i => i.Id == communicationLogId)
                .SingleAsync(cancellationToken);

            entity.Status = CommunicationLogStatus.Success.Value;
            entity.Message = message;
            entity.ShortMessage = shortMessage;

            communicationLogs.Update(entity);

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateCommunicationLogErrorAsync(Guid communicationLogId, string message, string shortMessage, string error, CancellationToken cancellationToken)
        {
            var entity = await communicationLogs
                .Where(i => i.Id == communicationLogId)
                .SingleAsync(cancellationToken);

            entity.Status = CommunicationLogStatus.Error.Value;
            entity.Error = error;
            entity.Message = message;
            entity.ShortMessage = shortMessage;

            communicationLogs.Update(entity);

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateCommunicationLogErrorAsync(Guid communicationLogId, string error, CancellationToken cancellationToken)
        {
            var entity = await communicationLogs
                .Where(i => i.Id == communicationLogId)
                .SingleAsync(cancellationToken);

            entity.Status = CommunicationLogStatus.Error.Value;
            entity.Error = error;

            communicationLogs.Update(entity);

            await dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
