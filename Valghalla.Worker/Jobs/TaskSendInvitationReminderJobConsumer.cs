using MassTransit;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Valghalla.Application.Communication;
using Valghalla.Application.Queue;
using Valghalla.Application.Queue.Messages;
using Valghalla.Application.Tenant;
using Valghalla.Worker.Exceptions;
using Valghalla.Worker.Infrastructure.Models;
using Valghalla.Worker.Infrastructure.Modules.Job.Repositories;
using Valghalla.Worker.Services;

namespace Valghalla.Worker.Jobs
{
    internal class TaskSendInvitationReminderJobConsumerDefinition : ConsumerDefinition<TaskSendInvitationReminderJobConsumer>
    {
        private readonly IOptions<JobConfiguration> jobConfigurationOptions;

        public TaskSendInvitationReminderJobConsumerDefinition(IOptions<JobConfiguration> jobConfigurationOptions)
        {
            this.jobConfigurationOptions = jobConfigurationOptions;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<TaskSendInvitationReminderJobConsumer> consumerConfigurator)
        {
            var config = jobConfigurationOptions.Value.TaskNotificationJob;

            consumerConfigurator.Options<JobOptions<LoggedQueueMessage<TaskSendInvitationReminderJobMessage>>>(options =>
                options
                    .SetRetry(retryConfig => retryConfig.Interval(config.Retry, TimeSpan.FromHours(config.RetryPeriod)))
                    .SetJobTimeout(TimeSpan.FromHours(config.Timeout))
                    .SetConcurrentJobLimit(config.ConcurrentLimit));
        }
    }

    internal class TaskSendInvitationReminderJobConsumer : JobConsumerBase<TaskSendInvitationReminderJobConsumer>, IJobConsumer<LoggedQueueMessage<TaskSendInvitationReminderJobMessage>>
    {
        protected override Guid JobDefinitionId => new("fee36f06-3849-45ce-9e3b-947812862157");

        private readonly ITaskCommunicationService taskCommunicationService;
        private readonly ICommunicationLogRepository communicationLogRepository;

        public TaskSendInvitationReminderJobConsumer(
            ILogger<TaskSendInvitationReminderJobConsumer> logger,
            ITenantContextProvider tenantContextProvider,
            IJobQueryRepository jobQueryRepository,
            IJobCommandRepository jobCommandRepository,
            ITaskCommunicationService taskCommunicationService,
            ICommunicationLogRepository communicationLogRepository) : base(logger, tenantContextProvider, jobQueryRepository, jobCommandRepository)
        {
            this.taskCommunicationService = taskCommunicationService;
            this.communicationLogRepository = communicationLogRepository;
        }

        public async Task Run(JobContext<LoggedQueueMessage<TaskSendInvitationReminderJobMessage>> ctx)
        {
            var tenant = tenantContextProvider.CurrentTenant;

            logger.LogInformation(
                $"[{tenant.Name}]" +
                "Starting to check and send task invitation reminder...");

            var valid = await PrepareAsync(ctx.JobId, ctx.CancellationToken);
            if (!valid) return;

            try
            {
                await ExecuteAsync(ctx);

                logger.LogInformation(
                    $"[{tenant.Name}]" +
                    "DONE checking and sending task invitation reminder.");
            }
            catch (Exception ex)
            {
                logger.LogError(
                    $"[{tenant.Name}]" +
                    "An error occurred when checking and sending task invitation reminder -- {@ex}", ex);

                throw;
            }
            finally
            {
                await EndAsync(ctx.CancellationToken);
            }
        }

        private async Task ExecuteAsync(JobContext<LoggedQueueMessage<TaskSendInvitationReminderJobMessage>> ctx)
        {
            try
            {
                await taskCommunicationService.SendTaskInvitationReminderAsync(ctx.Job.LogId, ctx.Job.Data.ParticipantId, ctx.Job.Data.TaskAssignmentId, ctx.CancellationToken);
            }
            catch (ExternalException ex)
            {
                var error = JsonSerializer.Serialize(new
                {
                    Message = ex.InnerException!.Message,
                    Details = ex.Details,
                    StackTrace = ex.StackTrace,
                });

                await communicationLogRepository.UpdateCommunicationLogErrorAsync(
                    ctx.Job.LogId,
                    ex.Content,
                    ex.ShortContent,
                    error,
                    ctx.CancellationToken);

                throw ex.InnerException;
            }
            catch (Exception ex)
            {
                var error = JsonSerializer.Serialize(new
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                });

                await communicationLogRepository.UpdateCommunicationLogErrorAsync(ctx.Job.LogId, error, ctx.CancellationToken);

                throw;
            }
        }
    }
}
