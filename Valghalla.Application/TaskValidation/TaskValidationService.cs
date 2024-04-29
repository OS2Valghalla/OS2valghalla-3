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

        public TaskValidationResult Execute(EvaluatedTask taskAssignment, EvaluatedParticipant participant, IEnumerable<TaskValidationRule> rules)
        {
            var failedRules = new List<TaskValidationRule>();

            if (participant.Deceased)
            {
                failedRules.Add(TaskValidationRule.Alive);
            }

            if (taskAssignment.ValidationNotRequired)
            {
                return new(failedRules);
            }

            if (rules.Any(i => i.Id == TaskValidationRule.Age18.Id))
            {
                var age = CalculateAge(participant, taskAssignment);

                if (age < 18)
                {
                    failedRules.Add(TaskValidationRule.Age18);
                }
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

        public async Task<TaskValidationResult> ExecuteAsync(Guid taskAssignmentId, Guid electionId, Guid participantId, CancellationToken cancellationToken)
        {
            var taskType = await taskValidationRepository.GetEvaluatedTask(taskAssignmentId, cancellationToken);
            var participant = await taskValidationRepository.GetEvaluatedParticipant(participantId, cancellationToken);
            var rules = await taskValidationRepository.GetValidationRules(electionId, cancellationToken);

            return Execute(taskType, participant, rules);
        }

        public async Task<TaskValidationResult> ExecuteAsync(Guid taskAssignmentId, Guid electionId, string cpr, CancellationToken cancellationToken)
        {
            var taskType = await taskValidationRepository.GetEvaluatedTask(taskAssignmentId, cancellationToken);
            var cprPersonInfo = await cprService.ExecuteAsync(cpr);
            var record = cprPersonInfo.ToRecord();

            var evaluatedParticipant = new EvaluatedParticipant()
            {
                Id = Guid.Empty,
                Birthdate = record.Birthdate,
                CountryCode = record.CountryCode,
                Deceased = record.Deceased,
                Disenfranchised = record.Disenfranchised,
                MunicipalityCode = record.MunicipalityCode
            };
                        
            var rules = await taskValidationRepository.GetValidationRules(electionId, cancellationToken);

            return Execute(taskType, evaluatedParticipant, rules);
        }

        private static int CalculateAge(EvaluatedParticipant participant, EvaluatedTask taskAssignment)
        {
            var taskDateLocalTime = taskAssignment.TaskDate.ToLocalTime();
            var birthDateLocalTime = participant.Birthdate.ToLocalTime();
            var taskDate = new DateTime(taskDateLocalTime.Year, taskDateLocalTime.Month, taskDateLocalTime.Day);
            var birthdate = new DateTime(birthDateLocalTime.Year, birthDateLocalTime.Month, birthDateLocalTime.Day);

            var months = taskDate.Month - birthdate.Month;
            var years = taskDate.Year - birthdate.Year;

            if (taskDate.Day < birthdate.Day)
            {
                months--;
            }

            if (months < 0)
            {
                years--;
            }

            return years;
        }
    }
}
