using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application.TaskValidation;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Worker.Infrastructure.Modules.Election.Responses;

namespace Valghalla.Worker.Infrastructure.Modules.Election.Repositories
{
    public interface IElectionQueryRepository
    {
        Task<IEnumerable<ElectionResponse>> GetActiveElectionsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<TaskValidationRule>> GetTaskValidationRulesAsync(Guid electionId, CancellationToken cancellationToken);
    }

    internal class ElectionQueryRepository : IElectionQueryRepository
    {
        public readonly IMapper mapper;
        public readonly IQueryable<ElectionEntity> elections;

        public ElectionQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            elections = dataContext.Set<ElectionEntity>().AsNoTracking();
        }

        public async Task<IEnumerable<ElectionResponse>> GetActiveElectionsAsync(CancellationToken cancellationToken)
        {
            var entities = await elections
                .Where(i => i.Active)
                .ToArrayAsync(cancellationToken);

            return entities.Select(mapper.Map<ElectionResponse>).ToArray();
        }

        public async Task<IEnumerable<TaskValidationRule>> GetTaskValidationRulesAsync(Guid electionId, CancellationToken cancellationToken)
        {
            var entities = await elections
                .Include(i => i.ElectionType)
                    .ThenInclude(i => i.ValidationRules)
                .Where(i => i.Id == electionId)
                .Select(i => i.ElectionType.ValidationRules)
                .SingleAsync(cancellationToken);

            return entities.Select(i => new TaskValidationRule(i.ValidationRuleId)).ToArray();
        }
    }
}
