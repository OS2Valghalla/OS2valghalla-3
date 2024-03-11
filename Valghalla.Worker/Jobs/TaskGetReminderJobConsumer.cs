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
    internal class TaskGetReminderJobConsumerDefinition : ConsumerDefinition<TaskGetReminderJobConsumer>
    {
        private readonly IOptions<JobConfiguration> jobConfigurationOptions;

        public TaskGetReminderJobConsumerDefinition(IOptions<JobConfiguration> jobConfigurationOptions)
        {
            this.jobConfigurationOptions = jobConfigurationOptions;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<TaskGetReminderJobConsumer> consumerConfigurator)
        {
            var config = jobConfigurationOptions.Value.TaskReminderJob;

            consumerConfigurator.Options<JobOptions<QueueMessage<TaskGetReminderJobMessage>>>(options =>
                options
                    .SetRetry(retryConfig => retryConfig.Interval(config.Retry, TimeSpan.FromHours(config.RetryPeriod)))
                    .SetJobTimeout(TimeSpan.FromHours(config.Timeout))
                    .SetConcurrentJobLimit(config.ConcurrentLimit));
        }
    }

    internal class TaskGetReminderJobConsumer : JobConsumerBase<TaskGetReminderJobConsumer>, IJobConsumer<QueueMessage<TaskGetReminderJobMessage>>
    {
        protected override Guid JobDefinitionId => new("fe383dc9-3b28-44c8-8526-0b55bbbbaff3");

        private readonly ICommunicationService communicationService;
        private readonly ITaskAssignmentQueryRepository taskAssignmentQueryRepository;

        public TaskGetReminderJobConsumer(
            ILogger<TaskGetReminderJobConsumer> logger,
            ITenantContextProvider tenantContextProvider,
            IJobQueryRepository jobQueryRepository,
            IJobCommandRepository jobCommandRepository,
            IQueueService queueService,
            ITaskAssignmentQueryRepository taskAssignmentQueryRepository,
            ICommunicationService communicationService) : base(logger, tenantContextProvider, jobQueryRepository, jobCommandRepository)
        {
            this.taskAssignmentQueryRepository = taskAssignmentQueryRepository;
            this.communicationService = communicationService;
        }

        public async Task Run(JobContext<QueueMessage<TaskGetReminderJobMessage>> ctx)
        {
            var tenant = tenantContextProvider.CurrentTenant;

            logger.LogInformation(
                $"[{tenant.Name}]" +
                "Starting to check and send task reminders...");

            var valid = await PrepareAsync(ctx.JobId, ctx.CancellationToken);
            if (!valid) return;

            try
            {
                await ExecuteAsync(ctx);

                logger.LogInformation(
                    $"[{tenant.Name}]" +
                    "DONE checking and sending task reminders.");
            }
            catch (Exception ex)
            {
                logger.LogError(
                    $"[{tenant.Name}]" +
                    "An error occurred when checking and sending task reminders -- {@ex}", ex);

                throw;
            }
            finally
            {
                await EndAsync(ctx.CancellationToken);
            }
        }

        private async Task ExecuteAsync(JobContext<QueueMessage<TaskGetReminderJobMessage>> ctx)
        {
            var taskAssignments = await taskAssignmentQueryRepository.GetReminderTaskAssignmentsAsync(ctx.CancellationToken);

            if (!taskAssignments.Any()) return;

            foreach (var taskAssignment in taskAssignments)
            {
                await communicationService.SendTaskReminderAsync(taskAssignment.ParticipantId, taskAssignment.Id, taskAssignment.ReminderDate, taskAssignment.TaskDate, ctx.CancellationToken);
            }
        }
    }
}
