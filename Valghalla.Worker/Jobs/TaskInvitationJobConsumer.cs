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
    internal class TaskInvitationJobConsumerDefinition : ConsumerDefinition<TaskInvitationJobConsumer>
    {
        private readonly IOptions<JobConfiguration> jobConfigurationOptions;

        public TaskInvitationJobConsumerDefinition(IOptions<JobConfiguration> jobConfigurationOptions)
        {
            this.jobConfigurationOptions = jobConfigurationOptions;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<TaskInvitationJobConsumer> consumerConfigurator)
        {
            var config = jobConfigurationOptions.Value.TaskNotificationJob;

            consumerConfigurator.Options<JobOptions<LoggedQueueMessage<TaskInvitationJobMessage>>>(options =>
                options
                    .SetRetry(retryConfig => retryConfig.Interval(config.Retry, TimeSpan.FromHours(config.RetryPeriod)))
                    .SetJobTimeout(TimeSpan.FromHours(config.Timeout))
                    .SetConcurrentJobLimit(config.ConcurrentLimit));
        }
    }

    internal class TaskInvitationJobConsumer : JobConsumerBase<TaskInvitationJobConsumer>, IJobConsumer<LoggedQueueMessage<TaskInvitationJobMessage>>
    {
        protected override Guid JobDefinitionId => new("593c3eca-7b5f-48f6-b0b5-37b56c4cf005");

        private readonly ITaskCommunicationService taskCommunicationService;
        private readonly ICommunicationLogRepository communicationLogRepository;

        public TaskInvitationJobConsumer(
            ILogger<TaskInvitationJobConsumer> logger,
            ITenantContextProvider tenantContextProvider,
            IJobQueryRepository jobQueryRepository,
            IJobCommandRepository jobCommandRepository,
            ITaskCommunicationService taskCommunicationService,
            ICommunicationLogRepository communicationLogRepository) : base(logger, tenantContextProvider, jobQueryRepository, jobCommandRepository)
        {
            this.taskCommunicationService = taskCommunicationService;
            this.communicationLogRepository = communicationLogRepository;
        }

        public async Task Run(JobContext<LoggedQueueMessage<TaskInvitationJobMessage>> ctx)
        {
            var tenant = tenantContextProvider.CurrentTenant;

            logger.LogInformation(
                $"[{tenant.Name}]" +
                "Starting to check and send task invitations...");

            var valid = await PrepareAsync(ctx.JobId, ctx.CancellationToken);
            if (!valid) return;

            try
            {
                await ExecuteAsync(ctx);

                logger.LogInformation(
                    $"[{tenant.Name}]" +
                    "DONE checking and sending task invitations.");
            }
            catch (Exception ex)
            {
                logger.LogError(
                    $"[{tenant.Name}]" +
                    "An error occurred when checking and sending task invitations -- {@ex}", ex);

                throw;
            }
            finally
            {
                await EndAsync(ctx.CancellationToken);
            }
        }

        private async Task ExecuteAsync(JobContext<LoggedQueueMessage<TaskInvitationJobMessage>> ctx)
        {
            try
            {
                await taskCommunicationService.SendTaskInvitationAsync(
                    ctx.Job.LogId,
                    ctx.Job.Data.ParticipantId,
                    ctx.Job.Data.TaskAssignmentId, ctx.CancellationToken);
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
