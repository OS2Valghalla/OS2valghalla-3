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
    internal class TaskInvitationRetractedJobConsumerDefinition : ConsumerDefinition<TaskInvitationRetractedJobConsumer>
    {
        private readonly IOptions<JobConfiguration> jobConfigurationOptions;

        public TaskInvitationRetractedJobConsumerDefinition(IOptions<JobConfiguration> jobConfigurationOptions)
        {
            this.jobConfigurationOptions = jobConfigurationOptions;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<TaskInvitationRetractedJobConsumer> consumerConfigurator)
        {
            var config = jobConfigurationOptions.Value.TaskNotificationJob;

            consumerConfigurator.Options<JobOptions<LoggedQueueMessage<TaskInvitationRetractedJobMessage>>>(options =>
                options
                    .SetRetry(retryConfig => retryConfig.Interval(config.Retry, TimeSpan.FromHours(config.RetryPeriod)))
                    .SetJobTimeout(TimeSpan.FromHours(config.Timeout))
                    .SetConcurrentJobLimit(config.ConcurrentLimit));
        }
    }

    internal class TaskInvitationRetractedJobConsumer : JobConsumerBase<TaskInvitationRetractedJobConsumer>, IJobConsumer<LoggedQueueMessage<TaskInvitationRetractedJobMessage>>
    {
        protected override Guid JobDefinitionId => new("593c3eca-7b5f-48f6-b0b5-37b56c4cf005");

        private readonly ITaskCommunicationService taskCommunicationService;
        private readonly ICommunicationLogRepository communicationLogRepository;

        public TaskInvitationRetractedJobConsumer(
            ILogger<TaskInvitationRetractedJobConsumer> logger,
            ITenantContextProvider tenantContextProvider,
            IJobQueryRepository jobQueryRepository,
            IJobCommandRepository jobCommandRepository,
            ITaskCommunicationService taskCommunicationService,
            ICommunicationLogRepository communicationLogRepository) : base(logger, tenantContextProvider, jobQueryRepository, jobCommandRepository)
        {
            this.taskCommunicationService = taskCommunicationService;
            this.communicationLogRepository = communicationLogRepository;
        }

        public async Task Run(JobContext<LoggedQueueMessage<TaskInvitationRetractedJobMessage>> ctx)
        {
            var tenant = tenantContextProvider.CurrentTenant;

            logger.LogInformation(
                $"[{tenant.Name}]" +
                "Starting to check and send task invitation retracted...");

            var valid = await PrepareAsync(ctx.JobId, ctx.CancellationToken);
            if (!valid) return;

            try
            {
                await ExecuteAsync(ctx);

                logger.LogInformation(
                    $"[{tenant.Name}]" +
                    "DONE checking and sending task invitation retracted.");
            }
            catch (Exception ex)
            {
                logger.LogError(
                    $"[{tenant.Name}]" +
                    "An error occurred when checking and sending task invitation retracted -- {@ex}", ex);

                throw;
            }
            finally
            {
                await EndAsync(ctx.CancellationToken);
            }
        }

        private async Task ExecuteAsync(JobContext<LoggedQueueMessage<TaskInvitationRetractedJobMessage>> ctx)
        {
            try
            {
                await taskCommunicationService.SendTaskInvitationRetractedAsync(
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
