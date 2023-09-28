using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Commands;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.SpecialDiet
{
    internal class SpecialDietCommandRepository : ISpecialDietCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<SpecialDietEntity> specialDiet;
        private readonly DbSet<SpecialDietParticipantEntity> specialDietParticipants;

        public SpecialDietCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            specialDiet = dataContext.Set<SpecialDietEntity>();
            specialDietParticipants = dataContext.Set<SpecialDietParticipantEntity>();
        }

        public async Task<Guid> CreateSpecialDietAsync(CreateSpecialDietCommand command, CancellationToken cancellationToken)
        {
            var entity = new SpecialDietEntity()
            {
                Id = Guid.NewGuid(),
                Title = command.Title,
            };

            await specialDiet.AddAsync(entity, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task DeleteSpecialDietAsync(DeleteSpecialDietCommand command, CancellationToken cancellationToken)
        {
            var entity = await specialDiet.Include(i => i.SpecialDietParticipants).SingleAsync(i => i.Id == command.Id, cancellationToken);

            specialDiet.Remove(entity);
            specialDietParticipants.RemoveRange(entity.SpecialDietParticipants);

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateSpecialDietAsync(UpdateSpecialDietCommand command, CancellationToken cancellationToken)
        {
            var entity = await specialDiet.SingleAsync(i => i.Id == command.Id, cancellationToken);
            entity.Title = command.Title;
            specialDiet.Update(entity);

            await dataContext.SaveChangesAsync(cancellationToken);
        }
    }

}
