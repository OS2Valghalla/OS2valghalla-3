using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Commands;
using Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.TaskTypeTemplate
{
    internal class TaskTypeTemplateQueryRepository : ITaskTypeTemplateQueryRepository
    {
        private readonly IQueryable<TaskTypeTemplateEntity> TaskTypeTemplates;
        private readonly IQueryable<TaskAssignmentEntity> taskAssignments;
        private readonly IMapper mapper;

        public TaskTypeTemplateQueryRepository(DataContext dataContext, IMapper mapper)
        {
            TaskTypeTemplates = dataContext.Set<TaskTypeTemplateEntity>().AsNoTracking();
            taskAssignments = dataContext.Set<TaskAssignmentEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<bool> CheckIfTaskTypeTemplateExistsAsync(CreateTaskTypeTemplateCommand command, CancellationToken cancellationToken)
        {
            return await TaskTypeTemplates
                .Where(i =>
                    i.Title.ToLower() == command.Title.ToLower() &&
                    i.ShortName.ToLower() == command.ShortName.ToLower() &&
                    i.StartTime == command.StartTime &&
                    i.EndTime == command.EndTime)
                .AnyAsync(cancellationToken);
        }

        public async Task<bool> CheckIfTaskTypeTemplateExistsAsync(UpdateTaskTypeTemplateCommand command, CancellationToken cancellationToken)
        {
            return await TaskTypeTemplates
                .Where(i =>
                    i.Id != command.Id &&
                    i.Title.ToLower() == command.Title.ToLower() &&
                    i.ShortName.ToLower() == command.ShortName.ToLower() &&
                    i.StartTime == command.StartTime &&
                    i.EndTime == command.EndTime)
                .AnyAsync(cancellationToken);
        }

        public async Task<bool> CheckIfTaskTypeTemplateHasTasksInActiveElectionAsync(UpdateTaskTypeTemplateCommand command, CancellationToken cancellationToken)
        {
            return await TaskTypeTemplates.AnyAsync(i =>
                i.Id == command.Id &&
                i.TaskAssignments.Any(t => t.Election.Active), cancellationToken);
        }

        public async Task<bool> CheckIfTaskTypeTemplateHasTasksInActiveElectionAsync(DeleteTaskTypeTemplateCommand command, CancellationToken cancellationToken)
        {
            return await TaskTypeTemplates.AnyAsync(i =>
               i.Id == command.Id &&
               i.TaskAssignments.Any(t => t.Election.Active), cancellationToken);
        }

        public async Task<TaskTypeTemplateDetailResponse?> GetTaskTypeTemplateAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await TaskTypeTemplates
                .Include(i => i.FileReferences)
                .Where(i => i.Id == id)
                .SingleOrDefaultAsync(cancellationToken);

            return mapper.Map<TaskTypeTemplateDetailResponse>(entity);
        }

        public async Task<IList<TaskTypeTemplateListingItemResponse>> GetAllTaskTypeTemplatesAsync(CancellationToken cancellationToken)
        {
            var entities = await TaskTypeTemplates.OrderBy(i => i.Title)
               .ToListAsync(cancellationToken);

            return entities.Select(mapper.Map<TaskTypeTemplateListingItemResponse>).ToList();
        }    
    }
}
