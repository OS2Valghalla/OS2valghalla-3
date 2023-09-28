using MassTransit;
using Microsoft.Extensions.Options;
using Valghalla.Application.Logger;
using Valghalla.Application.Queue;
using Valghalla.Application.Tenant;

namespace Valghalla.Worker.Filters
{
    internal static class LogContextFilter { }

    internal class LogContextFilter<T> : IFilter<ConsumeContext<T>> where T : QueueMessage
    {
        private readonly IOptions<LoggerConfiguration> loggerOptions;
        private readonly ITenantContextProvider tenantContextProvider;

        public LogContextFilter(
            IOptions<LoggerConfiguration> loggerOptions,
            ITenantContextProvider tenantContextProvider)
        {
            this.loggerOptions = loggerOptions;
            this.tenantContextProvider = tenantContextProvider;
        }

        public void Probe(ProbeContext context)
        {
            
        }

        public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {
            var tenant = tenantContextProvider.CurrentTenant;

            var serilogConfig = loggerOptions.Value.Serilog;

            var logFilePath = Path.Combine(
                AppContext.BaseDirectory,
                serilogConfig.TenantPath,
                tenant.Name,
                serilogConfig.FileName);

            using var disposable = Serilog.Context.LogContext.PushProperty(nameof(LogContextFilter), logFilePath);
            await next.Send(context);
        }
    }
}
