using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Worker.Infrastructure.Modules.Election.Repositories
{
    public interface IElectionCommandRepository
    {
        Task DeactivateElections(IEnumerable<Guid> electionIds, CancellationToken cancellationToken);
    }

    internal class ElectionCommandRepository : IElectionCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<ElectionEntity> elections;

        public ElectionCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            elections = dataContext.Set<ElectionEntity>();
        }

        public async Task DeactivateElections(IEnumerable<Guid> electionIds, CancellationToken cancellationToken)
        {
            var entities = await elections
                .Where(i => electionIds.Contains(i.Id))
                .ToArrayAsync(cancellationToken);

            foreach (var entity in entities)
            {
                entity.Active = false;
            }

            elections.UpdateRange(entities);

            await dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
