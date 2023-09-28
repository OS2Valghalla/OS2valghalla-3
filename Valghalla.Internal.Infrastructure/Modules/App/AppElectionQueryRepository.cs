using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.App.Interfaces;
using Valghalla.Internal.Application.Modules.App.Queries;
using Valghalla.Internal.Application.Modules.App.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.App
{
    internal class AppElectionQueryRepository : IAppElectionQueryRepository
    {
        private readonly IQueryable<ElectionEntity> elections;
        private readonly IMapper mapper;

        public AppElectionQueryRepository(DataContext dataContext, IMapper mapper)
        {
            elections = dataContext.Set<ElectionEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<AppElectionResponse?> GetDefaultElectionToWorkOnAsync(CancellationToken cancellationToken)
        {
            var entity = await elections
                .OrderByDescending(i => i.ElectionDate)
                .Take(1)
                .SingleOrDefaultAsync(cancellationToken);

            return mapper.Map<AppElectionResponse?>(entity);
        }

        public async Task<AppElectionResponse?> GetElectionToWorkOnAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await elections
                .Where(i => i.Id == id)
                .SingleOrDefaultAsync(cancellationToken);

            return mapper.Map<AppElectionResponse?>(entity);
        }

        public async Task<IEnumerable<AppElectionResponse>> GetElectionsToWorkOnAsync(GetElectionsToWorkOnQuery query, CancellationToken cancellationToken)
        {
            var entities = await elections
                .OrderByDescending(i => i.ElectionDate)
                .ToArrayAsync(cancellationToken);

            return entities
                .Select(mapper.Map<AppElectionResponse>)
                .ToArray();
        }
    }
}
