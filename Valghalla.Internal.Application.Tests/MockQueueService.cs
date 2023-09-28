using Valghalla.Application.Queue;

namespace Valghalla.Internal.Application.Tests
{
    internal class MockQueueService : IQueueService
    {
        public Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class
        {
            return Task.CompletedTask;
        }

        public Task PublishJobAsync<T>(T message, CancellationToken cancellationToken) where T : class
        {
            return Task.CompletedTask;
        }

        public Task PublishLoggedJobAsync<T>(Guid logId, T message, CancellationToken cancellationToken) where T : class
        {
            return Task.CompletedTask;
        }
    }
}
