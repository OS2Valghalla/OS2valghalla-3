using MassTransit;
using Microsoft.Extensions.Options;
using Valghalla.Application.Communication;
using Valghalla.Application.Queue;
using Valghalla.Application.Queue.Messages;
using Valghalla.Application.Tenant;
using Valghalla.Worker.Infrastructure.Models;
using Valghalla.Worker.Infrastructure.Modules.Election.Repositories;
using Valghalla.Worker.Infrastructure.Modules.Job.Repositories;
using Valghalla.Worker.Infrastructure.Modules.Tasks.Repositories;
using Valghalla.Worker.Services;

namespace Valghalla.Worker.Jobs
{
    internal class ElectionActivationJobConsumerDefinition : ConsumerDefinition<ElectionActivationJobConsumer>
    {
        private readonly IOptions<JobConfiguration> jobConfigurationOptions;

        public ElectionActivationJobConsumerDefinition(IOptions<JobConfiguration> jobConfigurationOptions)
        {
            this.jobConfigurationOptions = jobConfigurationOptions;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ElectionActivationJobConsumer> consumerConfigurator)
        {
            var config = jobConfigurationOptions.Value.ElectionActivationJob;

            consumerConfigurator.Options<JobOptions<QueueMessage<ElectionActivationJobMessage>>>(options =>
                options
                    .SetRetry(retryConfig => retryConfig.Interval(config.Retry, TimeSpan.FromHours(config.RetryPeriod)))
                    .SetJobTimeout(TimeSpan.FromHours(config.Timeout))
                    .SetConcurrentJobLimit(config.ConcurrentLimit));
        }
    }

    internal class ElectionActivationJobConsumer : JobConsumerBase<ElectionActivationJobConsumer>, IJobConsumer<QueueMessage<ElectionActivationJobMessage>>
    {
        protected override Guid JobDefinitionId => new("c3757749-622e-4233-b608-916faa0552ef");
        private readonly IOptions<JobConfiguration> jobConfigurationOptions;

        private readonly ICommunicationService communicationService;
        private readonly IElectionQueryRepository electionQueryRepository;
        private readonly IParticipantSyncService participantSyncService;
        private readonly ITaskAssignmentQueryRepository taskAssignmentQueryRepository;

        public ElectionActivationJobConsumer(
            ICommunicationService communicationService,
            IOptions<JobConfiguration> jobConfigurationOptions,
            ILogger<ElectionActivationJobConsumer> logger,
            ITenantContextProvider tenantContextProvider,
            IJobQueryRepository jobQueryRepository,
            IJobCommandRepository jobCommandRepository,
            IElectionQueryRepository electionQueryRepository,
            IParticipantSyncService participantSyncService,
            ITaskAssignmentQueryRepository taskAssignmentQueryRepository) : base(logger, tenantContextProvider, jobQueryRepository, jobCommandRepository)
        {
            this.communicationService = communicationService;
            this.jobConfigurationOptions = jobConfigurationOptions;
            this.electionQueryRepository = electionQueryRepository;
            this.participantSyncService = participantSyncService;
            this.taskAssignmentQueryRepository = taskAssignmentQueryRepository;
        }

        public async Task Run(JobContext<QueueMessage<ElectionActivationJobMessage>> ctx)
        {
            var tenant = tenantContextProvider.CurrentTenant;

            logger.LogInformation(
                $"[{tenant.Name}]" +
                "Starting to check, sync CPR and send notifications...");

            var valid = await PrepareAsync(ctx.JobId, ctx.CancellationToken);
            if (!valid) return;

            try
            {
                await ExecuteAsync(ctx);

                logger.LogInformation(
                    $"[{tenant.Name}]" +
                    "DONE chekcing, syncing CPR and sending notifications.");
            }
            catch (Exception ex)
            {
                logger.LogError(
                    $"[{tenant.Name}]" +
                    "An error occurred when chekcing, syncing CPR and sending notifications-- {@ex}", ex);

                throw;
            }
            finally
            {
                await EndAsync(ctx.CancellationToken);
            }
        }

        private async Task ExecuteAsync(JobContext<QueueMessage<ElectionActivationJobMessage>> ctx)
        {
            var tenant = tenantContextProvider.CurrentTenant;
            var config = jobConfigurationOptions.Value;
            var activatedElectionId = ctx.Job.Data.ElectionId;

            var elections = await electionQueryRepository.GetActiveElectionsAsync(ctx.CancellationToken);
            if (!elections.Any(i => i.Id == activatedElectionId)) return;

            await participantSyncService.ExecuteAsync(
                tenant.Name,
                new[] { activatedElectionId },
                config.ParticipantSyncJob.BatchSize,
                ctx.CancellationToken);

            var unsentInvitations = await taskAssignmentQueryRepository.GetUnsentInvitationTaskAssignmentsAsync(activatedElectionId, ctx.CancellationToken);

            foreach (var taskAssignment in unsentInvitations)
            {
                await communicationService.SendTaskInvitationAsync(taskAssignment.ParticipantId, taskAssignment.Id, ctx.CancellationToken);
            }

            var unsentRegistrations = await taskAssignmentQueryRepository.GetUnsentRegistrationTaskAssignmentsAsync(activatedElectionId, ctx.CancellationToken);

            foreach (var taskAssignment in unsentRegistrations)
            {
                await communicationService.SendTaskRegistrationAsync(taskAssignment.ParticipantId, taskAssignment.Id, ctx.CancellationToken);
            }
        }
    }
}
