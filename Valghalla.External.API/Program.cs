using MassTransit;
using MassTransit.Internals;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using Valghalla.Application.Auth;
using Valghalla.Application.Authentication;
using Valghalla.Application.Queue;
using Valghalla.Application.Saml;
using Valghalla.Application.Secret;
using Valghalla.Application.Tenant;
using Valghalla.Application.User;
using Valghalla.External.API.Auth;
using Valghalla.External.API.Filters;
using Valghalla.External.API.Middlewares;
using Valghalla.External.API.Queues;
using Valghalla.External.API.Services;
using Valghalla.External.Application;
using Valghalla.External.Infrastructure;
using Valghalla.Integration;

namespace Valghalla.External.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            IdentityModelEventSource.ShowPII = true;

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Valghalla External API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            builder.Services.Configure<SecretConfiguration>(builder.Configuration.GetSection("Secrets"));
            builder.Services.Configure<GlobalAuthConfiguration>(builder.Configuration.GetSection("Authentication"));
            builder.Services.Configure<Valghalla.Application.Logger.LoggerConfiguration>(builder.Configuration.GetSection("Logger"));

            builder.Services.AddMassTransit(mt =>
            {
                mt.AddConsumer<ExternalUserClearCacheConsumer, ExternalUserClearCacheConsumerDefinition>();

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

                    config.ConfigureEndpoints(ctx, f =>
                    {
                        f.Include<ExternalUserClearCacheConsumer>();
                    });
                });
            });

            builder.Services.AddInfrastructure();
            builder.Services.AddIntegration();
            builder.Services.AddApplication();
            builder.Services.AddRouting();
            builder.Services.AddControllers();

            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IQueueService, QueueService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ISaml2AuthPostProcessor, Saml2AuthPostProcessor>();
            builder.Services.AddSingleton<IUserTokenConfigurator, UserTokenConfigurator>();

            builder.Services.AddScoped<GlobalExceptionHandlingMiddleware>();
            builder.Services.AddScoped<LogContextHandlingMiddleware>();
            builder.Services.AddScoped<TenantContextHandlingMiddleware>();
            builder.Services.AddScoped<UserContextHandlingMiddleware>();

            builder.Services.AddScoped<ISaml2AuthContextProvider, Saml2AuthContextProvider>();
            builder.Services.AddScoped<TenantContextInternalProvider>();
            builder.Services.AddScoped<ITenantContextProvider, TenantContextProvider>();
            builder.Services.AddScoped<UserContextInternalProvider>();
            builder.Services.AddScoped<IUserContextProvider, UserContextProvider>();

            builder.Services.AddAuthServices();

            var corsPolicyName = "ValghallaCorsPolicy";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(corsPolicyName, policy =>
                {
                    if (builder.Environment.IsDevelopment())
                    {
                        var origins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

                        policy
                            .WithOrigins(origins)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    }
                    else
                    {
                        policy
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
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

            builder.Services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "wwwroot";
            });

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
                        .WriteTo.Map(nameof(LogContextHandlingMiddleware), (logFilePath, wt) =>
                        {
                            wt.File(
                                logFilePath,
                                shared: true,
                                outputTemplate: serilogConfig.OutputTemplate,
                                rollingInterval: Enum.Parse<RollingInterval>(serilogConfig.RollingInterval));
                        });
                });

                var app = builder.Build();

                app.UseForwardedHeaders();

                app.UseRouting();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseCors(corsPolicyName);

                app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
                // Tenant context middleware need to run first before authentication so claim transformation can query User
                app.UseMiddleware<TenantContextHandlingMiddleware>();

                // Depend on TenantContextProvider
                app.UseMiddleware<LogContextHandlingMiddleware>();

                app.UseAuthentication();

                // Depend on claims transformation so must be run after authentication middlware
                app.UseMiddleware<UserContextHandlingMiddleware>();

                // UserContextProvider will be used for authorization
                app.UseAuthorization();

                app.UseDefaultFiles();
                app.UseSpaStaticFiles();
                app.MapFallbackToFile("index.html");
                app.MapControllers();

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
        }
    }
}
