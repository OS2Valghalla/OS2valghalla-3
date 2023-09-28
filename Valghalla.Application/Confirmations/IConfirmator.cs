namespace Valghalla.Application.Confirmations
{
    public interface IConfirmator<T> where T : class
    {
        abstract string Title { get; }
        abstract string Message { get; }
        public bool BypassIfNoRuleTriggers { get; }
        public bool MultipleMessageEnabled { get; }
        ICollection<ConfirmationRule<T>> Rules { get; }
    }
}
