using System.Linq.Dynamic.Core;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Shared.Area.Responses;
using Valghalla.Internal.Application.Modules.Shared.Team.Responses;
using Valghalla.Internal.Application.Modules.Shared.WorkLocation.Responses;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Queries;
using Valghalla.Internal.Application.Modules.Tasks.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Tasks
{
    internal class ElectionAreaTasksQueryRepository : IElectionAreaTasksQueryRepository
    {
        private readonly IQueryable<ElectionEntity> elections;
        private readonly IQueryable<AreaEntity> areas;
        private readonly IQueryable<TaskAssignmentEntity> taskAssignments;
        private readonly IQueryable<WorkLocationEntity> workLocations;
        private readonly IQueryable<ElectionWorkLocationEntity> electionWorkLocations;
        private readonly IMapper mapper;

        public ElectionAreaTasksQueryRepository(DataContext dataContext, IMapper mapper)
        {
            elections = dataContext.Set<ElectionEntity>().AsNoTracking();
            areas = dataContext.Set<AreaEntity>().AsNoTracking();
            taskAssignments = dataContext.Set<TaskAssignmentEntity>().AsNoTracking();
            workLocations = dataContext.Set<WorkLocationEntity>().AsNoTracking();
            electionWorkLocations = dataContext.Set<ElectionWorkLocationEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<ElectionAreasGeneralInfoResponse> GetElectionAreasGeneralInfoAsync(GetElectionAreasGeneralInfoQuery query, CancellationToken cancellationToken)
        {
            var election = await elections.FirstAsync(e => e.Id == query.ElectionId);

            var areasEntities = await areas
                .OrderBy(i => i.Name)
                .ToListAsync(cancellationToken);

            var workLocations = await electionWorkLocations
                .Include(x => x.WorkLocation).ThenInclude(x => x.WorkLocationTeams).ThenInclude(x => x.Team)
                .Include(x => x.WorkLocation).ThenInclude(x => x.WorkLocationTaskTypes).ThenInclude(x => x.TaskType)
                .Where(x => x.ElectionId == query.ElectionId).OrderBy(x => x.WorkLocation.Title).ToListAsync(cancellationToken);

            var response = new ElectionAreasGeneralInfoResponse
            {
                ElectionStartDate = election.ElectionStartDate,
                ElectionEndDate = election.ElectionEndDate,
                Areas = areasEntities.Select(mapper.Map<AreaSharedResponse>).ToList(),
                WorkLocations = workLocations.Select(x => mapper.Map<WorkLocationSharedResponse>(x.WorkLocation)).ToList(),
                TaskTypes = workLocations.SelectMany(x => x.WorkLocation.WorkLocationTaskTypes.Select(w => mapper.Map<TaskTypeWithAreaIdsResponse>(w.TaskType))).DistinctBy(t => t.Id).OrderBy(t => t.ShortName).ToList(),
                Teams = workLocations.SelectMany(x => x.WorkLocation.WorkLocationTeams.Select(w => mapper.Map<TeamSharedResponse>(w.Team))).Distinct().OrderBy(t => t.Name).ToList()
            };

            foreach (var workLocationTaskType in response.TaskTypes)
            {
                workLocationTaskType.AreaIds = workLocations.Where(x => x.WorkLocation.WorkLocationTaskTypes.Any(t => t.TaskTypeId == workLocationTaskType.Id)).Select(t => t.WorkLocation.AreaId).ToList();
            }

            return response;
        }

        public async Task<IList<WorkLocationTasksSummaryResponse>> GetElectionAreaTasksSummaryAsync(GetElectionAreaTasksSummaryQuery query, CancellationToken cancellationToken)
        {
            List<WorkLocationTasksSummaryResponse> response = new List<WorkLocationTasksSummaryResponse>();

            var tasksQuery = taskAssignments.Where(i => i.ElectionId == query.ElectionId);

            if (query.SelectedDates != null && query.SelectedDates.Any())
            {
                tasksQuery = tasksQuery.Where(i => query.SelectedDates.Contains(i.TaskDate));
            }

            if (query.SelectedTeamIds != null && query.SelectedTeamIds.Any())
            {
                tasksQuery = tasksQuery.Where(i => query.SelectedTeamIds.Contains(i.TeamId));
            }

            var tasks = await tasksQuery.ToListAsync(cancellationToken);

            foreach (var task in tasks)
            {
                if (response.Any(t => t.WorkLocationId == task.WorkLocationId && t.TaskTypeId == task.TaskTypeId)) continue;

                var assignedTasksCount = tasks.Count(t => t.WorkLocationId == task.WorkLocationId && t.TaskTypeId == task.TaskTypeId && t.Accepted);
                var missingTasksCount = tasks.Count(t => t.WorkLocationId == task.WorkLocationId && t.TaskTypeId == task.TaskTypeId && !t.ParticipantId.HasValue);
                var awaitingTasksCount = tasks.Count(t => t.WorkLocationId == task.WorkLocationId && t.TaskTypeId == task.TaskTypeId && t.ParticipantId.HasValue && !t.Responsed);

                response.Add(new WorkLocationTasksSummaryResponse
                {
                    WorkLocationId = task.WorkLocationId,
                    TaskTypeId = task.TaskTypeId,
                    AssignedTasksCount = assignedTasksCount,
                    MissingTasksCount = missingTasksCount,
                    AwaitingTasksCount = awaitingTasksCount,
                    AllTasksCount = assignedTasksCount + missingTasksCount + awaitingTasksCount
                });
            }

            return response;
        }
    }
}
