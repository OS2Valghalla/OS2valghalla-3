﻿using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Commands;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.TaskType
{
    internal class TaskTypeQueryRepository : ITaskTypeQueryRepository
    {
        private readonly IQueryable<TaskTypeEntity> taskTypes;
        private readonly IQueryable<TaskAssignmentEntity> taskAssignments;
        private readonly IMapper mapper;

        public TaskTypeQueryRepository(DataContext dataContext, IMapper mapper)
        {
            taskTypes = dataContext.Set<TaskTypeEntity>().AsNoTracking();
            taskAssignments = dataContext.Set<TaskAssignmentEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<bool> CheckIfTaskTypeExistsAsync(CreateTaskTypeCommand command, CancellationToken cancellationToken)
        {
            return await taskTypes
                .Where(i =>
                    i.Title.ToLower() == command.Title.ToLower() &&
                    i.ShortName.ToLower() == command.ShortName.ToLower() &&
                    i.StartTime == command.StartTime &&
                    i.EndTime == command.EndTime)
                .AnyAsync(cancellationToken);
        }
        public async Task<(bool, Guid?)> CheckIfTaskTypeExistsAsync(Guid taskTypeTemplateId, Guid workLocationId, CancellationToken cancellationToken)
        {
            var taskTypeId = await taskTypes
                .Where(i =>
                    i.TaskTypeTemplateEntityId == taskTypeTemplateId &&
                    i.WorkLocationTaskTypes.Any(wltt => wltt.WorkLocationId == workLocationId && wltt.TaskTypeId == i.Id)
                )
                .Select(i => (Guid?)i.Id)
                .FirstOrDefaultAsync(cancellationToken);

            return (taskTypeId != null, taskTypeId);
        }     

        public async Task<bool> CheckIfTaskTypeExistsAsync(UpdateTaskTypeCommand command, CancellationToken cancellationToken)
        {
            return await taskTypes
                .Where(i =>
                    i.Id != command.Id &&
                    i.Title.ToLower() == command.Title.ToLower() &&
                    i.ShortName.ToLower() == command.ShortName.ToLower() &&
                    i.StartTime == command.StartTime &&
                    i.EndTime == command.EndTime)
                .AnyAsync(cancellationToken);
        }

        public async Task<bool> CheckIfTaskTypeHasTasksInActiveElectionAsync(UpdateTaskTypeCommand command, CancellationToken cancellationToken)
        {
            return await taskTypes.AnyAsync(i =>
                i.Id == command.Id &&
                i.TaskAssignments.Any(t => t.Election.Active), cancellationToken);
        }

        public async Task<bool> CheckIfTaskTypeHasTasksInActiveElectionAsync(DeleteTaskTypeCommand command, CancellationToken cancellationToken)
        {
            return await taskTypes.AnyAsync(i =>
               i.Id == command.Id &&
               i.TaskAssignments.Any(t => t.Election.Active), cancellationToken);
        }

        public async Task<TaskTypeDetailResponse?> GetTaskTypeAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await taskTypes
                .Include(i => i.FileReferences)
                .Include(i => i.WorkLocationTaskTypes).ThenInclude(i => i.WorkLocation).ThenInclude(i => i.Elections)
                .Where(i => i.Id == id)
                .SingleOrDefaultAsync(cancellationToken);

            return mapper.Map<TaskTypeDetailResponse>(entity);
        }
        public async Task<IList<TaskTypeListingItemResponse>> GetAllTaskTypesAsync(CancellationToken cancellationToken)
        {
            var entities = await taskTypes.OrderBy(i => i.Title)
               .ToListAsync(cancellationToken);

            return entities.Select(mapper.Map<TaskTypeListingItemResponse>).ToList();
        }
        public async Task<IList<TaskTypeListingItemResponse>> GetAllTaskTypesByElectionIdAsync(Guid electionId, CancellationToken cancellationToken)
        {
            var entities = await taskTypes
                .Include(t => t.TaskAssignments)
                .Where(t => t.TaskAssignments.Any(a => a.ElectionId == electionId))
                .OrderBy(t => t.Title)
                .ToListAsync(cancellationToken);


            return entities.Select(mapper.Map<TaskTypeListingItemResponse>).ToList();
        }
    }
}
