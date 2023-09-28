using Microsoft.Extensions.Options;
using Serilog.Context;
using Valghalla.Application.Logger;
using Valghalla.Application.Tenant;

namespace Valghalla.External.API.Middlewares
{
    internal class LogContextHandlingMiddleware : IMiddleware
    {
        private readonly IOptions<LoggerConfiguration> loggerOptions;
        private readonly ITenantContextProvider tenantContextProvider;

        public LogContextHandlingMiddleware(
            IOptions<LoggerConfiguration> loggerOptions,
            ITenantContextProvider tenantContextProvider)
        {
            this.loggerOptions = loggerOptions;
            this.tenantContextProvider = tenantContextProvider;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var tenant = tenantContextProvider.CurrentTenant;

            var serilogConfig = loggerOptions.Value.Serilog;

            var logFilePath = Path.Combine(
                AppContext.BaseDirectory,
                serilogConfig.TenantPath,
                tenant.Name,
                serilogConfig.FileName);

            using var disposable = LogContext.PushProperty(nameof(LogContextHandlingMiddleware), logFilePath);
            await next(context);
        }
    }
}
