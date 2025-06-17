using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Shared.TaskTypeTemplate.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.TaskTypeTemplate.Queries;
using Valghalla.Internal.Application.Modules.Shared.TaskTypeTemplate.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Shared.TaskTypeTemplate
{
    internal class TaskTypeTemplateSharedQueryRepository: ITaskTypeTemplateSharedQueryRepository
    {
        private readonly IQueryable<TaskTypeTemplateEntity> TaskTypeTemplates;
        private readonly IMapper mapper;

        public TaskTypeTemplateSharedQueryRepository(DataContext dataContext, IMapper mapper)
        {
            TaskTypeTemplates = dataContext.Set<TaskTypeTemplateEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TaskTypeTemplateSharedResponse>> GetTaskTypeTemplatesAsync(GetTaskTypeTemplatesSharedQuery query, CancellationToken cancellationToken)
        {
            var entities = await TaskTypeTemplates
                .OrderBy(i => i.Title)
                .ToArrayAsync(cancellationToken);

            return entities
                .Select(mapper.Map<TaskTypeTemplateSharedResponse>)
                .ToArray();
        }
    }
}
