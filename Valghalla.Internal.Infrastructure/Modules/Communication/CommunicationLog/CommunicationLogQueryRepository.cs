using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application.Communication;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Communication.CommunicationLog.Interfaces;
using Valghalla.Internal.Application.Modules.Communication.CommunicationLog.Queries;

namespace Valghalla.Internal.Infrastructure.Modules.Communication.CommunicationLog
{
    internal class CommunicationLogQueryRepository : ICommunicationLogQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<CommunicationLogEntity> communicationLogs;

        public CommunicationLogQueryRepository(IMapper mapper, DataContext dataContext)
        {
            this.mapper = mapper;
            communicationLogs = dataContext.Set<CommunicationLogEntity>().AsNoTracking();
        }

        public async Task<CommunicationLogDetails?> GetCommunicationLogDetailsAsync(GetCommunicationLogDetailsQuery query, CancellationToken cancellationToken)
        {
            var entity = await communicationLogs
                .Include(i => i.Participant)
                .Where(i => i.Id == query.Id)
                .SingleOrDefaultAsync(cancellationToken);

            return mapper.Map<CommunicationLogDetails>(entity);
        }
    }
}
