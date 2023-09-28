using MassTransit;
using Microsoft.Extensions.Options;
using Valghalla.Application.Queue;
using Valghalla.Application.Tenant;
using Valghalla.Worker.Infrastructure.Models;
using Valghalla.Worker.Infrastructure.Modules.Election.Repositories;
using Valghalla.Worker.Infrastructure.Modules.Job.Repositories;
using Valghalla.Worker.QueueMessages;

namespace Valghalla.Worker.Jobs
{
    internal class ElectionDeactivationJobConsumerDefinition : ConsumerDefinition<ElectionDeactivationJobConsumer>
    {
        private readonly IOptions<JobConfiguration> jobConfigurationOptions;

        public ElectionDeactivationJobConsumerDefinition(IOptions<JobConfiguration> jobConfigurationOptions)
        {
            this.jobConfigurationOptions = jobConfigurationOptions;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ElectionDeactivationJobConsumer> consumerConfigurator)
        {
            var config = jobConfigurationOptions.Value.ElectionDeactivationJob;

            consumerConfigurator.Options<JobOptions<QueueMessage<ElectionDeactivationJobMessage>>>(options =>
                options
                    .SetRetry(retryConfig => retryConfig.Interval(config.Retry, TimeSpan.FromHours(config.RetryPeriod)))
                    .SetJobTimeout(TimeSpan.FromHours(config.Timeout))
                    .SetConcurrentJobLimit(config.ConcurrentLimit));
        }
    }

    internal class ElectionDeactivationJobConsumer : JobConsumerBase<ElectionDeactivationJobConsumer>, IJobConsumer<QueueMessage<ElectionDeactivationJobMessage>>
    {
        protected override Guid JobDefinitionId => new("8cc1a8f3-1fb7-45f9-b75d-a09b2ea2af99");

        private readonly IElectionQueryRepository electionQueryRepository;
        private readonly IElectionCommandRepository electionCommandRepository;

        public ElectionDeactivationJobConsumer(
            ILogger<ElectionDeactivationJobConsumer> logger,
            ITenantContextProvider tenantContextProvider,
            IJobQueryRepository jobQueryRepository,
            IJobCommandRepository jobCommandRepository,
            IElectionQueryRepository electionQueryRepository,
            IElectionCommandRepository electionCommandRepository) : base(logger, tenantContextProvider, jobQueryRepository, jobCommandRepository)
        {
            this.electionQueryRepository = electionQueryRepository;
            this.electionCommandRepository = electionCommandRepository;
        }

        public async Task Run(JobContext<QueueMessage<ElectionDeactivationJobMessage>> ctx)
        {
            var tenant = tenantContextProvider.CurrentTenant;

            logger.LogInformation(
                $"[{tenant.Name}]" +
                "Starting to check and deactivate elections...");

            var valid = await PrepareAsync(ctx.JobId, ctx.CancellationToken);
            if (!valid) return;

            try
            {
                await ExecuteAsync(ctx);

                logger.LogInformation(
                    $"[{tenant.Name}]" +
                    "DONE checking and deactivating elections.");
            }
            catch (Exception ex)
            {
                logger.LogError(
                    $"[{tenant.Name}]" +
                    "An error occurred when checking and deactivating elections -- {@ex}", ex);

                throw;
            }
            finally
            {
                await EndAsync(ctx.CancellationToken);
            }
        }

        private async Task ExecuteAsync(JobContext<QueueMessage<ElectionDeactivationJobMessage>> ctx)
        {
            var now = DateTime.UtcNow;

            var elections = await electionQueryRepository.GetActiveElectionsAsync(ctx.CancellationToken);
            var electionsToDeactivate = elections
                .Where(i => i.ElectionEndDate < now)
                .ToArray();

            if (!electionsToDeactivate.Any()) return;

            await electionCommandRepository.DeactivateElections(
                electionsToDeactivate.Select(i => i.Id),
                ctx.CancellationToken);
        }
    }
}
