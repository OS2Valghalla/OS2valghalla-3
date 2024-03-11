using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.Internals;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using Valghalla.Application;
using Valghalla.Application.Cache;
using Valghalla.Application.Queue;
using Valghalla.Application.Secret;
using Valghalla.Application.Tenant;
using Valghalla.Application.User;
using Valghalla.Integration;
using Valghalla.Worker;
using Valghalla.Worker.Auth;
using Valghalla.Worker.Filters;
using Valghalla.Worker.Infrastructure;
using Valghalla.Worker.Infrastructure.Models;
using Valghalla.Worker.Jobs;
using Valghalla.Worker.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

});

builder.Services.AddHostedService<Worker>();

var jobConfigurationSection = builder.Configuration.GetSection("Job");
var jobConfiguration = jobConfigurationSection.Get<JobConfiguration>()!;

builder.Services.Configure<SecretConfiguration>(builder.Configuration.GetSection("Secrets"));
builder.Services.Configure<JobConfiguration>(jobConfigurationSection);
builder.Services.Configure<Valghalla.Application.Logger.LoggerConfiguration>(builder.Configuration.GetSection("Logger"));

builder.Services.AddScoped<TenantContextInternalProvider>();
builder.Services.AddScoped<ITenantContextProvider, TenantContextProvider>();
builder.Services.AddScoped<IUserContextProvider, UserContextProvider>();

builder.Services.AddSharedServices();
builder.Services.AddInfrastructure();
builder.Services.AddIntegration();

builder.Services.AddDbContext<JobServiceSagaDbContext>(builder =>
{
    builder.UseNpgsql(jobConfiguration.ConnectionString, m =>
    {
        m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
        m.MigrationsHistoryTable($"__{nameof(JobServiceSagaDbContext)}");
    });
});

builder.Services.AddOptions<MassTransitHostOptions>()
    .Configure(options =>
    {
        options.WaitUntilStarted = true;
        options.StartTimeout = TimeSpan.FromMinutes(1);
        options.StopTimeout = TimeSpan.FromMinutes(1);
    });

builder.Services.AddHttpClient();

builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.AddScoped<IParticipantSyncService, ParticipantSyncService>();
builder.Services.AddScoped<ITaskCommunicationService, TaskCommunicationService>();

builder.Services.AddMassTransit(mt =>
{
    mt.AddDelayedMessageScheduler();

    mt.AddSagaRepository<JobSaga>()
        .EntityFrameworkRepository(r =>
        {
            r.ExistingDbContext<JobServiceSagaDbContext>();
            r.UsePostgres();
        });

    mt.AddSagaRepository<JobTypeSaga>()
        .EntityFrameworkRepository(r =>
        {
            r.ExistingDbContext<JobServiceSagaDbContext>();
            r.UsePostgres();
        });

    mt.AddSagaRepository<JobAttemptSaga>()
        .EntityFrameworkRepository(r =>
        {
            r.ExistingDbContext<JobServiceSagaDbContext>();
            r.UsePostgres();
        });

    mt.AddConsumer<ParticipantSyncJobConsumer, ParticipantSyncJobConsumerDefinition>();
    mt.AddConsumer<ElectionActivationJobConsumer, ElectionActivationJobConsumerDefinition>();
    mt.AddConsumer<ElectionDeactivationJobConsumer, ElectionDeactivationJobConsumerDefinition>();
    mt.AddConsumer<AuditLogClearJobConsumer, AuditLogClearJobConsumerDefinition>();
    mt.AddConsumer<CommunicationLogClearJobConsumer, CommunicationLogClearJobConsumerDefinition>();
    // Task notification jobs
    mt.AddConsumer<TaskInvitationJobConsumer, TaskInvitationJobConsumerDefinition>();
    mt.AddConsumer<RemovedFromTaskJobConsumer, RemovedFromTaskJobConsumerDefinition>();
    mt.AddConsumer<TaskGetInvitationReminderJobConsumer, TaskGetInvitationReminderJobConsumerDefinition>();
    mt.AddConsumer<TaskSendInvitationReminderJobConsumer, TaskSendInvitationReminderJobConsumerDefinition>();
    mt.AddConsumer<TaskGetReminderJobConsumer, TaskGetReminderJobConsumerDefinition>();
    mt.AddConsumer<TaskSendReminderJobConsumer, TaskSendReminderJobConsumerDefinition>();
    mt.AddConsumer<TaskRegistrationJobConsumer, TaskRegistrationJobConsumerDefinition>();
    mt.AddConsumer<TaskCancellationJobConsumer, TaskCancellationJobConsumerDefinition>();
    mt.AddConsumer<TaskInvitationRetractedJobConsumer, TaskInvitationRetractedJobConsumerDefinition>();
    mt.AddConsumer<SendGroupMessageJobConsumer, SendGroupMessageJobConsumerDefinition>();

    mt.UsingRabbitMq((ctx, config) =>
    {
        var secretService = ctx.GetRequiredService<ISecretService>();
        var settings = secretService.GetQueueConfigurationAsync(default).Result;

        config.UseDelayedMessageScheduler();

        config.Host(settings.Host, settings.Port, settings.VirtualHost, hostConfig =>
        {
            hostConfig.Username(settings.Username);
            hostConfig.Password(settings.Password);
        });

        config.UseConsumeFilter(typeof(TenantContextFilter<>), ctx,
            x => x.Include(type => type.HasInterface<IQueueMessage>()));

        config.UseConsumeFilter(typeof(LogContextFilter<>), ctx,
            x => x.Include(type => type.HasInterface<IQueueMessage>()));

        config.ServiceInstance(instance =>
        {
            instance.ConfigureJobServiceEndpoints(js =>
            {
                js.SagaPartitionCount = 1;

                js.ConfigureSagaRepositories(ctx);
            });

            instance.ConfigureEndpoints(ctx, f =>
            {
                f.Include<ParticipantSyncJobConsumer>();
                f.Include<ElectionActivationJobConsumer>();
                f.Include<ElectionDeactivationJobConsumer>();
                f.Include<AuditLogClearJobConsumer>();
                f.Include<CommunicationLogClearJobConsumer>();
                f.Include<RemovedFromTaskJobConsumer>();
                f.Include<TaskInvitationJobConsumer>();
                f.Include<TaskRegistrationJobConsumer>();
                f.Include<TaskGetInvitationReminderJobConsumer>();
                f.Include<TaskSendInvitationReminderJobConsumer>();
                f.Include<TaskGetReminderJobConsumer>();
                f.Include<TaskSendReminderJobConsumer>();
                f.Include<TaskCancellationJobConsumer>();
                f.Include<TaskInvitationRetractedJobConsumer>();
                f.Include<SendGroupMessageJobConsumer>();
            });
        });

        config.ConfigureEndpoints(ctx);
    });
});

var loggerConfig = builder.Configuration.GetSection("Logger").Get<Valghalla.Application.Logger.LoggerConfiguration>()!;
var serilogConfig = loggerConfig.Serilog;
var systemLogFilePath = Path.Combine(
    AppContext.BaseDirectory,
    serilogConfig.SystemPath,
    serilogConfig.FileName);

// Global serilog config during startup phase
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.File(
        systemLogFilePath,
        shared: true,
        outputTemplate: serilogConfig.OutputTemplate,
        rollingInterval: Enum.Parse<RollingInterval>(serilogConfig.RollingInterval))
    .CreateBootstrapLogger();

builder.Logging.ClearProviders();

try
{
    Log.Information("Starting host");

    builder.Host.UseSerilog((context, services, configuration) =>
    {
        // Override serilog config after startup
        // fallback to system log file in case tenant not available
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.File(
                systemLogFilePath,
                shared: true,
                outputTemplate: serilogConfig.OutputTemplate,
                rollingInterval: Enum.Parse<RollingInterval>(serilogConfig.RollingInterval))
            .WriteTo.Map(nameof(LogContextFilter), (logFilePath, wt) =>
            {
                wt.File(
                    logFilePath,
                    shared: true,
                    outputTemplate: serilogConfig.OutputTemplate,
                    rollingInterval: Enum.Parse<RollingInterval>(serilogConfig.RollingInterval));
            });
    });

    var app = builder.Build();

    app.Use((context, next) =>
    {
        if (context.Request.Headers.ContainsKey("X-Forwarded-Proto"))
        {
            if (!string.IsNullOrEmpty(context.Request.Headers["X-Forwarded-Proto"]))
            {
                context.Request.Scheme = context.Request.Headers["X-Forwarded-Proto"];
            }
        }

        return next(context);
    });

    app.MapGet("/", () => "Hello World!");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}