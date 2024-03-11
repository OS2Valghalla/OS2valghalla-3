using MassTransit;
using MassTransit.Contracts.JobService;
using Microsoft.Extensions.Options;
using Valghalla.Application.Queue;
using Valghalla.Application.Secret;
using Valghalla.Worker.Infrastructure.Models;
using Valghalla.Worker.QueueMessages;

namespace Valghalla.Worker
{
    internal class Worker : IHostedService, IDisposable
    {
        private Timer? participantSyncJobTimer = null;
        private Timer? taskInvitationReminderJobTimer = null;
        private Timer? taskReminderJobTimer = null;
        private Timer? electionDeactivationJobTimer = null;
        private Timer? auditLogClearJobTimer = null;
        private Timer? communicationLogClearJobTimer = null;

        private readonly ILogger<Worker> logger;
        private readonly IOptions<JobConfiguration> jobConfigurationOptions;
        private readonly IServiceProvider serviceProvider;
        private readonly ISecretService secretService;

        public Worker(
            ILogger<Worker> logger,
            IOptions<JobConfiguration> jobConfigurationOptions,
            IServiceProvider serviceProvider,
            ISecretService secretService)
        {
            this.logger = logger;
            this.jobConfigurationOptions = jobConfigurationOptions;
            this.serviceProvider = serviceProvider;
            this.secretService = secretService;
        }

        public void Dispose()
        {
            participantSyncJobTimer?.Dispose();
            taskInvitationReminderJobTimer?.Dispose();
            taskReminderJobTimer?.Dispose();
            electionDeactivationJobTimer?.Dispose();
            auditLogClearJobTimer?.Dispose();
            communicationLogClearJobTimer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Worker starting...");

            var config = jobConfigurationOptions.Value;

            participantSyncJobTimer = new Timer(
                async _ =>
                {
                    try
                    {
                        await ExecuteParticipantSyncJobAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("An error occurred when running paritcipant sync job in worker -- {@ex}", ex);
                    }
                },
                null,
                TimeSpan.Zero,
                TimeSpan.FromHours(config.ParticipantSyncJob.Period));

            taskInvitationReminderJobTimer = new Timer(
                async _ =>
                {
                    try
                    {
                        await ExecuteTaskInvitationReminderJobAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("An error occurred when running task invitation reminder job in worker -- {@ex}", ex);
                    }
                },
                null,
                TimeSpan.Zero,
                TimeSpan.FromHours(config.TaskInvitationReminderJob.Period));

            taskReminderJobTimer = new Timer(
                async _ =>
                {
                    try
                    {
                        await ExecuteTaskReminderJobAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("An error occurred when running task reminder job in worker -- {@ex}", ex);
                    }
                },
                null,
                TimeSpan.Zero,
                TimeSpan.FromHours(config.TaskReminderJob.Period));

            electionDeactivationJobTimer = new Timer(
                async _ =>
                {
                    try
                    {
                        await ExecuteElectionDeactivationJobAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("An error occurred when checking and deactivating elections in worker -- {@ex}", ex);
                    }
                },
                null,
                TimeSpan.Zero,
                TimeSpan.FromHours(config.ElectionDeactivationJob.Period));

            auditLogClearJobTimer = new Timer(
                async _ =>
                {
                    try
                    {
                        await ExecuteAuditLogClearJobAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("An error occurred when checking and clearing audit logs in worker -- {@ex}", ex);
                    }
                },
                null,
                TimeSpan.Zero,
                TimeSpan.FromHours(config.AuditLogClearJob.Period));

            communicationLogClearJobTimer = new Timer(
                async _ =>
                {
                    try
                    {
                        await ExecuteCommunicationLogClearJobAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("An error occurred when checking and clearing communication logs in worker -- {@ex}", ex);
                    }
                },
                null,
                TimeSpan.Zero,
                TimeSpan.FromHours(config.CommunicationLogClearJob.Period));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Worker stopping...");

            participantSyncJobTimer?.Change(Timeout.Infinite, 0);
            taskReminderJobTimer?.Change(Timeout.Infinite, 0);
            taskInvitationReminderJobTimer?.Change(Timeout.Infinite, 0);
            electionDeactivationJobTimer?.Change(Timeout.Infinite, 0);
            auditLogClearJobTimer?.Change(Timeout.Infinite, 0);
            communicationLogClearJobTimer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private async Task ExecuteParticipantSyncJobAsync(CancellationToken cancellationToken)
        {
            var tenants = await secretService.GetTenantConfigurationsAsync(cancellationToken);
            var tasks = tenants
                .Select(tenant => Task.Run(async () =>
                {
                    try
                    {
                        using var scope = serviceProvider.CreateScope();

                        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                        var queueMessage = new QueueMessage<ParticipantSyncJobMessage>(tenant.InternalDomain, new ParticipantSyncJobMessage());

                        await publishEndpoint.Publish<SubmitJob<QueueMessage<ParticipantSyncJobMessage>>>(new
                        {
                            JobId = Guid.NewGuid(),
                            Job = queueMessage
                        }, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(
                            $"[{tenant.Name}]" +
                            "An error occurred when firing paritcipant sync job message in worker -- {@ex}", ex);
                    }
                }))
                .ToArray();

            await Task.WhenAll(tasks);
        }

        private async Task ExecuteTaskInvitationReminderJobAsync(CancellationToken cancellationToken)
        {
            var tenants = await secretService.GetTenantConfigurationsAsync(cancellationToken);
            var tasks = tenants
                .Select(tenant => Task.Run(async () =>
                {
                    try
                    {
                        using var scope = serviceProvider.CreateScope();

                        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                        var queueMessage = new QueueMessage<TaskGetInvitationReminderJobMessage>(tenant.InternalDomain, new TaskGetInvitationReminderJobMessage());

                        await publishEndpoint.Publish<SubmitJob<QueueMessage<TaskGetInvitationReminderJobMessage>>>(new
                        {
                            JobId = Guid.NewGuid(),
                            Job = queueMessage
                        }, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(
                            $"[{tenant.Name}]" +
                            "An error occurred when firing task get invitation reminder job message in worker -- {@ex}", ex);
                    }
                }))
                .ToArray();

            await Task.WhenAll(tasks);
        }

        private async Task ExecuteTaskReminderJobAsync(CancellationToken cancellationToken)
        {
            var tenants = await secretService.GetTenantConfigurationsAsync(cancellationToken);
            var tasks = tenants
                .Select(tenant => Task.Run(async () =>
                {
                    try
                    {
                        using var scope = serviceProvider.CreateScope();

                        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                        var queueMessage = new QueueMessage<TaskGetReminderJobMessage>(tenant.InternalDomain, new TaskGetReminderJobMessage());

                        await publishEndpoint.Publish<SubmitJob<QueueMessage<TaskGetReminderJobMessage>>>(new
                        {
                            JobId = Guid.NewGuid(),
                            Job = queueMessage
                        }, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(
                            $"[{tenant.Name}]" +
                            "An error occurred when firing task get reminder job message in worker -- {@ex}", ex);
                    }
                }))
                .ToArray();

            await Task.WhenAll(tasks);
        }

        private async Task ExecuteElectionDeactivationJobAsync(CancellationToken cancellationToken)
        {
            var tenants = await secretService.GetTenantConfigurationsAsync(cancellationToken);
            var tasks = tenants
                .Select(tenant => Task.Run(async () =>
                {
                    try
                    {
                        using var scope = serviceProvider.CreateScope();

                        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                        var queueMessage = new QueueMessage<ElectionDeactivationJobMessage>(tenant.InternalDomain, new ElectionDeactivationJobMessage());

                        await publishEndpoint.Publish<SubmitJob<QueueMessage<ElectionDeactivationJobMessage>>>(new
                        {
                            JobId = Guid.NewGuid(),
                            Job = queueMessage
                        }, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(
                            $"[{tenant.Name}]" +
                            "An error occurred when firing election deactivation job message in worker -- {@ex}", ex);
                    }
                }))
                .ToArray();

            await Task.WhenAll(tasks);
        }

        private async Task ExecuteAuditLogClearJobAsync(CancellationToken cancellationToken)
        {
            var tenants = await secretService.GetTenantConfigurationsAsync(cancellationToken);
            var tasks = tenants
                .Select(tenant => Task.Run(async () =>
                {
                    try
                    {
                        using var scope = serviceProvider.CreateScope();

                        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                        var queueMessage = new QueueMessage<AuditLogClearJobMessage>(tenant.InternalDomain, new AuditLogClearJobMessage());

                        await publishEndpoint.Publish<SubmitJob<QueueMessage<AuditLogClearJobMessage>>>(new
                        {
                            JobId = Guid.NewGuid(),
                            Job = queueMessage
                        }, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(
                            $"[{tenant.Name}]" +
                            "An error occurred when firing audit log clear job message in worker -- {@ex}", ex);
                    }
                }))
                .ToArray();

            await Task.WhenAll(tasks);
        }

        private async Task ExecuteCommunicationLogClearJobAsync(CancellationToken cancellationToken)
        {
            var tenants = await secretService.GetTenantConfigurationsAsync(cancellationToken);
            var tasks = tenants
                .Select(tenant => Task.Run(async () =>
                {
                    try
                    {
                        using var scope = serviceProvider.CreateScope();

                        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                        var queueMessage = new QueueMessage<CommunicationLogClearJobMessage>(tenant.InternalDomain, new CommunicationLogClearJobMessage());

                        await publishEndpoint.Publish<SubmitJob<QueueMessage<CommunicationLogClearJobMessage>>>(new
                        {
                            JobId = Guid.NewGuid(),
                            Job = queueMessage
                        }, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(
                            $"[{tenant.Name}]" +
                            "An error occurred when firing communication log clear job message in worker -- {@ex}", ex);
                    }
                }))
                .ToArray();

            await Task.WhenAll(tasks);
        }
    }
}