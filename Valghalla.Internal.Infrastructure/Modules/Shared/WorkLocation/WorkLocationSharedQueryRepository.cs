using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Shared.WorkLocation.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.WorkLocation.Queries;
using Valghalla.Internal.Application.Modules.Shared.WorkLocation.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Shared.WorkLocation
{
    internal class WorkLocationSharedQueryRepository : IWorkLocationSharedQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<WorkLocationEntity> workLocations;

        public WorkLocationSharedQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            workLocations = dataContext.Set<WorkLocationEntity>().AsNoTracking();
        }

        public async Task<IEnumerable<WorkLocationSharedResponse>> GetWorkLocationsAsync(GetWorkLocationsSharedQuery query, CancellationToken cancellationToken)
        {
            var entities = await workLocations.OrderBy(x => x.Title).ToArrayAsync(cancellationToken);
            return entities.Select(mapper.Map<WorkLocationSharedResponse>);
        }

        public async Task<WorkLocationSharedResponse?> GetWorkLocationAsync(GetWorkLocationSharedQuery query, CancellationToken cancellationToken)
        {
            var entity = await workLocations.Include(x => x.ElectionWorkLocations)
                .SingleOrDefaultAsync(i => i.Id == query.WorkLocationId && (!query.ElectionId.HasValue || i.ElectionWorkLocations.Any(e => e.ElectionId == query.ElectionId)), cancellationToken);

            if (entity == null) return null;

            var mappedEntity = mapper.Map<WorkLocationSharedResponse>(entity);

            return mappedEntity;
        }
    }
}
