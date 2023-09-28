using Valghalla.Application.Abstractions.Messaging;

namespace Valghalla.Application.Confirmations
{
    public abstract class Confimrator
    {
        public abstract string Title { get; }
        public abstract string Message { get; }
        public bool BypassIfNoRuleTriggers { get; set; }
        public bool MultipleMessageEnabled { get; set; }
    }

    public abstract class Confirmator<T> : Confimrator, IConfirmator<T> where T : ConfirmationCommand
    {
        private readonly ICollection<ConfirmationRule<T>> _rules = new List<ConfirmationRule<T>>();
        private readonly ConfimationBuilder<T> builder;

        public ICollection<ConfirmationRule<T>> Rules
        {
            get
            {
                return _rules;
            }
        }

        public Confirmator()
        {
            builder = new ConfimationBuilder<T>(this);
        }

        protected ConfimationBuilder<T> WhenAsync(Func<T, CancellationToken, Task<bool>> predicate)
        {
            Rules.Add(new ConfirmationRule<T>()
            {
                Predicate = predicate
            });

            return builder;
        }
    }

    public sealed class ConfimationBuilder<T> where T : ConfirmationCommand
    {
        private readonly Confirmator<T> confirmator;

        public ConfimationBuilder(Confirmator<T> confirmator)
        {
            this.confirmator = confirmator;
        }

        public Confirmator<T> WithMessage(string message)
        {
            var currentRule = confirmator.Rules.Last();
            currentRule.Message = message;

            return confirmator;
        }

        public Confirmator<T> WithMessage(Func<T, CancellationToken, Task<string[]>> messageProvider)
        {
            var currentRule = confirmator.Rules.Last();
            currentRule.MessageProvider = messageProvider;

            return confirmator;
        }
    }
}
