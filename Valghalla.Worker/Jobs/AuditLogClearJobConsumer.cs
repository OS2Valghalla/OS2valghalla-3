using MassTransit;
using Microsoft.Extensions.Options;
using Valghalla.Application.Queue;
using Valghalla.Application.Tenant;
using Valghalla.Worker.Infrastructure.Models;
using Valghalla.Worker.Infrastructure.Modules.AuditLog.Repositories;
using Valghalla.Worker.Infrastructure.Modules.Job.Repositories;
using Valghalla.Worker.QueueMessages;

namespace Valghalla.Worker.Jobs
{
    internal class AuditLogClearJobConsumerDefinition : ConsumerDefinition<AuditLogClearJobConsumer>
    {
        private readonly IOptions<JobConfiguration> jobConfigurationOptions;

        public AuditLogClearJobConsumerDefinition(IOptions<JobConfiguration> jobConfigurationOptions)
        {
            this.jobConfigurationOptions = jobConfigurationOptions;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<AuditLogClearJobConsumer> consumerConfigurator)
        {
            var config = jobConfigurationOptions.Value.AuditLogClearJob;

            consumerConfigurator.Options<JobOptions<QueueMessage<AuditLogClearJobMessage>>>(options =>
                options
                    .SetRetry(retryConfig => retryConfig.Interval(config.Retry, TimeSpan.FromHours(config.RetryPeriod)))
                    .SetJobTimeout(TimeSpan.FromHours(config.Timeout))
                    .SetConcurrentJobLimit(config.ConcurrentLimit));
        }
    }

    internal class AuditLogClearJobConsumer : JobConsumerBase<AuditLogClearJobConsumer>, IJobConsumer<QueueMessage<AuditLogClearJobMessage>>
    {
        protected override Guid JobDefinitionId => new("cdd1b73e-4d50-4fd4-813f-c17fdcede510");

        private readonly IOptions<JobConfiguration> jobConfigurationOptions;
        private readonly IAuditLogCommandRepository auditLogCommandRepository;

        public AuditLogClearJobConsumer(
            ILogger<AuditLogClearJobConsumer> logger,
            ITenantContextProvider tenantContextProvider,
            IJobQueryRepository jobQueryRepository,
            IJobCommandRepository jobCommandRepository,
            IOptions<JobConfiguration> jobConfigurationOptions,
            IAuditLogCommandRepository auditLogCommandRepository) : base(logger, tenantContextProvider, jobQueryRepository, jobCommandRepository)
        {
            this.jobConfigurationOptions = jobConfigurationOptions;
            this.auditLogCommandRepository = auditLogCommandRepository;
        }

        public async Task Run(JobContext<QueueMessage<AuditLogClearJobMessage>> ctx)
        {
            var tenant = tenantContextProvider.CurrentTenant;

            logger.LogInformation(
                $"[{tenant.Name}]" +
                "Starting to check and clear audit logs...");

            var valid = await PrepareAsync(ctx.JobId, ctx.CancellationToken);
            if (!valid) return;

            try
            {
                await ExecuteAsync(ctx);

                logger.LogInformation(
                    $"[{tenant.Name}]" +
                    "DONE checking and clearing audit logs.");
            }
            catch (Exception ex)
            {
                logger.LogError(
                    $"[{tenant.Name}]" +
                    "An error occurred when checking and clearing audit logs -- {@ex}", ex);

                throw;
            }
            finally
            {
                await EndAsync(ctx.CancellationToken);
            }
        }

        private async Task ExecuteAsync(JobContext<QueueMessage<AuditLogClearJobMessage>> ctx)
        {
            var maxTime = jobConfigurationOptions.Value.AuditLogClearJob.MaxAvailableTime;
            var time = DateTime.UtcNow.AddMonths(maxTime * -1);

            await auditLogCommandRepository.ClearAuditLogsAsync(time, ctx.CancellationToken);
        }
    }
}
