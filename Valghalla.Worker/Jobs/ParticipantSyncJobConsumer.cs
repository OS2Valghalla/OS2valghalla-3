using MassTransit;
using Microsoft.Extensions.Options;
using Valghalla.Application.Queue;
using Valghalla.Application.Tenant;
using Valghalla.Worker.Infrastructure.Models;
using Valghalla.Worker.Infrastructure.Modules.Election.Repositories;
using Valghalla.Worker.Infrastructure.Modules.Job.Repositories;
using Valghalla.Worker.QueueMessages;
using Valghalla.Worker.Services;

namespace Valghalla.Worker.Jobs
{
    internal class ParticipantSyncJobConsumerDefinition : ConsumerDefinition<ParticipantSyncJobConsumer>
    {
        private readonly IOptions<JobConfiguration> jobConfigurationOptions;

        public ParticipantSyncJobConsumerDefinition(IOptions<JobConfiguration> jobConfigurationOptions)
        {
            this.jobConfigurationOptions = jobConfigurationOptions;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ParticipantSyncJobConsumer> consumerConfigurator)
        {
            var config = jobConfigurationOptions.Value.ParticipantSyncJob;

            consumerConfigurator.Options<JobOptions<QueueMessage<ParticipantSyncJobMessage>>>(options =>
                options
                    .SetRetry(retryConfig => retryConfig.Interval(config.Retry, TimeSpan.FromHours(config.RetryPeriod)))
                    .SetJobTimeout(TimeSpan.FromHours(config.Timeout))
                    .SetConcurrentJobLimit(config.ConcurrentLimit));
        }
    }

    internal class ParticipantSyncJobConsumer : JobConsumerBase<ParticipantSyncJobConsumer>, IJobConsumer<QueueMessage<ParticipantSyncJobMessage>>
    {
        protected override Guid JobDefinitionId => new("d1b6cef2-a9b5-478c-b80e-ed1db7b8b74f");

        private readonly IOptions<JobConfiguration> jobConfigurationOptions;

        private readonly IElectionQueryRepository electionQueryRepository;
        private readonly IParticipantSyncService participantSyncService;

        public ParticipantSyncJobConsumer(
            ILogger<ParticipantSyncJobConsumer> logger,
            IOptions<JobConfiguration> jobConfigurationOptions,
            ITenantContextProvider tenantContextProvider,
            IJobQueryRepository jobQueryRepository,
            IJobCommandRepository jobCommandRepository,
            IElectionQueryRepository electionQueryRepository,
            IParticipantSyncService participantSyncService) : base(logger, tenantContextProvider, jobQueryRepository, jobCommandRepository)
        {
            this.jobConfigurationOptions = jobConfigurationOptions;

            this.electionQueryRepository = electionQueryRepository;
            this.participantSyncService = participantSyncService;
        }

        public async Task Run(JobContext<QueueMessage<ParticipantSyncJobMessage>> ctx)
        {
            var tenant = tenantContextProvider.CurrentTenant;

            logger.LogInformation(
                $"[{tenant.Name}]" +
                "Starting to sync and validate participant data from CPR service...");

            var valid = await PrepareAsync(ctx.JobId, ctx.CancellationToken);
            if (!valid) return;

            try
            {
                await ExecuteAsync(ctx);

                logger.LogInformation(
                    $"[{tenant.Name}]" +
                    "DONE syncing and validating participant data from CPR service.");
            }
            catch (Exception ex)
            {
                logger.LogError(
                    $"[{tenant.Name}]" +
                    "An error occurred when syncing and validating participant data from CPR service -- {@ex}", ex);

                throw;
            }
            finally
            {
                await EndAsync(ctx.CancellationToken);
            }
        }

        private async Task ExecuteAsync(JobContext<QueueMessage<ParticipantSyncJobMessage>> ctx)
        {
            var tenant = tenantContextProvider.CurrentTenant;
            var config = jobConfigurationOptions.Value;

            var elections = await electionQueryRepository.GetActiveElectionsAsync(ctx.CancellationToken);

            if (!elections.Any()) return;

            var electionIds = elections.Select(i => i.Id).ToArray();

            await participantSyncService.ExecuteAsync(
                tenant.Name,
                electionIds,
                config.ParticipantSyncJob.BatchSize,
                ctx.CancellationToken);
        }
    }
}
