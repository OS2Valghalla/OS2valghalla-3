namespace Valghalla.Application.Queue
{
    public interface IQueueService
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class;
        Task PublishJobAsync<T>(T message, CancellationToken cancellationToken) where T : class;
        Task PublishLoggedJobAsync<T>(Guid logId, T message, CancellationToken cancellationToken) where T : class;
    }
}
