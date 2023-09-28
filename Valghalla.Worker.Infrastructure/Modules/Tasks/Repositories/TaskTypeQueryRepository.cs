using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application.TaskValidation;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Worker.Infrastructure.Modules.Tasks.Repositories
{
    public interface ITaskTypeQueryRepository
    {
        Task<IEnumerable<EvaluatedTaskType>> GetEvaluatedTaskTypesAsync(CancellationToken cancellationToken);
    }

    internal class TaskTypeQueryRepository : ITaskTypeQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<TaskTypeEntity> taskTypes;

        public TaskTypeQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            taskTypes = dataContext.Set<TaskTypeEntity>().AsNoTracking();
        }

        public async Task<IEnumerable<EvaluatedTaskType>> GetEvaluatedTaskTypesAsync(CancellationToken cancellationToken)
        {
            var entities = await taskTypes.ToArrayAsync(cancellationToken);
            return entities.Select(mapper.Map<EvaluatedTaskType>).ToArray();
        }
    }
}
