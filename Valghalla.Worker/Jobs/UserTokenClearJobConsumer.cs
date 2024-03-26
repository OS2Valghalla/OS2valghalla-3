using MassTransit;
using Microsoft.Extensions.Options;
using Valghalla.Application.Auth;
using Valghalla.Application.Queue;
using Valghalla.Application.Tenant;
using Valghalla.Worker.Infrastructure.Models;
using Valghalla.Worker.Infrastructure.Modules.Job.Repositories;
using Valghalla.Worker.QueueMessages;

namespace Valghalla.Worker.Jobs
{
    internal class UserTokenClearJobConsumerDefinition : ConsumerDefinition<UserTokenClearJobConsumer>
    {
        private readonly IOptions<JobConfiguration> jobConfigurationOptions;

        public UserTokenClearJobConsumerDefinition(IOptions<JobConfiguration> jobConfigurationOptions)
        {
            this.jobConfigurationOptions = jobConfigurationOptions;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<UserTokenClearJobConsumer> consumerConfigurator)
        {
            var config = jobConfigurationOptions.Value.UserTokenClearJob;

            consumerConfigurator.Options<JobOptions<QueueMessage<UserTokenClearJobMessage>>>(options =>
                options
                    .SetRetry(retryConfig => retryConfig.Interval(config.Retry, TimeSpan.FromHours(config.RetryPeriod)))
                    .SetJobTimeout(TimeSpan.FromHours(config.Timeout))
                    .SetConcurrentJobLimit(config.ConcurrentLimit));
        }
    }

    internal class UserTokenClearJobConsumer : JobConsumerBase<UserTokenClearJobConsumer>, IJobConsumer<QueueMessage<UserTokenClearJobMessage>>
    {
        protected override Guid JobDefinitionId => new("395610d2-f95c-489e-81eb-8353e0939202");

        private readonly IUserTokenRepository userTokenRepository;

        public UserTokenClearJobConsumer(
            ILogger<UserTokenClearJobConsumer> logger,
            ITenantContextProvider tenantContextProvider,
            IJobQueryRepository jobQueryRepository,
            IJobCommandRepository jobCommandRepository,
            IUserTokenRepository userTokenRepository) : base(logger, tenantContextProvider, jobQueryRepository, jobCommandRepository)
        {
            this.userTokenRepository = userTokenRepository;
        }

        public async Task Run(JobContext<QueueMessage<UserTokenClearJobMessage>> ctx)
        {
            var tenant = tenantContextProvider.CurrentTenant;

            logger.LogInformation(
                $"[{tenant.Name}]" +
                "Starting to check and clear expired user tokens...");

            var valid = await PrepareAsync(ctx.JobId, ctx.CancellationToken);
            if (!valid) return;

            try
            {
                await userTokenRepository.RemoveExpiredUserTokensAsync(ctx.CancellationToken);

                logger.LogInformation(
                    $"[{tenant.Name}]" +
                    "DONE checking and clearing expired user tokens.");
            }
            catch (Exception ex)
            {
                logger.LogError(
                    $"[{tenant.Name}]" +
                    "An error occurred when checking and clearing expired user tokens -- {@ex}", ex);

                throw;
            }
            finally
            {
                await EndAsync(ctx.CancellationToken);
            }
        }
    }
}
