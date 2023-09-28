namespace Valghalla.Application.DigitalPost
{
    public interface IDigitalPostService
    {
        Task SendAsync(Guid messageUUID, DigitalPostMessage message, CancellationToken cancellationToken);
    }
}
