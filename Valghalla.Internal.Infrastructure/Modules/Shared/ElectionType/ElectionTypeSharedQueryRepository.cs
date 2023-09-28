using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Shared.ElectionType.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.ElectionType.Queries;
using Valghalla.Internal.Application.Modules.Shared.ElectionType.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Shared.ElectionType
{
    internal class ElectionTypeSharedQueryRepository : IElectionTypeSharedQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<ElectionTypeEntity> electionTypes;

        public ElectionTypeSharedQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            electionTypes = dataContext.Set<ElectionTypeEntity>().AsNoTracking();
        }

        public async Task<IEnumerable<ElectionTypeSharedResponse>> GetElectionTypesAsync(GetElectionTypesSharedQuery query, CancellationToken cancellationToken)
        {
            var entities = await electionTypes.OrderBy(x => x.Title).ToArrayAsync(cancellationToken);
            return entities.Select(mapper.Map<ElectionTypeSharedResponse>);
        }
    }
}
