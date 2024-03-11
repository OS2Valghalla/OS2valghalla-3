using MassTransit;
using Microsoft.Extensions.Options;
using Valghalla.Application.Communication;
using Valghalla.Application.Queue;
using Valghalla.Application.Tenant;
using Valghalla.Worker.Infrastructure.Models;
using Valghalla.Worker.Infrastructure.Modules.Job.Repositories;
using Valghalla.Worker.Infrastructure.Modules.Tasks.Repositories;
using Valghalla.Worker.QueueMessages;

namespace Valghalla.Worker.Jobs
{
    internal class TaskGetInvitationReminderJobConsumerDefinition : ConsumerDefinition<TaskGetInvitationReminderJobConsumer>
    {
        private readonly IOptions<JobConfiguration> jobConfigurationOptions;

        public TaskGetInvitationReminderJobConsumerDefinition(IOptions<JobConfiguration> jobConfigurationOptions)
        {
            this.jobConfigurationOptions = jobConfigurationOptions;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<TaskGetInvitationReminderJobConsumer> consumerConfigurator)
        {
            var config = jobConfigurationOptions.Value.TaskInvitationReminderJob;

            consumerConfigurator.Options<JobOptions<QueueMessage<TaskGetInvitationReminderJobMessage>>>(options =>
                options
                    .SetRetry(retryConfig => retryConfig.Interval(config.Retry, TimeSpan.FromHours(config.RetryPeriod)))
                    .SetJobTimeout(TimeSpan.FromHours(config.Timeout))
                    .SetConcurrentJobLimit(config.ConcurrentLimit));
        }
    }

    internal class TaskGetInvitationReminderJobConsumer : JobConsumerBase<TaskGetInvitationReminderJobConsumer>, IJobConsumer<QueueMessage<TaskGetInvitationReminderJobMessage>>
    {
        protected override Guid JobDefinitionId => new("cbc8a76b-f558-476e-8488-dcd8c9f5f70d");

        private readonly ICommunicationService communicationService;
        private readonly ITaskAssignmentQueryRepository taskAssignmentQueryRepository;

        public TaskGetInvitationReminderJobConsumer(
            ILogger<TaskGetInvitationReminderJobConsumer> logger,
            ITenantContextProvider tenantContextProvider,
            IJobQueryRepository jobQueryRepository,
            IJobCommandRepository jobCommandRepository,
            ITaskAssignmentQueryRepository taskAssignmentQueryRepository,
            ICommunicationService communicationService) : base(logger, tenantContextProvider, jobQueryRepository, jobCommandRepository)
        {
            this.taskAssignmentQueryRepository = taskAssignmentQueryRepository;
            this.communicationService = communicationService;
        }

        public async Task Run(JobContext<QueueMessage<TaskGetInvitationReminderJobMessage>> ctx)
        {
            var tenant = tenantContextProvider.CurrentTenant;

            logger.LogInformation(
                $"[{tenant.Name}]" +
                "Starting to check and send task invitation reminders...");

            var valid = await PrepareAsync(ctx.JobId, ctx.CancellationToken);
            if (!valid) return;

            try
            {
                await ExecuteAsync(ctx);

                logger.LogInformation(
                    $"[{tenant.Name}]" +
                    "DONE checking and sending task invitation reminders.");
            }
            catch (Exception ex)
            {
                logger.LogError(
                    $"[{tenant.Name}]" +
                    "An error occurred when checking and sending task invitation reminders -- {@ex}", ex);

                throw;
            }
            finally
            {
                await EndAsync(ctx.CancellationToken);
            }
        }

        private async Task ExecuteAsync(JobContext<QueueMessage<TaskGetInvitationReminderJobMessage>> ctx)
        {
            var taskAssignments = await taskAssignmentQueryRepository.GetInvitationReminderTaskAssignmentsAsync(ctx.CancellationToken);

            if (!taskAssignments.Any()) return;

            foreach (var taskAssignment in taskAssignments)
            {
                await communicationService.SendTaskInvitationReminderAsync(taskAssignment.ParticipantId, taskAssignment.Id, taskAssignment.InvitationDate, taskAssignment.InvitationReminderDate, taskAssignment.TaskDate, ctx.CancellationToken);
            }
        }
    }
}
