namespace Valghalla.Application.SMS
{
    public interface ITextMessageService
    {
        Task SendTextMessageAsync(TextMessage value, CancellationToken cancellationToken);
    }
}
