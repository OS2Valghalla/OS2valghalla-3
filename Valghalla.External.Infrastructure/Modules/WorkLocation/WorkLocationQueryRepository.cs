using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.Shared.WorkLocation.Responses;
using Valghalla.External.Application.Modules.Tasks.Responses;
using Valghalla.External.Application.Modules.Team.Responses;
using Valghalla.External.Application.Modules.WorkLocation.Interfaces;
using Valghalla.External.Application.Modules.WorkLocation.Responses;

namespace Valghalla.External.Infrastructure.Modules.WorkLocation
{
    internal class WorkLocationQueryRepository : IWorkLocationQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<ParticipantEntity> participants;
        private readonly IQueryable<WorkLocationEntity> workLocations;
        private readonly IQueryable<TaskAssignmentEntity> tasks;

        public WorkLocationQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            participants = dataContext.Set<ParticipantEntity>().AsNoTracking();
            workLocations = dataContext.Set<WorkLocationEntity>().AsNoTracking();
            tasks = dataContext.Set<TaskAssignmentEntity>().AsNoTracking();
        }

        public async Task<IList<WorkLocationSharedResponse>> GetMyWorkLocationsAsync(Guid participantId, CancellationToken cancellationToken)
        {
            var participant = await participants
                .Where(i => i.Id == participantId)
                .SingleAsync(cancellationToken);

            var workLocationEntities = await workLocations.Include(i => i.WorkLocationResponsibles)
                .Where(i => i.WorkLocationResponsibles.Any(x => x.ParticipantId == participant.Id))
                .OrderBy(i => i.Title).ToListAsync(cancellationToken);

            return workLocationEntities.Select(mapper.Map<WorkLocationSharedResponse>).ToList();
        }

        public async Task<IList<DateTime>> GetWorkLocationDatesAsync(Guid workLocationId, Guid participantId, CancellationToken cancellationToken)
        {
            var participant = await participants
               .Where(i => i.Id == participantId)
               .SingleAsync(cancellationToken);

            var workLocationEntities = await workLocations.Include(i => i.WorkLocationResponsibles)
                .Where(i => i.WorkLocationResponsibles.Any(x => x.ParticipantId == participant.Id))
                .OrderBy(i => i.Title).ToListAsync(cancellationToken);

            if (!workLocationEntities.Any(t => t.Id == workLocationId)) return new List<DateTime>();

            return tasks.Where(x => x.WorkLocationId == workLocationId).Select(x => x.TaskDate).Distinct().OrderBy(x => x).ToList();
        }

        public async Task<WorkLocationDetailsResponse> GetWorkLocationDetailsAsync(Guid workLocationId, DateTime taskDate, Guid participantId, CancellationToken cancellationToken)
        {
            var participant = await participants
               .Where(i => i.Id == participantId)
               .SingleAsync(cancellationToken);

            var workLocationEntities = await workLocations.Include(i => i.WorkLocationResponsibles)
                .Where(i => i.WorkLocationResponsibles.Any(x => x.ParticipantId == participant.Id))
                .OrderBy(i => i.Title).ToListAsync(cancellationToken);

            if (!workLocationEntities.Any(t => t.Id == workLocationId)) return new WorkLocationDetailsResponse();

            var workLocationTasks = await tasks.Include(x => x.TaskType).Include(x => x.Team).Include(x => x.Participant).Where(x => x.WorkLocationId == workLocationId && x.TaskDate == taskDate).ToListAsync(cancellationToken);
            var taskTypes = workLocationTasks.Select(x => x.TaskType).DistinctBy(x => x.Id).OrderBy(x => x.Title).ToList();

            WorkLocationDetailsResponse result = new()
            {
                AcceptedTasksCount = workLocationTasks.Count(x => x.Accepted),
                AllTasksCount = workLocationTasks.Count(),
                Participants = workLocationTasks.Where(x => x.Accepted).Select(x => x.Participant).DistinctBy(x => x.Id).OrderBy(i => i.FirstName + " " + i.LastName).Select(mapper.Map<WorkLocationParticipantDetailsResponse>).ToList()
            };
            foreach (var participantResult in result.Participants)
            {
                participantResult.TaskTypes = string.Join(", ", workLocationTasks.Where(x => x.ParticipantId == participantResult.Id).Select(x => x.TaskType.Title).DistinctBy(x => x).ToList());
                participantResult.Teams = string.Join(", ", workLocationTasks.Where(x => x.ParticipantId == participantResult.Id).Select(x => x.Team.Name).DistinctBy(x => x).ToList());
            }
            foreach (var taskType in taskTypes)
            {
                WorkLocationTaskTypeDetailsResponse taskTypeDetail = new()
                {
                    Id = taskType.Id,
                    Title = taskType.Title,
                    EndTime = taskType.EndTime,
                    StartTime = taskType.StartTime,
                    AcceptedTasksCount = workLocationTasks.Count(x => x.TaskTypeId == taskType.Id && x.Accepted),
                    AllTasksCount = workLocationTasks.Count(x => x.TaskTypeId == taskType.Id)
                };
                result.TaskTypes.Add(taskTypeDetail);
            }

            return result;
        }
    }
}
