using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Shared.Area.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Area.Queries;
using Valghalla.Internal.Application.Modules.Shared.Area.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Shared.Area
{
    internal class AreaSharedQueryRepository : IAreaSharedQueryRepository
    {
        private readonly IQueryable<AreaEntity> areas;
        private readonly IMapper mapper;

        public AreaSharedQueryRepository(DataContext dataContext, IMapper mapper)
        {
            areas = dataContext.Set<AreaEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<IEnumerable<AreaSharedResponse>> GetAreasAsync(GetAreasSharedQuery query, CancellationToken cancellationToken)
        {
            var entities = await areas
                .OrderBy(i => i.Name)
                .ToArrayAsync(cancellationToken);

            return entities
                .Select(mapper.Map<AreaSharedResponse>)
                .ToArray();
        }
    }
}
