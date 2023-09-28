using Valghalla.Application.TaskValidation;

namespace Valghalla.External.Application.Modules.Tasks.Responses
{
    public sealed record TaskConfirmationResult
    {
        public bool Succeed
        {
            get
            {
                return !CprInvalid && !Conflicted && !FailedRuleIds.Any();
            }
        }

        public bool CprInvalid { get; init; } = false;
        public bool Conflicted { get; init; } = false;
        public IEnumerable<Guid> FailedRuleIds { get; init; } = Enumerable.Empty<Guid>();

        private TaskConfirmationResult() { }

        public static TaskConfirmationResult SuccessResult() => new();

        public static TaskConfirmationResult CprInvalidResult() => new()
        {
            CprInvalid = true
        };

        public static TaskConfirmationResult ConflictResult() => new()
        {
            Conflicted = true
        };

        public static TaskConfirmationResult ValidationFailedResult(TaskValidationResult taskValidationResult) => new()
        {
            FailedRuleIds = taskValidationResult.FailedRules.Select(i => i.Id).ToArray()
        };
    }
}
