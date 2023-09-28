using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Shared.Election.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Election.Queries;
using Valghalla.Internal.Application.Modules.Shared.Election.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Shared.Election
{
    internal class ElectionSharedQueryRepository : IElectionSharedQueryRepository
    {
        private readonly IQueryable<ElectionEntity> elections;
        private readonly IMapper mapper;

        public ElectionSharedQueryRepository(DataContext dataContext, IMapper mapper)
        {
            elections = dataContext.Set<ElectionEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ElectionSharedResponse>> GetElectionsAsync(GetElectionsSharedQuery query, CancellationToken cancellationToken)
        {
            var entities = await elections
                .OrderByDescending(i => i.ElectionDate)
                .ToArrayAsync(cancellationToken);

            return entities
                .Select(mapper.Map<ElectionSharedResponse>)
                .ToArray();
        }
    }
}
