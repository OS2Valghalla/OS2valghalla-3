using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Shared.TaskType.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.TaskType.Queries;
using Valghalla.Internal.Application.Modules.Shared.TaskType.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Shared.TaskType
{
    internal class TaskTypeSharedQueryRepository: ITaskTypeSharedQueryRepository
    {
        private readonly IQueryable<TaskTypeEntity> taskTypes;
        private readonly IMapper mapper;

        public TaskTypeSharedQueryRepository(DataContext dataContext, IMapper mapper)
        {
            taskTypes = dataContext.Set<TaskTypeEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TaskTypeSharedResponse>> GetTaskTypesAsync(GetTaskTypesSharedQuery query, CancellationToken cancellationToken)
        {
            var entities = await taskTypes
                .OrderBy(i => i.Title)
                .ToArrayAsync(cancellationToken);

            return entities
                .Select(mapper.Map<TaskTypeSharedResponse>)
                .ToArray();
        }
    }
}
