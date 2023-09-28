namespace Valghalla.Application.TaskValidation
{
    public sealed class TaskValidationResult
    {
        public IEnumerable<TaskValidationRule> FailedRules { get; init; } = Enumerable.Empty<TaskValidationRule>();

        public bool Succeed
        {
            get
            {
                return !FailedRules.Any();
            }
        }

        public bool IsAlive() => !FailedRules.Any(i => i.Id == TaskValidationRule.Alive.Id);
        public bool IsAge18OrOver() => !FailedRules.Any(i => i.Id == TaskValidationRule.Age18.Id);
        public bool IsResidencyMunicipality() => !FailedRules.Any(i => i.Id == TaskValidationRule.ResidencyMunicipality.Id);
        public bool HasDanishCitizenship() => !FailedRules.Any(i => i.Id == TaskValidationRule.Citizenship.Id);
        public bool IsDisenfranchised() => !FailedRules.Any(i => i.Id == TaskValidationRule.Disenfranchised.Id);

        public TaskValidationResult(IEnumerable<TaskValidationRule> failedRules) => FailedRules = failedRules;
    }
}
