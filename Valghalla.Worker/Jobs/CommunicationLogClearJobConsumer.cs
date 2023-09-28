using MassTransit;
using Microsoft.Extensions.Options;
using Valghalla.Application.Queue;
using Valghalla.Application.Tenant;
using Valghalla.Worker.Infrastructure.Models;
using Valghalla.Worker.Infrastructure.Modules.Communication.Repositories;
using Valghalla.Worker.Infrastructure.Modules.Job.Repositories;
using Valghalla.Worker.QueueMessages;

namespace Valghalla.Worker.Jobs
{
    internal class CommunicationLogClearJobConsumerDefinition : ConsumerDefinition<CommunicationLogClearJobConsumer>
    {
        private readonly IOptions<JobConfiguration> jobConfigurationOptions;

        public CommunicationLogClearJobConsumerDefinition(IOptions<JobConfiguration> jobConfigurationOptions)
        {
            this.jobConfigurationOptions = jobConfigurationOptions;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CommunicationLogClearJobConsumer> consumerConfigurator)
        {
            var config = jobConfigurationOptions.Value.CommunicationLogClearJob;

            consumerConfigurator.Options<JobOptions<QueueMessage<CommunicationLogClearJobMessage>>>(options =>
                options
                    .SetRetry(retryConfig => retryConfig.Interval(config.Retry, TimeSpan.FromHours(config.RetryPeriod)))
                    .SetJobTimeout(TimeSpan.FromHours(config.Timeout))
                    .SetConcurrentJobLimit(config.ConcurrentLimit));
        }
    }

    internal class CommunicationLogClearJobConsumer : JobConsumerBase<CommunicationLogClearJobConsumer>, IJobConsumer<QueueMessage<CommunicationLogClearJobMessage>>
    {
        protected override Guid JobDefinitionId => new("86a3387b-0777-4045-9826-07aa1b260e0b");

        private readonly IOptions<JobConfiguration> jobConfigurationOptions;
        private readonly ICommunicationLogCommandRepository communicationLogCommandRepository;

        public CommunicationLogClearJobConsumer(
            ILogger<CommunicationLogClearJobConsumer> logger,
            ITenantContextProvider tenantContextProvider,
            IJobQueryRepository jobQueryRepository,
            IJobCommandRepository jobCommandRepository,
            IOptions<JobConfiguration> jobConfigurationOptions,
            ICommunicationLogCommandRepository communicationLogCommandRepository) : base(logger, tenantContextProvider, jobQueryRepository, jobCommandRepository)
        {
            this.jobConfigurationOptions = jobConfigurationOptions;
            this.communicationLogCommandRepository = communicationLogCommandRepository;
        }

        public async Task Run(JobContext<QueueMessage<CommunicationLogClearJobMessage>> ctx)
        {
            var tenant = tenantContextProvider.CurrentTenant;

            logger.LogInformation(
                $"[{tenant.Name}]" +
                "Starting to check and clear communication logs...");

            var valid = await PrepareAsync(ctx.JobId, ctx.CancellationToken);
            if (!valid) return;

            try
            {
                await ExecuteAsync(ctx);

                logger.LogInformation(
                    $"[{tenant.Name}]" +
                    "DONE checking and clearing communication logs.");
            }
            catch (Exception ex)
            {
                logger.LogError(
                    $"[{tenant.Name}]" +
                    "An error occurred when checking and clearing communication logs -- {@ex}", ex);

                throw;
            }
            finally
            {
                await EndAsync(ctx.CancellationToken);
            }
        }

        private async Task ExecuteAsync(JobContext<QueueMessage<CommunicationLogClearJobMessage>> ctx)
        {
            var maxTime = jobConfigurationOptions.Value.CommunicationLogClearJob.MaxAvailableTime;
            var time = DateTime.UtcNow.AddMonths(maxTime * -1);

            await communicationLogCommandRepository.ClearCommunicationLogsAsync(time, ctx.CancellationToken);
        }
    }
}
