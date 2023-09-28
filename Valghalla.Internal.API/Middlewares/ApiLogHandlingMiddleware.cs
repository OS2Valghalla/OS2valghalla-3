using Microsoft.Extensions.Options;
using Valghalla.Application.AuditLog;
using Valghalla.Application.Logger;
using Valghalla.Application.User;

namespace Valghalla.Internal.API.Middlewares
{
    internal class ApiLogHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ApiLogHandlingMiddleware> logger;
        private readonly IOptions<LoggerConfiguration> loggerOptions;
        private readonly IUserContextProvider userContextProvider;
        private readonly IAuditLogService auditLogService;

        public ApiLogHandlingMiddleware(
            ILogger<ApiLogHandlingMiddleware> logger,
            IOptions<LoggerConfiguration> loggerOptions,
            IUserContextProvider userContextProvider,
            IAuditLogService auditLogService)
        {
            this.logger = logger;
            this.loggerOptions = loggerOptions;
            this.userContextProvider = userContextProvider;
            this.auditLogService = auditLogService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var user = userContextProvider.CurrentUser;
            var apiPath = context.Request.Path.ToString();

            if (apiPath.StartsWith("/api/") && user != null)
            {
                if (loggerOptions.Value.ApiLogging)
                {
                    var auditLog = new ApiAuditLog(apiPath);
                    await auditLogService.AddAuditLogAsync(auditLog, default);
                }
                else
                {
                    var userId = userContextProvider.CurrentUser.UserId;
                    logger.LogInformation($"API: {apiPath} - UserId: {userId}");
                }
            }

            await next(context);
        }
    }
}
