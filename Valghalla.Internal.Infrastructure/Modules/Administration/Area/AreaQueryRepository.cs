using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Area.Commands;
using Valghalla.Internal.Application.Modules.Administration.Area.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Area.Queries;
using Valghalla.Internal.Application.Modules.Administration.Area.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.Area
{
    internal class AreaQueryRepository : IAreaQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<AreaEntity> areas;
        private readonly IQueryable<WorkLocationEntity> workLocations;

        public AreaQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;

            areas = dataContext.Set<AreaEntity>().AsNoTracking();
            workLocations = dataContext.Set<WorkLocationEntity>().AsNoTracking();
        }

        public async Task<bool> CheckIfAreaExistsAsync(CreateAreaCommand command, CancellationToken cancellationToken)
        {
            var name = command.Name.Trim().ToLower();
            return await areas.AnyAsync(i => i.Name.ToLower() == name, cancellationToken);
        }

        public async Task<bool> CheckIfAreaExistsAsync(UpdateAreaCommand command, CancellationToken cancellationToken)
        {
            var name = command.Name.Trim().ToLower();
            return await areas.AnyAsync(i => i.Name.ToLower() == name && i.Id != command.Id, cancellationToken);
        }

        public async Task<bool> CheckIfAreaHasWorkLocationsAsync(DeleteAreaCommand command, CancellationToken cancellationToken)
        {
            return await workLocations.AnyAsync(x => x.AreaId == command.Id, cancellationToken);
        }

        public async Task<bool> CheckIfAreaIsLastOneAsync(DeleteAreaCommand command, CancellationToken cancellationToken)
        {
            var count = await areas
                .CountAsync(cancellationToken);

            return count == 1;
        }

        public async Task<AreaDetailsResponse?> GetAreaAsync(GetAreaDetailsQuery query, CancellationToken cancellationToken)
        {
            var entity = await areas
                .Where(i => i.Id == query.Id)
                .SingleOrDefaultAsync(cancellationToken);

            return mapper.Map<AreaDetailsResponse>(entity);
        }

        public async Task<IList<AreaListingItemResponse>> GetAllAreasAsync(GetAllAreasQuery query, CancellationToken cancellationToken)
        {
            var entities = await areas.OrderBy(i => i.Name)
                .ToListAsync(cancellationToken);

            return entities.Select(mapper.Map<AreaListingItemResponse>).ToList();
        }
    }
}
