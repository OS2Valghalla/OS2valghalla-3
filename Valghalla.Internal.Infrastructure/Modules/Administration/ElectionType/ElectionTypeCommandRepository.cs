using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Commands;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.ElectionType
{
    public class ElectionTypeCommandRepository: IElectionTypeCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<ElectionTypeEntity> electionTypes;
        private readonly DbSet<ElectionTypeValidationRuleEntity> electionTypeValidationRules;
        public ElectionTypeCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            electionTypes = dataContext.Set<ElectionTypeEntity>();
            electionTypeValidationRules = dataContext.Set<ElectionTypeValidationRuleEntity>();
        }

        public async Task<Guid> CreateElectionTypeAsync(CreateElectionTypeCommand command, CancellationToken cancellationToken)
        {
            Guid electionTypeId = Guid.NewGuid();
            var entity = new ElectionTypeEntity()
            {
                Id = electionTypeId,
                Title = command.Title
            };

            await electionTypes.AddAsync(entity, cancellationToken);

            foreach (var electionTypeValidationRule in command.ValidationRuleIds.ToList())
            {
                var childEntity = new ElectionTypeValidationRuleEntity()
                {
                    ElectionTypeId = electionTypeId,
                    ValidationRuleId = electionTypeValidationRule
                };

                await electionTypeValidationRules.AddAsync(childEntity, cancellationToken);
            }

            await dataContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task UpdateElectionTypeAsync(UpdateElectionTypeCommand command, CancellationToken cancellationToken)
        {
            var entity = await electionTypes.SingleAsync(x => x.Id == command.Id, cancellationToken);
            var existingElectionTypeValidationRules = await electionTypeValidationRules.Where(x => x.ElectionTypeId == command.Id).ToListAsync(cancellationToken);

            entity.Title = command.Title;
            electionTypes.Update(entity);

            var deletedValidationRules = existingElectionTypeValidationRules.Where(r => !command.ValidationRuleIds.Contains(r.ValidationRuleId)).ToList();
            var newValidationRuleIds = command.ValidationRuleIds.Where(r => !existingElectionTypeValidationRules.Any(i => i.ValidationRuleId == r)).ToList();

            foreach (var deletedValidationRule in deletedValidationRules)
            {
                electionTypeValidationRules.Remove(deletedValidationRule);
            }

            foreach (var newValidationRuleId in newValidationRuleIds)
            {
                await electionTypeValidationRules.AddAsync(new ElectionTypeValidationRuleEntity
                {
                    ElectionTypeId = command.Id,
                    ValidationRuleId = newValidationRuleId
                }, cancellationToken);
            }

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteElectionTypeAsync(DeleteElectionTypeCommand command, CancellationToken cancellationToken)
        {
            var entity = await electionTypes.Include(x => x.ValidationRules).SingleAsync(x => x.Id == command.Id, cancellationToken);
            var existingElectionTypeValidationRules = await electionTypeValidationRules.Where(x => x.ElectionTypeId == command.Id).ToListAsync(cancellationToken);

            foreach (var existingElectionTypeValidationRule in existingElectionTypeValidationRules)
            {
                electionTypeValidationRules.Remove(existingElectionTypeValidationRule);
            }

            electionTypes.Remove(entity);

            await dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
