using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Area.Commands;
using Valghalla.Internal.Application.Modules.Administration.Area.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.Area
{
    internal class AreaCommandRepository : IAreaCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<AreaEntity> areas;

        public AreaCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            areas = dataContext.Set<AreaEntity>();
        }

        public async Task<Guid> CreateAreaAsync(CreateAreaCommand command, CancellationToken cancellationToken)
        {
            var entity = new AreaEntity()
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Description = command.Description
            };

            await areas.AddAsync(entity, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task UpdateAreaAsync(UpdateAreaCommand command, CancellationToken cancellationToken)
        {
            var entity = await areas.SingleAsync(i => i.Id == command.Id, cancellationToken);

            entity.Name = command.Name;
            entity.Description = command.Description;

            areas.Update(entity);

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAreaAsync(DeleteAreaCommand command, CancellationToken cancellationToken)
        {
            var entity = await areas.SingleAsync(i => i.Id == command.Id, cancellationToken);

            areas.Remove(entity);

            await dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
