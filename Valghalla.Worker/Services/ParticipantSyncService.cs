using Valghalla.Application.Communication;
using Valghalla.Application.CPR;
using Valghalla.Application.TaskValidation;
using Valghalla.Worker.Infrastructure.Modules.Election.Repositories;
using Valghalla.Worker.Infrastructure.Modules.Participant.Repositories;
using Valghalla.Worker.Infrastructure.Modules.Participant.Requests;
using Valghalla.Worker.Infrastructure.Modules.Tasks.Repositories;
using Valghalla.Worker.Infrastructure.Modules.Tasks.Responses;

namespace Valghalla.Worker.Services
{
    internal interface IParticipantSyncService
    {
        Task ExecuteAsync(string tenantName, IEnumerable<Guid> electionIds, int batchSize, CancellationToken cancellationToken);
    }

    internal class ParticipantSyncService : IParticipantSyncService
    {
        private readonly ILogger<ParticipantSyncService> logger;

        private readonly IElectionQueryRepository electionQueryRepository;
        private readonly IParticipantQueryRepository participantQueryRepository;
        private readonly IParticipantCommandRepository participantCommandRepository;
        private readonly ITaskTypeQueryRepository taskTypeQueryRepository;
        private readonly ITaskAssignmentQueryRepository taskAssignmentQueryRepository;
        private readonly ITaskAssignmentCommandRepository taskAssignmentCommandRepository;
        private readonly ITaskValidationService taskValidationService;
        private readonly ICommunicationService communicationService;
        private readonly ICPRService cprService;

        public ParticipantSyncService(
            ILogger<ParticipantSyncService> logger,
            IElectionQueryRepository electionQueryRepository,
            IParticipantQueryRepository participantQueryRepository,
            IParticipantCommandRepository participantCommandRepository,
            ITaskTypeQueryRepository taskTypeQueryRepository,
            ITaskAssignmentQueryRepository taskAssignmentQueryRepository,
            ITaskAssignmentCommandRepository taskAssignmentCommandRepository,
            ITaskValidationService taskValidationService,
            ICommunicationService communicationService,
            ICPRService cprService)
        {
            this.logger = logger;
            this.electionQueryRepository = electionQueryRepository;
            this.participantQueryRepository = participantQueryRepository;
            this.participantCommandRepository = participantCommandRepository;
            this.taskTypeQueryRepository = taskTypeQueryRepository;
            this.taskAssignmentQueryRepository = taskAssignmentQueryRepository;
            this.taskAssignmentCommandRepository = taskAssignmentCommandRepository;
            this.taskValidationService = taskValidationService;
            this.communicationService = communicationService;
            this.cprService = cprService;
        }

        public async Task ExecuteAsync(string tenantName, IEnumerable<Guid> electionIds, int batchSize, CancellationToken cancellationToken)
        {
            var taskTypes = await taskTypeQueryRepository.GetEvaluatedTaskTypesAsync(cancellationToken);
            var ruleDict = await GetDictionaryRulesAsync(electionIds, cancellationToken);
            var participants = await participantQueryRepository.GetParticipantsAsync(cancellationToken);

            var batches = participants.Chunk(batchSize);

            foreach (var batch in batches)
            {
                var tasks = batch
                    .Select(p => Task.Run(async () =>
                    {
                        try
                        {
                            return await cprService.ExecuteAsync(p.Cpr);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(
                                $"[{tenantName}]" +
                                "An error occurred when query from CPR service -- {@ex}", ex);

                            return null!;
                        }

                    }))
                    .ToArray();

                var cprInfos = (await Task.WhenAll(tasks))
                    .Where(i => i != null)
                    .ToArray();

                var syncItems = batch
                    .Where(i => cprInfos.Any(j => i.Cpr == j.Cpr))
                    .Select(i =>
                    {
                        var cprInfo = cprInfos.Single(j => i.Cpr == j.Cpr);
                        return new ParticipantSyncJobItem()
                        {
                            ParticipantId = i.Id,
                            Record = cprInfo.ToRecord()
                        };
                    })
                .ToArray();

                var evaluatedParticipants = await participantCommandRepository.UpdateRecordsAsync(syncItems, cancellationToken);
                var taskAssignments = await taskAssignmentQueryRepository.GetTaskAssignmentsAsync(
                    evaluatedParticipants.Select(i => i.Id),
                    electionIds,
                    cancellationToken);

                var invalidTaskAssignmentDict = Evaluate(evaluatedParticipants, taskAssignments, taskTypes, ruleDict);
                var invalidTaskAssignmentIds = invalidTaskAssignmentDict.Values.SelectMany(i => i);
                var invalidTaskAssignments = await taskAssignmentQueryRepository.GetTaskAssignmentsAsync(invalidTaskAssignmentIds, cancellationToken);
                await taskAssignmentCommandRepository.UnassignTaskAssignmentsAsync(invalidTaskAssignmentIds, cancellationToken);
                foreach (var taskAssignment in invalidTaskAssignments)
                {
                    if (taskAssignment.Accepted && taskAssignment.Responsed)
                    {
                        await communicationService.SendRemovedFromTaskByValidationAsync(taskAssignment.ParticipantId, taskAssignment.Id, cancellationToken);
                    }
                }
            }
        }

        private Dictionary<Guid, IEnumerable<Guid>> Evaluate(
            IEnumerable<EvaluatedParticipant> participants,
            IEnumerable<TaskAssignmentResponse> taskAssignments,
            IEnumerable<EvaluatedTaskType> taskTypes,
            Dictionary<Guid, IEnumerable<TaskValidationRule>> ruleDict)
        {
            var dict = new Dictionary<Guid, IEnumerable<Guid>>();

            foreach (var participant in participants)
            {
                var invalidTaskAssignmentIds = new List<Guid>();
                var currentTaskAssignments = taskAssignments.Where(i => i.ParticipantId == participant.Id);

                foreach (var taskAssignment in currentTaskAssignments)
                {
                    var taskType = taskTypes.Single(i => i.Id == taskAssignment.TaskTypeId);
                    ruleDict.TryGetValue(taskAssignment.ElectionId, out var rules);

                    var evaluatedTask = new EvaluatedTask()
                    {
                        TaskAssignmentId = taskAssignment.Id,
                        TaskTypeId = taskType.Id,
                        TaskDate = taskAssignment.TaskDate,
                        ValidationNotRequired = taskType.ValidationNotRequired
                    };

                    var taskValidationResult = taskValidationService.Execute(evaluatedTask, participant, rules!);

                    if (!taskValidationResult.Succeed)
                    {
                        invalidTaskAssignmentIds.Add(taskAssignment.Id);
                    }
                }

                if (invalidTaskAssignmentIds.Any())
                {
                    dict.Add(participant.Id, invalidTaskAssignmentIds);
                }
            }

            return dict;
        }

        private async Task<Dictionary<Guid, IEnumerable<TaskValidationRule>>> GetDictionaryRulesAsync(IEnumerable<Guid> electionIds, CancellationToken cancellationToken)
        {
            var dict = new Dictionary<Guid, IEnumerable<TaskValidationRule>>();

            foreach (var electionId in electionIds)
            {
                var rules = await electionQueryRepository.GetTaskValidationRulesAsync(electionId, cancellationToken);
                dict.Add(electionId, rules);
            }

            return dict;
        }
    }
}
