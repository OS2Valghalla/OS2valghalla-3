using MassTransit;
using MediatR;
using Valghalla.Application.Queue;
using Valghalla.Application.Queue.Messages;
using Valghalla.External.Application.Modules.App.Commands;

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
        private readonly ISender sender;

        public ExternalUserClearCacheConsumer(ISender sender)
        {
            this.sender = sender;
        }

        public async Task Consume(ConsumeContext<QueueMessage<ExternalUserClearCacheMessage>> context)
        {
            var command = new ClearExternalUserCacheCommand(context.Message.Data.CprNumbers);
            await sender.Send(command);
        }
    }
}
