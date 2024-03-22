using MassTransit;
using Valghalla.Application.Cache;
using Valghalla.Application.Queue;
using Valghalla.Application.Queue.Messages;
using Valghalla.Application.User;

namespace Valghalla.External.API.Queues
{
    internal class ExternalUserClearCacheConsumerDefinition : ConsumerDefinition<ExternalUserClearCacheConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ExternalUserClearCacheConsumer> consumerConfigurator)
        {
            endpointConfigurator.ConcurrentMessageLimit = 10;
        }
    }

    public class ExternalUserClearCacheConsumer : IConsumer<QueueMessage<ExternalUserClearCacheMessage>>
    {
        private readonly ITenantMemoryCache tenantMemoryCache;

        public ExternalUserClearCacheConsumer(ITenantMemoryCache tenantMemoryCache)
        {
            this.tenantMemoryCache = tenantMemoryCache;
        }

        public async Task Consume(ConsumeContext<QueueMessage<ExternalUserClearCacheMessage>> context)
        {
            var cprNumbers = context.Message.Data.CprNumbers;

            foreach (var cpr in cprNumbers)
            {
                var cacheKey = UserContext.GetCacheKey(cpr);
                tenantMemoryCache.Remove(cacheKey);
            }

            await Task.CompletedTask;
        }
    }
}
