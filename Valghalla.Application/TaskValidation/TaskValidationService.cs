using Valghalla.Application.Configuration;
using Valghalla.Application.CPR;

namespace Valghalla.Application.TaskValidation
{
    internal class TaskValidationService : ITaskValidationService
    {
        private readonly AppConfiguration appConfiguration;
        private readonly ITaskValidationRepository taskValidationRepository;
        private readonly ICPRService cprService;

        public TaskValidationService(ITaskValidationRepository taskValidationRepository, ICPRService cprService, AppConfiguration appConfiguration)
        {
            this.appConfiguration = appConfiguration;
            this.taskValidationRepository = taskValidationRepository;
            this.cprService = cprService;
        }

        public TaskValidationResult Execute(EvaluatedTaskType taskType, EvaluatedParticipant participant, IEnumerable<TaskValidationRule> rules)
        {
            var failedRules = new List<TaskValidationRule>();

            if (participant.Deceased)
            {
                failedRules.Add(TaskValidationRule.Alive);
            }

            if (taskType.ValidationNotRequired)
            {
                return new(failedRules);
            }

            if (rules.Any(i => i.Id == TaskValidationRule.Age18.Id) && participant.Age < 18)
            {
                failedRules.Add(TaskValidationRule.Age18);
            }

            if (rules.Any(i => i.Id == TaskValidationRule.Disenfranchised.Id) && participant.Disenfranchised)
            {
                failedRules.Add(TaskValidationRule.Disenfranchised);
            }

            if (rules.Any(i => i.Id == TaskValidationRule.Citizenship.Id) && participant.CountryCode != "5100")
            {
                failedRules.Add(TaskValidationRule.Citizenship);
            }

            if (rules.Any(i => i.Id == TaskValidationRule.ResidencyMunicipality.Id) && participant.MunicipalityCode != appConfiguration.Komkod)
            {
                failedRules.Add(TaskValidationRule.ResidencyMunicipality);
            }

            return new(failedRules);
        }

        public async Task<TaskValidationResult> ExecuteAsync(Guid taskTypeId, Guid electionId, Guid participantId, CancellationToken cancellationToken)
        {
            var taskType = await taskValidationRepository.GetEvaluatedTaskType(taskTypeId, cancellationToken);
            var participant = await taskValidationRepository.GetEvaluatedParticipant(participantId, cancellationToken);
            var rules = await taskValidationRepository.GetValidationRules(electionId, cancellationToken);

            return Execute(taskType, participant, rules);
        }

        public async Task<TaskValidationResult> ExecuteAsync(Guid taskId, Guid electionId, string cpr, CancellationToken cancellationToken)
        {
            var taskType = await taskValidationRepository.GetEvaluatedTaskTypeByTaskId(taskId, cancellationToken);
            var cprPersonInfo = await cprService.ExecuteAsync(cpr);
            var record = cprPersonInfo.ToRecord();

            var evaluatedParticipant = new EvaluatedParticipant()
            {
                Id = Guid.Empty,
                Age = record.Age,
                CountryCode = record.CountryCode,
                Deceased = record.Deceased,
                Disenfranchised = record.Disenfranchised,
                MunicipalityCode = record.MunicipalityCode
            };
                        
            var rules = await taskValidationRepository.GetValidationRules(electionId, cancellationToken);

            return Execute(taskType, evaluatedParticipant, rules);
        }
    }
}
