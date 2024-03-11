using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application.Communication;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Communication.Commands;
using Valghalla.Internal.Application.Modules.Administration.Communication.Queries;
using Valghalla.Internal.Application.Modules.Administration.Communication.Responses;
using ICommunicationQueryRepository = Valghalla.Internal.Application.Modules.Administration.Communication.Interfaces.ICommunicationQueryRepository;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.Communication
{
    internal class CommunicationQueryRepository: ICommunicationQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<CommunicationTemplateEntity> communicationTemplates;
        private readonly IQueryable<ElectionEntity> elections;
        private readonly IQueryable<TaskAssignmentEntity> tasks;
        private readonly IQueryable<RejectedTaskAssignmentEntity> rejectedTasks;
        private readonly IQueryable<ElectionTaskTypeCommunicationTemplateEntity> electionTaskTypeCommunicationTemplates;

        public CommunicationQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;

            communicationTemplates = dataContext.Set<CommunicationTemplateEntity>().AsNoTracking();
            elections = dataContext.Set<ElectionEntity>().AsNoTracking();
            tasks = dataContext.Set<TaskAssignmentEntity>().AsNoTracking();
            rejectedTasks = dataContext.Set<RejectedTaskAssignmentEntity>().AsNoTracking();
            electionTaskTypeCommunicationTemplates = dataContext.Set<ElectionTaskTypeCommunicationTemplateEntity>().AsNoTracking();
        }

        public async Task<bool> CheckIfCommunicationTemplateExistsAsync(CreateCommunicationTemplateCommand command, CancellationToken cancellationToken)
        {
            return await communicationTemplates.Where(i => i.Title.ToLower() == command.Title.ToLower()).AnyAsync(cancellationToken);
        }

        public async Task<bool> CheckIfCommunicationTemplateExistsAsync(UpdateCommunicationTemplateCommand command, CancellationToken cancellationToken)
        {
            return await communicationTemplates.Where(i => i.Id != command.Id && i.Title.ToLower() == command.Title.ToLower()).AnyAsync(cancellationToken);
        }

        public async Task<bool> CheckIsDefaultCommunicationTemplateAsync(Guid id, CancellationToken cancellationToken)
        {
            return await communicationTemplates.Where(i => i.Id == id && i.IsDefaultTemplate!.HasValue).AnyAsync(cancellationToken);
        }

        public async Task<bool> CheckIfCommunicationTemplateUsedInElectionAsync(DeleteCommunicationTemplateCommand command, CancellationToken cancellationToken)
        {
            var usedInElection = await elections
                .Where(i =>
                    i.ConfirmationOfRegistrationCommunicationTemplateId == command.Id ||
                    i.ConfirmationOfCancellationCommunicationTemplateId == command.Id ||
                    i.InvitationCommunicationTemplateId == command.Id ||
                    i.InvitationReminderCommunicationTemplateId == command.Id ||
                    i.TaskReminderCommunicationTemplateId == command.Id ||
                    i.RetractedInvitationCommunicationTemplateId == command.Id ||
                    i.RemovedFromTaskCommunicationTemplateId == command.Id ||
                    i.RemovedByValidationCommunicationTemplateId == command.Id)
                .AnyAsync(cancellationToken);

            if (usedInElection)
            {
                return usedInElection;
            }

            return await electionTaskTypeCommunicationTemplates
                .Where(i =>
                    i.ConfirmationOfRegistrationCommunicationTemplateId == command.Id ||
                    i.ConfirmationOfCancellationCommunicationTemplateId == command.Id ||
                    i.InvitationCommunicationTemplateId == command.Id ||
                    i.InvitationReminderCommunicationTemplateId == command.Id ||
                    i.TaskReminderCommunicationTemplateId == command.Id ||
                    i.RetractedInvitationCommunicationTemplateId == command.Id ||
                    i.RemovedFromTaskCommunicationTemplateId == command.Id ||
                    i.RemovedByValidationCommunicationTemplateId == command.Id)
                .AnyAsync(cancellationToken);
        }

        public async Task<CommunicationTemplateDetailsResponse?> GetCommunicationTemplateAsync(GetCommunicationTemplateQuery query, CancellationToken cancellationToken)
        {
            var entity = await communicationTemplates
                .Include(i => i.CommunicationTemplateFileReferences)
                .SingleOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

            if (entity == null) return null;
            return mapper.Map<CommunicationTemplateDetailsResponse>(entity);
        }

        public async Task<IList<ParticipantForSendingGroupMessageResponse>> GetParticipantsForSendingGroupMessageAsync(GetParticipantsForSendingGroupMessageQuery query, CancellationToken cancellationToken)
        {
            List<ParticipantForSendingGroupMessageResponse> result = new();
            List<TaskAssignmentEntity> tasksInElection = new();
            List<RejectedTaskAssignmentEntity> rejectedTasksInElection = new();

            if (!query.TaskStatuses!.Any() || query.TaskStatuses!.Any(s => s == Valghalla.Application.Enums.TaskStatus.Accepted) || query.TaskStatuses!.Any(s => s == Valghalla.Application.Enums.TaskStatus.Unanswered))
            {
                var tasksInElectionQuery = tasks.Include(x => x.Participant).ThenInclude(x => x.TeamForMembers).Where(x => x.ElectionId == query.ElectionId && x.ParticipantId.HasValue);
                if (query.WorkLocationIds!.Any())
                {
                    tasksInElectionQuery = tasksInElectionQuery.Where(i => query.WorkLocationIds!.Contains(i.WorkLocationId));
                }
                if (query.TeamIds!.Any())
                {
                    tasksInElectionQuery = tasksInElectionQuery.Where(i => query.TeamIds!.Contains(i.TeamId));
                }
                if (query.TaskTypeIds!.Any())
                {
                    tasksInElectionQuery = tasksInElectionQuery.Where(i => query.TaskTypeIds!.Contains(i.TaskTypeId));
                }
                if (query.TaskDates!.Any())
                {
                    tasksInElectionQuery = tasksInElectionQuery.Where(i => query.TaskDates!.Contains(i.TaskDate));
                }
                if (query.TaskStatuses!.Any(s => s == Valghalla.Application.Enums.TaskStatus.Accepted) && !query.TaskStatuses!.Any(s => s == Valghalla.Application.Enums.TaskStatus.Unanswered))
                {
                    tasksInElectionQuery = tasksInElectionQuery.Where(i => i.Accepted);
                }
                else if (!query.TaskStatuses!.Any(s => s == Valghalla.Application.Enums.TaskStatus.Accepted) && query.TaskStatuses!.Any(s => s == Valghalla.Application.Enums.TaskStatus.Unanswered))
                {
                    tasksInElectionQuery = tasksInElectionQuery.Where(i => !i.Accepted);
                }

                tasksInElection = await tasksInElectionQuery.OrderBy(x => x.Participant!.FirstName + " " + x.Participant!.LastName).ToListAsync(cancellationToken);
            }

            if (!query.TaskStatuses!.Any() || query.TaskStatuses!.Any(s => s == Valghalla.Application.Enums.TaskStatus.Rejected))
            {
                var rejectedTasksInElectionQuery = rejectedTasks.Include(x => x.Participant).ThenInclude(x => x.TeamForMembers).Where(x => x.ElectionId == query.ElectionId);
                if (query.WorkLocationIds!.Any())
                {
                    rejectedTasksInElectionQuery = rejectedTasksInElectionQuery.Where(i => query.WorkLocationIds!.Contains(i.WorkLocationId));
                }
                if (query.TeamIds!.Any())
                {
                    rejectedTasksInElectionQuery = rejectedTasksInElectionQuery.Where(i => query.TeamIds!.Contains(i.TeamId));
                }
                if (query.TaskTypeIds!.Any())
                {
                    rejectedTasksInElectionQuery = rejectedTasksInElectionQuery.Where(i => query.TaskTypeIds!.Contains(i.TaskTypeId));
                }
                if (query.TaskDates!.Any())
                {
                    rejectedTasksInElectionQuery = rejectedTasksInElectionQuery.Where(i => query.TaskDates!.Contains(i.TaskDate));
                }
                rejectedTasksInElection = await rejectedTasksInElectionQuery.OrderBy(x => x.Participant!.FirstName + " " + x.Participant!.LastName).ToListAsync(cancellationToken);
            }

            var participantList = tasksInElection.Select(x => x.Participant).DistinctBy(x => x.Id).ToList().Concat(rejectedTasksInElection.Select(x => x.Participant).DistinctBy(x => x.Id).ToList()).DistinctBy(x => x.Id).ToList();

            foreach(var participant in participantList)
            {
                ParticipantForSendingGroupMessageResponse participantForSendingGroupMessageResponse = new ParticipantForSendingGroupMessageResponse()
                {
                    ParticipantId = participant.Id,
                    ParticipantName = participant.FirstName + " " + participant.LastName,
                    TeamIds = participant.TeamForMembers.OrderBy(x => x.Name).Select(x => x.Id).ToList()
                };

                result.Add(participantForSendingGroupMessageResponse);
            }

            return result.OrderBy(x => x.ParticipantName).ToList();
        }

        public async Task<IList<CommunicationTaskParticipantInfo>> GetTasksForSendingGroupMessageAsync(Guid electionId, IEnumerable<Guid> workLocationIds, IEnumerable<Guid> teamIds, IEnumerable<Guid> taskTypeIds, 
            IEnumerable<Valghalla.Application.Enums.TaskStatus> taskStatuses, IEnumerable<DateTime> taskDates, CancellationToken cancellationToken)
        {
            List<CommunicationTaskParticipantInfo> result = new();

            if (!taskStatuses.Any() || taskStatuses.Any(s => s == Valghalla.Application.Enums.TaskStatus.Accepted) || taskStatuses.Any(s => s == Valghalla.Application.Enums.TaskStatus.Unanswered))
            {
                var tasksInElectionQuery = tasks.Include(x => x.Participant).ThenInclude(x => x.TeamForMembers).Where(x => x.ElectionId == electionId && x.ParticipantId.HasValue);
                if (workLocationIds.Any())
                {
                    tasksInElectionQuery = tasksInElectionQuery.Where(i => workLocationIds.Contains(i.WorkLocationId));
                }
                if (teamIds.Any())
                {
                    tasksInElectionQuery = tasksInElectionQuery.Where(i => teamIds.Contains(i.TeamId));
                }
                if (taskTypeIds.Any())
                {
                    tasksInElectionQuery = tasksInElectionQuery.Where(i => taskTypeIds.Contains(i.TaskTypeId));
                }
                if (taskDates.Any())
                {
                    tasksInElectionQuery = tasksInElectionQuery.Where(i => taskDates.Contains(i.TaskDate));
                }
                if (taskStatuses.Any(s => s == Valghalla.Application.Enums.TaskStatus.Accepted) && !taskStatuses.Any(s => s == Valghalla.Application.Enums.TaskStatus.Unanswered))
                {
                    tasksInElectionQuery = tasksInElectionQuery.Where(i => i.Accepted);
                }
                else if (!taskStatuses.Any(s => s == Valghalla.Application.Enums.TaskStatus.Accepted) && taskStatuses.Any(s => s == Valghalla.Application.Enums.TaskStatus.Unanswered))
                {
                    tasksInElectionQuery = tasksInElectionQuery.Where(i => !i.Accepted);
                }

                result.AddRange(await tasksInElectionQuery.Select(x => new CommunicationTaskParticipantInfo { ParticipantId = x.ParticipantId!.Value, TaskId = x.Id, IsRejectedTask = false }).ToListAsync(cancellationToken));
            }

            if (!taskStatuses.Any() || taskStatuses.Any(s => s == Valghalla.Application.Enums.TaskStatus.Rejected))
            {
                var rejectedTasksInElectionQuery = rejectedTasks.Include(x => x.Participant).ThenInclude(x => x.TeamForMembers).Where(x => x.ElectionId == electionId);
                if (workLocationIds.Any())
                {
                    rejectedTasksInElectionQuery = rejectedTasksInElectionQuery.Where(i => workLocationIds.Contains(i.WorkLocationId));
                }
                if (teamIds.Any())
                {
                    rejectedTasksInElectionQuery = rejectedTasksInElectionQuery.Where(i => teamIds.Contains(i.TeamId));
                }
                if (taskTypeIds.Any())
                {
                    rejectedTasksInElectionQuery = rejectedTasksInElectionQuery.Where(i => taskTypeIds.Contains(i.TaskTypeId));
                }
                if (taskDates.Any())
                {
                    rejectedTasksInElectionQuery = rejectedTasksInElectionQuery.Where(i => taskDates.Contains(i.TaskDate));
                }
                result.AddRange(await rejectedTasksInElectionQuery.Select(x => new CommunicationTaskParticipantInfo { ParticipantId = x.ParticipantId, TaskId = x.Id, IsRejectedTask = true }).ToListAsync(cancellationToken));
            }

            return result;
        }
    }
}
