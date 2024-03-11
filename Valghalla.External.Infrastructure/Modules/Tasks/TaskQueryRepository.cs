using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application.QueryEngine;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.Tasks.Commands;
using Valghalla.External.Application.Modules.Tasks.Interfaces;
using Valghalla.External.Application.Modules.Tasks.Queries;
using Valghalla.External.Application.Modules.Tasks.Responses;

namespace Valghalla.External.Infrastructure.Modules.Tasks
{
    internal class TaskQueryRepository : ITaskQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<TaskAssignmentEntity> taskAssignments;
        private readonly IQueryable<ParticipantEntity> participants;

        public TaskQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            taskAssignments = dataContext.Set<TaskAssignmentEntity>().AsNoTracking();
            participants = dataContext.Set<ParticipantEntity>().AsNoTracking();
        }

        public async Task<TaskPreviewResponse?> GetTaskPreviewAsync(GetTaskPreviewQuery query, Guid? participantId, CancellationToken cancellationToken)
        {
            var entity = await taskAssignments
                .Where(i =>
                    i.HashValue == query.HashValue &&
                    (
                        (participantId.HasValue ? i.ParticipantId == participantId : i.ParticipantId == Guid.Empty) ||
                        (query.InvitationCode.HasValue ? i.InvitationCode == query.InvitationCode : i.ParticipantId == null)
                    ))
                .Include(i => i.Team)
                .Include(i => i.TaskType)
                .Include(i => i.WorkLocation)
                .FirstOrDefaultAsync(cancellationToken);

            return mapper.Map<TaskPreviewResponse>(entity);
        }

        public async Task<TeamResponsibleTaskPreviewResponse?> GetTeamResponsibleTaskPreviewAsync(GetTeamResponsibleTaskPreviewQuery query, Guid? participantId, CancellationToken cancellationToken)
        {
            var entities = await taskAssignments
                .Where(i => i.HashValue == query.HashValue)
                .Include(i => i.Team)
                .Include(i => i.TaskType)
                .Include(i => i.WorkLocation)
                .ToListAsync(cancellationToken);

            var result = mapper.Map<TeamResponsibleTaskPreviewResponse>(entities.First());
            result.TaskType.AllTasksCount = entities.Count();
            result.TaskType.AcceptedTasksCount = entities.Count(x => x.Accepted);
            result.TaskType.UnansweredTasksCount = entities.Count(x => x.ParticipantId.HasValue && !x.Accepted);

            return result;
        }

        public async Task<IList<TaskDetailsResponse>> GetMyTasksAsync(Guid participantId, CancellationToken cancellationToken)
        {
            var entities = await taskAssignments
               .Include(i => i.Election)
               .Include(i => i.Team)
               .Include(i => i.TaskType).ThenInclude(i => i.FileReferences)
               .Include(i => i.WorkLocation)
               .Where(i => i.ParticipantId == participantId && i.TaskDate >= DateTime.Today && i.Election.Active)
               .OrderBy(i => i.TaskDate).ThenBy(i => i.TaskType.Title)
               .ToArrayAsync(cancellationToken);

            return entities.Select(mapper.Map<TaskDetailsResponse>).ToList();
        }

        public async Task<TeamResponsibleTasksFiltersOptionsResponse> GetTeamResponsibleTasksFiltersOptionsAsync(Guid teamResponsibleId, CancellationToken cancellationToken)
        {
            var entities = await taskAssignments
               .Include(i => i.Election)
               .Include(i => i.Team)
               .Include(i => i.TaskType)
               .Include(i => i.WorkLocation)
               .Where(i => i.Team.TeamResponsibles.Any(x => x.ParticipantId == teamResponsibleId) && i.TaskDate >= DateTime.Today && i.Election.Active)
               .ToArrayAsync(cancellationToken);

            TeamResponsibleTasksFiltersOptionsResponse result = new TeamResponsibleTasksFiltersOptionsResponse();
            result.Teams = entities.Select(i => i.Team).DistinctBy(i => i.Id).OrderBy(i => i.Name).Select(mapper.Map<TaskPreviewTeam>).ToList();
            result.WorkLocations = entities.Select(i => i.WorkLocation).DistinctBy(i => i.Id).OrderBy(i => i.Title).Select(mapper.Map<TaskPreviewWorkLocation>).ToList();
            result.TaskTypes = entities.Select(i => i.TaskType).DistinctBy(i => i.Id).OrderBy(i => i.Title).Select(mapper.Map<TaskPreviewTaskType>).ToList();

            return result;
        }

        public async Task<TeamResponsibleTaskResponse> GetTeamResponsibleTasksAsync(Guid teamResponsibleId, GetTeamResponsibleTasksQuery query, CancellationToken cancellationToken)
        {
            var queryableObj = taskAssignments
               .Include(i => i.Election)
               .Include(i => i.Team)
               .Include(i => i.TaskType)
               .Include(i => i.WorkLocation)
               .Where(i => i.TeamId == query.TeamId && i.Team.TeamResponsibles.Any(x => x.ParticipantId == teamResponsibleId) && i.TaskDate >= DateTime.Today && i.Election.Active);

            if (query.TaskTypeId.HasValue)
            {
                queryableObj = queryableObj.Where(i => i.TaskTypeId == query.TaskTypeId.Value);
            }
            if (query.WorkLocationId.HasValue)
            {
                queryableObj = queryableObj.Where(i => i.WorkLocationId == query.WorkLocationId.Value);
            }
            if (query.TaskDate.HasValue)
            {
                queryableObj = queryableObj.Where(i => i.TaskDate == query.TaskDate.Value);
            }

            var entities = await queryableObj
               .OrderBy(i => i.TaskDate).ThenBy(i => i.TaskType.Title)
               .ToArrayAsync(cancellationToken);

            TeamResponsibleTaskResponse result = new()
            {
                TotalUnansweredTasksCount = entities.Count(t => t.ParticipantId.HasValue && !t.Accepted),
                TotalAcceptedTasksCount = entities.Count(t => t.Accepted),
                TotalTasksCount = entities.Count()
            };

            foreach (var task in entities)
            {
                if (result.Tasks.Any(t => t.TaskDate == task.TaskDate && t.WorkLocationId == task.WorkLocationId && t.TaskTypeId == task.TaskTypeId)) continue;

                var tasksDetails = mapper.Map<TeamResponsibleTaskDetailsResponse>(task);
                tasksDetails.UnansweredTasksCount = entities.Count(t => t.TaskDate == task.TaskDate && t.WorkLocationId == task.WorkLocationId && t.TaskTypeId == task.TaskTypeId && t.ParticipantId.HasValue && !t.Accepted);
                tasksDetails.AcceptedTasksCount = entities.Count(t => t.TaskDate == task.TaskDate && t.WorkLocationId == task.WorkLocationId && t.TaskTypeId == task.TaskTypeId && t.Accepted);
                tasksDetails.AllTasksCount = entities.Count(t => t.TaskDate == task.TaskDate && t.WorkLocationId == task.WorkLocationId && t.TaskTypeId == task.TaskTypeId);

                result.Tasks.Add(tasksDetails);
            }

            return result;
        }

        public async Task<TaskAssignmentResponse?> GetTaskAssignmentAsync(string hashValue, Guid? invitationCode, Guid participantId, CancellationToken cancellationToken)
        {
            var queryable = taskAssignments.Include(i => i.TaskType)
                .Where(i =>
                    i.HashValue == hashValue &&
                    i.Responsed == false);

            if (invitationCode != null)
            {
                queryable = queryable.Where(i =>
                    i.ParticipantId == participantId &&
                    i.InvitationCode == invitationCode);
            }
            else
            {
                queryable = queryable.Where(i => i.ParticipantId == null);
            }

            var entity = await queryable.FirstOrDefaultAsync(cancellationToken);
            return mapper.Map<TaskAssignmentResponse>(entity);
        }

        public async Task<bool> CheckIfTaskHasConflicts(Guid participantId, DateTime taskDate, TimeSpan startTime, TimeSpan endTime, Guid? invitationCode, CancellationToken cancellationToken)
        {
            var queryable = taskAssignments
                .Include(i => i.TaskType)
                .Where(i =>
                    i.ParticipantId == participantId &&
                    i.TaskDate == taskDate &&
                    !(
                        (i.TaskType.StartTime > startTime && i.TaskType.StartTime >= endTime) ||
                        (i.TaskType.EndTime <= startTime && i.TaskType.EndTime < endTime)
                    ));

            if (invitationCode.HasValue)
            {
                queryable = queryable.Where(i => i.InvitationCode != invitationCode);
            }

            return await queryable.AnyAsync(cancellationToken);
        }

        public async Task<IEnumerable<TaskOverviewItem>> GetTaskOverviewAsync(Guid participantId, GetTaskOverviewQuery query, CancellationToken cancellationToken)
        {
            var participantEntity = await participants
                .Include(i => i.TeamMembers)
                .Include(i => i.TeamResponsibles)
                .Where(i => i.Id == participantId)
                .SingleAsync(cancellationToken);

            var relevantTeamIds = new List<Guid>();
            relevantTeamIds.AddRange(participantEntity.TeamMembers.Select(i => i.TeamId));
            relevantTeamIds.AddRange(participantEntity.TeamResponsibles.Select(i => i.TeamId));
            relevantTeamIds = relevantTeamIds.Distinct().ToList();

            var queryBuilder = taskAssignments
                .Include(i => i.WorkLocation)
                .Include(i => i.TaskType)
                .Where(i =>
                    i.TaskType.Trusted == false &&
                    i.Accepted == false &&
                    i.Responsed == false &&
                    i.ParticipantId == null &&
                    i.Election.Active);

            if (query.TeamId.HasValue)
            {
                queryBuilder = queryBuilder.Where(i => i.TeamId == query.TeamId.Value);
            }
            else
            {
                queryBuilder = queryBuilder.Where(i => relevantTeamIds.Contains(i.TeamId));
            }

            if (query.TaskDate.HasValue)
            {
                queryBuilder = queryBuilder.Where(i => i.TaskDate == query.TaskDate.Value);
            }
            else
            {
                queryBuilder = queryBuilder.Where(i => i.TaskDate > DateTime.UtcNow);
            }

            if (query.TaskTypeId.HasValue)
            {
                queryBuilder = queryBuilder.Where(i => i.TaskTypeId == query.TaskTypeId.Value);
            }

            if (query.WorkLocationId.HasValue)
            {
                queryBuilder = queryBuilder.Where(i => i.WorkLocationId == query.WorkLocationId.Value);
            }

            var taskAssignmentEntities = await queryBuilder
                .OrderBy(i => i.TaskDate)
                .ThenBy(i => i.TaskType.Title)
                .ToArrayAsync(cancellationToken);

            var taskOverviewItems = new List<TaskOverviewItem>();
            var hashValues = taskAssignmentEntities.Select(i => i.HashValue).Distinct();

            foreach (var hashValue in hashValues)
            {
                var taskAssignmentEntity = taskAssignmentEntities.First(i => i.HashValue == hashValue);

                var item = mapper.Map<TaskOverviewItem>(taskAssignmentEntity);
                item.AvailableTasksCount = taskAssignmentEntities.Count(i => i.HashValue == hashValue);

                taskOverviewItems.Add(item);
            }

            return taskOverviewItems;
        }

        public async Task<TaskOverviewFilterOptions> GetTaskOverviewFilterOptionsAsync(Guid participantId, GetTaskOverviewFilterOptionsQuery query, CancellationToken cancellationToken)
        {
            var participantEntity = await participants
                .Include(i => i.TeamMembers)
                .Include(i => i.TeamResponsibles)
                .Where(i => i.Id == participantId)
                .SingleAsync(cancellationToken);

            var relevantTeamIds = new List<Guid>();
            relevantTeamIds.AddRange(participantEntity.TeamMembers.Select(i => i.TeamId));
            relevantTeamIds.AddRange(participantEntity.TeamResponsibles.Select(i => i.TeamId));
            relevantTeamIds = relevantTeamIds.Distinct().ToList();

            var tasks = await taskAssignments.Include(x => x.Election).Include(x => x.WorkLocation).Include(x => x.TaskType).Include(x => x.Team)
               .Where(i => i.Election.Active && i.TaskType.Trusted == false && !i.ParticipantId.HasValue && i.TaskDate > DateTime.UtcNow && relevantTeamIds.Contains(i.TeamId)).OrderBy(x => x.TaskDate).ToListAsync(cancellationToken);

            var resultWorkLocations = tasks.Select(x => x.WorkLocation).OrderBy(x => x.Title).DistinctBy(x => x.Id).Select(i => new SelectOption<Guid>(i.Id, i.Title)).ToList();
            var resultTaskTypes = tasks.Select(x => x.TaskType).OrderBy(x => x.Title).DistinctBy(x => x.Id).Select(i => new SelectOption<Guid>(i.Id, i.Title)).ToList();
            var resultTeams = tasks.Where(x => relevantTeamIds.Contains(x.TeamId)).Select(x => x.Team).OrderBy(x => x.Name).DistinctBy(x => x.Id).Select(i => new SelectOption<Guid>(i.Id, i.Name)).ToList();

            return new()
            {
                TaskDates = tasks.Select(x => x.TaskDate).Distinct().ToList(),
                WorkLocations = resultWorkLocations,
                TaskTypes = resultTaskTypes,
                Teams = resultTeams
            };
        }
    }
}
