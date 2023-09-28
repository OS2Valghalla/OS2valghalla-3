using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.Shared.WorkLocation.Responses;
using Valghalla.External.Application.Modules.Unprotected.Interfaces;
using Valghalla.External.Application.Modules.Unprotected.Queries;
using Valghalla.External.Application.Modules.Unprotected.Request;
using Valghalla.External.Application.Modules.Unprotected.Responses;

namespace Valghalla.External.Infrastructure.Modules.Unprotected
{
    internal class UnprotectedTasksQueryRepository: IUnprotectedTasksQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<TasksFilteredLinkEntity> tasksFilteredLinks;
        private readonly IQueryable<TaskAssignmentEntity> taskAssignments;

        public UnprotectedTasksQueryRepository(DataContext dataContext, IMapper mapper)
        {            
            this.mapper = mapper;
            tasksFilteredLinks = dataContext.Set<TasksFilteredLinkEntity>().AsNoTracking();
            taskAssignments = dataContext.Set<TaskAssignmentEntity>().AsNoTracking();
        }

        public async Task<TasksFiltersOptionsResponse> GetTasksFiltersOptionsAsync(GetTasksFiltersOptionsQuery query, CancellationToken cancellationToken)
        {
            var tasksFilteredLink = await tasksFilteredLinks.FirstOrDefaultAsync(x => x.HashValue == query.HashValue);
            if (tasksFilteredLink == null) 
            {
                return null;
            }

            TasksFilterRequest tasksFilter = JsonSerializer.Deserialize<TasksFilterRequest>(tasksFilteredLink.Value);
            var queryableObj = taskAssignments.Include(x => x.WorkLocation).Include(x => x.TaskType)
               .Where(i => i.ElectionId == tasksFilteredLink.ElectionId && i.TeamId == tasksFilter.TeamId && !i.ParticipantId.HasValue);

            if (tasksFilter.AreaIds!.Any())
            {
                queryableObj = queryableObj.Where(i => tasksFilter.AreaIds!.Contains(i.WorkLocation.AreaId));
            }
            if (tasksFilter.TaskTypeIds!.Any())
            {
                queryableObj = queryableObj.Where(i => tasksFilter.TaskTypeIds!.Contains(i.TaskTypeId));
            }
            if (tasksFilter.TrustedTaskType.HasValue)
            {
                queryableObj = queryableObj.Where(i => i.TaskType.Trusted == tasksFilter.TrustedTaskType.Value);
            }
            var tasks = await queryableObj.OrderBy(x => x.TaskDate).ToListAsync(cancellationToken);
            var resultWorkLocations = tasks.Select(x => x.WorkLocation).OrderBy(x => x.Title).Select(mapper.Map<WorkLocationSharedResponse>).DistinctBy(x => x.Id).ToList();

            UnprotectedTasksFilterRequest unprotectedTasksFilter = JsonSerializer.Deserialize<UnprotectedTasksFilterRequest>(tasksFilteredLink.Value);

            var result = new TasksFiltersOptionsResponse()
            {
                TaskDates = tasks.Select(x => x.TaskDate).Distinct().ToList(),
                WorkLocations = resultWorkLocations,
                TasksFilter = unprotectedTasksFilter
            };

            return result;
        }

        public async Task<IList<UnprotectedAvailableTasksDetailsResponse>> GetAvailableTasksByFiltersAsync(GetUnprotectedAvailableTasksByFiltersQuery query, CancellationToken cancellationToken)
        {
            var tasksFilteredLink = await tasksFilteredLinks.FirstOrDefaultAsync(x => x.HashValue == query.HashValue);
            if (tasksFilteredLink == null)
            {
                return null;
            }

            List<UnprotectedAvailableTasksDetailsResponse> result = new List<UnprotectedAvailableTasksDetailsResponse>();

            TasksFilterRequest tasksFilter = JsonSerializer.Deserialize<TasksFilterRequest>(tasksFilteredLink.Value);

            var queryableObj = taskAssignments.Include(x => x.WorkLocation).Include(x => x.TaskType)
                .Where(i => i.ElectionId == tasksFilteredLink.ElectionId && i.TeamId == tasksFilter.TeamId && !i.ParticipantId.HasValue);

            if (tasksFilter.AreaIds!.Any())
            {
                queryableObj = queryableObj.Where(i => tasksFilter.AreaIds!.Contains(i.WorkLocation.AreaId));
            }
            if (tasksFilter.TaskTypeIds!.Any())
            {
                queryableObj = queryableObj.Where(i => tasksFilter.TaskTypeIds!.Contains(i.TaskTypeId));
            }
            if (tasksFilter.TrustedTaskType.HasValue)
            {
                queryableObj = queryableObj.Where(i => i.TaskType.Trusted == tasksFilter.TrustedTaskType.Value);
            }
            if (query.Filters.WorkLocationIds!.Any())
            {
                queryableObj = queryableObj.Where(i => query.Filters.WorkLocationIds!.Contains(i.WorkLocationId));
            }
            if (query.Filters.TaskDate.HasValue)
            {
                queryableObj = queryableObj.Where(i => i.TaskDate == query.Filters.TaskDate.Value);
            }

            var tasks = await queryableObj.OrderBy(x => x.TaskDate).ThenBy(x => x.TaskType.Title).ToListAsync(cancellationToken);
            foreach (var task in tasks)
            {
                if (result.Any(t => t.TaskDate == task.TaskDate && t.WorkLocationId == task.WorkLocationId && t.TaskTypeId == task.TaskTypeId)) continue;

                var tasksDetails = mapper.Map<UnprotectedAvailableTasksDetailsResponse>(task);
                tasksDetails.AvailableTasksCount = tasks.Count(t => t.TaskDate == task.TaskDate && t.WorkLocationId == task.WorkLocationId && t.TaskTypeId == task.TaskTypeId);

                result.Add(tasksDetails);
            }

            return result;
        }
    }
}
