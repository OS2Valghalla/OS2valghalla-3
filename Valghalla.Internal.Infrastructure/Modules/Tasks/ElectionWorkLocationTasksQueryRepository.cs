using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Shared.TaskType.Responses;
using Valghalla.Internal.Application.Modules.Shared.Team.Responses;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Queries;
using Valghalla.Internal.Application.Modules.Tasks.Responses;
using static MassTransit.ValidationResultExtensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Valghalla.Internal.Infrastructure.Modules.Tasks
{
    internal class ElectionWorkLocationTasksQueryRepository: IElectionWorkLocationTasksQueryRepository
    {
        private readonly IQueryable<TaskAssignmentEntity> taskAssignments;
        private readonly IQueryable<RejectedTaskAssignmentEntity> rejectedTaskAssignments;
        private readonly IQueryable<WorkLocationEntity> workLocations;
        private readonly IQueryable<ElectionWorkLocationEntity> electionWorkLocations;
        private readonly IMapper mapper;

        public ElectionWorkLocationTasksQueryRepository(DataContext dataContext, IMapper mapper)
        {
            taskAssignments = dataContext.Set<TaskAssignmentEntity>().AsNoTracking();
            rejectedTaskAssignments = dataContext.Set<RejectedTaskAssignmentEntity>().AsNoTracking();
            workLocations = dataContext.Set<WorkLocationEntity>().AsNoTracking();
            electionWorkLocations = dataContext.Set<ElectionWorkLocationEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<bool> CheckIfWorkLocationInElectionAsync(Guid workLocationId, Guid electionId, CancellationToken cancellationToken)
        {
            return await workLocations.Include(x => x.ElectionWorkLocations).AnyAsync(i => i.Id == workLocationId && i.ElectionWorkLocations.Any(e => e.ElectionId == electionId), cancellationToken);
        }

        public async Task<ElectionWorkLocationTasksSummaryResponse> GetElectionWorkLocationTasksSummaryAsync(GetElectionWorkLocationTasksSummaryQuery query, CancellationToken cancellationToken)
        {
            var electionWorkLocation = await electionWorkLocations.Include(x => x.Election).Include(x => x.WorkLocation).ThenInclude(x => x.WorkLocationTeams).ThenInclude(x => x.Team)
                .Include(x => x.WorkLocation).ThenInclude(x => x.WorkLocationTaskTypes).ThenInclude(x => x.TaskType)
                .Where(x => x.WorkLocationId == query.WorkLocationId && x.ElectionId == query.ElectionId).FirstAsync(cancellationToken);

            var tasks = await taskAssignments
                .Where(i => i.WorkLocationId == query.WorkLocationId && i.ElectionId == query.ElectionId).OrderBy(x => x.TaskDate).ToListAsync(cancellationToken);

            var response = new ElectionWorkLocationTasksSummaryResponse
            {
                ElectionStartDate = electionWorkLocation.Election.ElectionStartDate,
                ElectionEndDate = electionWorkLocation.Election.ElectionEndDate,
                ElectionDate = electionWorkLocation.Election.ElectionDate,
                Teams = electionWorkLocation.WorkLocation.WorkLocationTeams.OrderBy(x => x.Team.Name).Select(x => mapper.Map<TeamSharedResponse>(x.Team)).ToList(),
                TaskTypes = electionWorkLocation.WorkLocation.WorkLocationTaskTypes.OrderBy(x => x.TaskType.Title).Select(x => mapper.Map<TaskTypeSharedResponse>(x.TaskType)).ToList(),
                Tasks = new List<TeamTasksSummaryResponse>()
            };

            foreach(var task in tasks)
            {
                if (response.Tasks.Any(t => t.TasksDate == task.TaskDate && t.TeamId == task.TeamId && t.TaskTypeId == task.TaskTypeId)) continue;

                var assignedTasksCount = tasks.Count(t => t.TaskDate == task.TaskDate && t.TeamId == task.TeamId && t.TaskTypeId == task.TaskTypeId
                    && t.Accepted);

                var missingTasksCount = tasks.Count(t => t.TaskDate == task.TaskDate && t.TeamId == task.TeamId && t.TaskTypeId == task.TaskTypeId
                    && !t.ParticipantId.HasValue);

                var awaitingTasksCount = tasks.Count(t => t.TaskDate == task.TaskDate && t.TeamId == task.TeamId && t.TaskTypeId == task.TaskTypeId
                    && t.ParticipantId.HasValue && !t.Responsed);

                response.Tasks.Add(new TeamTasksSummaryResponse
                {
                    TasksDate = task.TaskDate,
                    TeamId = task.TeamId,
                    TaskTypeId = task.TaskTypeId,
                    AssignedTasksCount = assignedTasksCount,
                    MissingTasksCount = missingTasksCount,
                    AwaitingTasksCount = awaitingTasksCount,
                    AllTasksCount = assignedTasksCount + missingTasksCount + awaitingTasksCount
                });
            }

            return response;
        }

        public async Task<TaskAssignmentResponse> GetTaskAssignmentAsync(GetTaskAssignmentQuery query, CancellationToken cancellationToken)
        {
            var task = await taskAssignments.Include(t => t.TaskType).Include(t => t.Participant).Include(t => t.WorkLocation).Include(t => t.Team)
                .FirstAsync(i => i.Id == query.TaskAssignmentId && i.ElectionId == query.ElectionId);

            return mapper.Map<TaskAssignmentResponse>(task);
        }

        public async Task<IEnumerable<TaskAssignmentResponse>> GetTeamTasksAsync(GetTeamTasksQuery query, string taskDetailsPageUrl, CancellationToken cancellationToken)
        {
            List<TaskAssignmentResponse> result = new List<TaskAssignmentResponse>();

            var tasks = await taskAssignments.Include(t => t.TaskType).Include(t => t.Participant)
                .Where(i => i.WorkLocationId == query.WorkLocationId && i.ElectionId == query.ElectionId && i.TeamId == query.TeamId)
                .OrderBy(x => x.TaskDate).ThenBy(x => x.TaskType.Title).ThenBy(x => x.Participant != null ? x.Participant.FirstName + " " + x.Participant.LastName : "ZZZZZZZZZ").ToListAsync(cancellationToken);

            foreach (var task in tasks)
            {
                var tasksDetails = mapper.Map<TaskAssignmentResponse>(task);
                tasksDetails.TaskDetailsPageUrl = taskDetailsPageUrl + task.HashValue;

                result.Add(tasksDetails);
            }

            return result;
        }

        public async Task<IEnumerable<TaskAssignmentResponse>> GetRejectedTeamTasksAsync(GetTeamTasksQuery query, CancellationToken cancellationToken)
        {
            var tasks = await rejectedTaskAssignments.Include(t => t.TaskType).Include(t => t.Participant)
                .Where(i => i.WorkLocationId == query.WorkLocationId && i.ElectionId == query.ElectionId && i.TeamId == query.TeamId)
                .OrderBy(x => x.TaskDate).ThenBy(x => x.TaskType.Title).ThenBy(x => x.Participant.FirstName + " " + x.Participant.LastName).ToListAsync(cancellationToken);

            return tasks.Select(mapper.Map<TaskAssignmentResponse>);
        }

        public async Task<bool> CheckIfTaskHasConflictsAsync(Guid electionId, Guid taskAssignmentId, Guid participantId, CancellationToken cancellationToken)
        {
            var selectedTask = await taskAssignments.Include(t => t.TaskType)
                .FirstAsync(i => i.Id == taskAssignmentId && i.ElectionId == electionId); 

            return await taskAssignments.Include(t => t.TaskType)
                .Where(i =>
                    i.Id != selectedTask.Id &&
                    i.ParticipantId == participantId &&
                    i.TaskDate == selectedTask.TaskDate &&
                    !(
                        (i.TaskType.StartTime > selectedTask.TaskType.StartTime && i.TaskType.StartTime >= selectedTask.TaskType.EndTime) ||
                        (i.TaskType.EndTime <= selectedTask.TaskType.StartTime && i.TaskType.EndTime < selectedTask.TaskType.EndTime)
                    ))
                .AnyAsync(cancellationToken);
        }
    }
}
