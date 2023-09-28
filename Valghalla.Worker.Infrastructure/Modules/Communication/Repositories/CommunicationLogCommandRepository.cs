using Microsoft.EntityFrameworkCore;
using Valghalla.Database;

namespace Valghalla.Worker.Infrastructure.Modules.Communication.Repositories
{
    public interface ICommunicationLogCommandRepository
    {
        Task ClearCommunicationLogsAsync(DateTime time, CancellationToken cancellationToken);
    }

    internal class CommunicationLogCommandRepository : ICommunicationLogCommandRepository
    {
        private readonly DataContext dataContext;

        public CommunicationLogCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task ClearCommunicationLogsAsync(DateTime time, CancellationToken cancellationToken)
        {
            await dataContext.CommunicationLogs
                .Where(t => t.CreatedAt < time)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}
