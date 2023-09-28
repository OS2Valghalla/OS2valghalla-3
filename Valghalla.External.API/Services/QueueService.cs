using MassTransit;
using MassTransit.Contracts.JobService;
using Valghalla.Application.Queue;
using Valghalla.Application.Tenant;

namespace Valghalla.External.API.Services
{
    internal class QueueService : IQueueService
    {
        private readonly ILogger<QueueService> logger;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly ITenantContextProvider tenantContextProvider;

        public QueueService(
            ILogger<QueueService> logger,
            IPublishEndpoint publishEndpoint,
            ITenantContextProvider tenantContextProvider)
        {
            this.logger = logger;
            this.publishEndpoint = publishEndpoint;
            this.tenantContextProvider = tenantContextProvider;
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class
        {
            try
            {
                var domain = tenantContextProvider.CurrentTenant.ExternalDomain;
                var queueMessage = new QueueMessage<T>(domain, message);

                await publishEndpoint.Publish(queueMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError("An error occurred when firing queue message -- {@ex}", ex);
            }
        }

        public async Task PublishJobAsync<T>(T message, CancellationToken cancellationToken) where T : class
        {
            try
            {
                var domain = tenantContextProvider.CurrentTenant.ExternalDomain;
                var queueMessage = new QueueMessage<T>(domain, message);

                await publishEndpoint.Publish<SubmitJob<QueueMessage<T>>>(new
                {
                    JobId = Guid.NewGuid(),
                    Job = queueMessage
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError("An error occurred when firing queue message -- {@ex}", ex);
            }
        }

        public async Task PublishLoggedJobAsync<T>(Guid logId, T message, CancellationToken cancellationToken) where T : class
        {
            try
            {
                var domain = tenantContextProvider.CurrentTenant.ExternalDomain;
                var queueMessage = new LoggedQueueMessage<T>(domain, message, logId);

                await publishEndpoint.Publish<SubmitJob<LoggedQueueMessage<T>>>(new
                {
                    JobId = Guid.NewGuid(),
                    Job = queueMessage
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError("An error occurred when firing queue message -- {@ex}", ex);
            }
        }
    }
}
