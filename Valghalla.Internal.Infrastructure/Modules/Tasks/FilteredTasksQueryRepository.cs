using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Shared.Area.Responses;
using Valghalla.Internal.Application.Modules.Shared.Team.Responses;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Queries;
using Valghalla.Internal.Application.Modules.Tasks.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Tasks
{
    internal class FilteredTasksQueryRepository: IFilteredTasksQueryRepository
    {
        private readonly IQueryable<ElectionEntity> elections;
        private readonly IQueryable<ElectionWorkLocationEntity> electionWorkLocations;
        private readonly IQueryable<WorkLocationTeamEntity> workLocationTeams;
        private readonly IQueryable<WorkLocationTaskTypeEntity> workLocationTaskTypes;
        private readonly IQueryable<TaskAssignmentEntity> taskAssignments;
        private readonly IMapper mapper;

        public FilteredTasksQueryRepository(DataContext dataContext, IMapper mapper)
        {
            elections = dataContext.Set<ElectionEntity>().AsNoTracking();
            electionWorkLocations = dataContext.Set<ElectionWorkLocationEntity>().AsNoTracking();
            workLocationTeams = dataContext.Set<WorkLocationTeamEntity>().AsNoTracking();
            workLocationTaskTypes = dataContext.Set<WorkLocationTaskTypeEntity>().AsNoTracking();
            taskAssignments = dataContext.Set<TaskAssignmentEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<TasksFiltersOptionsResponse> GetTasksFiltersOptionsAsync(GetTasksFiltersOptionsQuery query, CancellationToken cancellationToken)
        {
            var election = await elections.FirstAsync(e => e.Id == query.ElectionId);

            var relatedWorkLocations = await electionWorkLocations.Include(x => x.WorkLocation).ThenInclude(x => x.Area).Where(x => x.ElectionId == query.ElectionId).ToListAsync(cancellationToken);
            var relatedWorkLocationIds = relatedWorkLocations.Select(x => x.WorkLocationId).ToList();

            var resultAreas = relatedWorkLocations.Select(x => x.WorkLocation.Area).OrderBy(x => x.Name).Select(mapper.Map<AreaSharedResponse>).DistinctBy(x => x.Id).ToList();
            var resultTeams = workLocationTeams.Include(x => x.Team).Where(x => relatedWorkLocationIds.Contains(x.WorkLocationId)).Select(x => x.Team).OrderBy(x => x.Name).Select(mapper.Map<TeamSharedResponse>).DistinctBy(x => x.Id).ToList();

            var resultWorkLocations = relatedWorkLocations.Select(x => x.WorkLocation).OrderBy(x => x.Title).Select(mapper.Map<WorkLocationWithTeamIdsResponse>).DistinctBy(x => x.Id).ToList();
            foreach (var resultWorkLocation in resultWorkLocations)
            {
                resultWorkLocation.TeamIds = workLocationTeams.Where(x => x.WorkLocationId == resultWorkLocation.Id).Select(t => t.TeamId).ToList();
            }

            var resultTaskTypes = workLocationTaskTypes.Include(x => x.TaskType).Where(x => relatedWorkLocationIds.Contains(x.WorkLocationId)).Select(x => x.TaskType).OrderBy(x => x.Title).Select(mapper.Map<TaskTypeWithTeamIdsResponse>).DistinctBy(x => x.Id).ToList();
            foreach (var resultTaskType in resultTaskTypes)
            {
                var taskTypeWorkLocationIds = workLocationTaskTypes.Where(x => x.TaskTypeId == resultTaskType.Id).Select(x => x.WorkLocationId).Distinct().ToList();
                resultTaskType.TeamIds = workLocationTeams.Where(x => taskTypeWorkLocationIds.Contains(x.WorkLocationId)).Select(t => t.TeamId).Distinct().ToList();
            }

            var result = new TasksFiltersOptionsResponse()
            {
                ElectionStartDate = election.ElectionStartDate,
                ElectionEndDate = election.ElectionEndDate,
                Areas = resultAreas,
                Teams = resultTeams,
                WorkLocations = resultWorkLocations,
                TaskTypes = resultTaskTypes
            };

            return result;
        }

        public async Task<IList<AvailableTasksDetailsResponse>> GetAvailableTasksByFiltersAsync(GetAvailableTasksByFiltersQuery query, string taskDetailsPageUrl, CancellationToken cancellationToken)
        {
            List<AvailableTasksDetailsResponse> result = new List<AvailableTasksDetailsResponse>();

            var queryableObj = taskAssignments.Include(x => x.WorkLocation).Include(x => x.TaskType)
                .Where(i => i.ElectionId == query.ElectionId && i.TeamId == query.Filters.TeamId && !i.ParticipantId.HasValue);
            if (query.Filters.AreaIds!.Any())
            {
                queryableObj = queryableObj.Where(i => query.Filters.AreaIds!.Contains(i.WorkLocation.AreaId));
            }
            if (query.Filters.WorkLocationIds!.Any())
            {
                queryableObj = queryableObj.Where(i => query.Filters.WorkLocationIds!.Contains(i.WorkLocationId));
            }
            if (query.Filters.TaskTypeIds!.Any())
            {
                queryableObj = queryableObj.Where(i => query.Filters.TaskTypeIds!.Contains(i.TaskTypeId));
            }
            if (query.Filters.TaskDate.HasValue)
            {
                queryableObj = queryableObj.Where(i => i.TaskDate == query.Filters.TaskDate.Value);
            }
            if (query.Filters.TrustedTaskType.HasValue)
            {
                queryableObj = queryableObj.Where(i => i.TaskType.Trusted == query.Filters.TrustedTaskType.Value);
            }

            var tasks = await queryableObj.OrderBy(x => x.TaskDate).ThenBy(x => x.TaskType.Title).ToListAsync(cancellationToken);
            foreach (var task in tasks)
            {
                if (result.Any(t => t.TaskDate == task.TaskDate && t.WorkLocationId == task.WorkLocationId && t.TaskTypeId == task.TaskTypeId)) continue;

                var tasksDetails = mapper.Map<AvailableTasksDetailsResponse>(task);
                tasksDetails.TaskDetailsPageUrl = taskDetailsPageUrl + task.HashValue;
                tasksDetails.AvailableTasksCount = tasks.Count(t => t.TaskDate == task.TaskDate && t.WorkLocationId == task.WorkLocationId && t.TaskTypeId == task.TaskTypeId);

                result.Add(tasksDetails);
            }

            return result;
        }

        public async Task<IList<ParticipantTaskDetailsResponse>> GetParticipantTasksAsync(GetParticipantsTasksQuery query, CancellationToken cancellationToken)
        {
            List<ParticipantTaskDetailsResponse> result = new List<ParticipantTaskDetailsResponse>();

            var queryableObj = taskAssignments
                .Include(x => x.Participant).ThenInclude(x => x.SpecialDiets)                
                .Include(x => x.Participant).ThenInclude(x => x.User)
                .Include(x => x.WorkLocation).ThenInclude(x => x.Area)
                .Include(x => x.Team)
                .Include(x => x.TaskType)
                .Where(i => i.ElectionId == query.ElectionId && i.ParticipantId.HasValue);

            if (query.Filters.TaskStatus != null)
            {
                if (query.Filters.TaskStatus == Valghalla.Application.Enums.TaskStatus.Accepted)
                {
                    queryableObj = queryableObj.Where(i => i.Accepted);
                }
                else
                {
                    queryableObj = queryableObj.Where(i => !i.Accepted);
                }
            }
            if (query.Filters.TeamIds!.Any())
            {
                queryableObj = queryableObj.Where(i => query.Filters.TeamIds!.Contains(i.TeamId));
            }
            if (query.Filters.WorkLocationIds!.Any())
            {
                queryableObj = queryableObj.Where(i => query.Filters.WorkLocationIds!.Contains(i.WorkLocationId));
            }
            if (query.Filters.AreaIds!.Any())
            {
                queryableObj = queryableObj.Where(i => query.Filters.AreaIds!.Contains(i.WorkLocation.AreaId));
            }            
            if (query.Filters.TaskTypeIds!.Any())
            {
                queryableObj = queryableObj.Where(i => query.Filters.TaskTypeIds!.Contains(i.TaskTypeId));
            }
            if (query.Filters.TaskDates!.Any())
            {
                queryableObj = queryableObj.Where(i => query.Filters.TaskDates!.Contains(i.TaskDate));
            }

            var tasks = await queryableObj.OrderBy(x => x.Participant!.FirstName + " " + x.Participant!.LastName).ThenBy(x => x.TaskDate).ToListAsync(cancellationToken);
            foreach(var task in tasks)
            {
                var tasksDetails = mapper.Map<ParticipantTaskDetailsResponse>(task);

                result.Add(tasksDetails);
            }

            return result;
        }
    }
}
