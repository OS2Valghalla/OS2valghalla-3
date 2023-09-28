namespace Valghalla.Application.Confirmations
{
    public class ConfirmationRule<T>
    {
        public Func<T, CancellationToken, Task<bool>> Predicate { get; set; } = null!;
        public string? Message { get; set; }
        public Func<T, CancellationToken, Task<string[]>>? MessageProvider { get; set; }
    }
}
