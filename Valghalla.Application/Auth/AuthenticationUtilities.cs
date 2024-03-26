using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Valghalla.Application.Auth
{
    public static class AuthenticationUtilities
    {
        public static bool IsAnonymousEndpoint(HttpContext context) => context.GetEndpoint()?.Metadata?.GetMetadata<IAllowAnonymous>() != null;

        public static bool IsApiEndpoint(HttpContext context) => context.Request.Path.HasValue && context.Request.Path.Value.Contains("/api/");

        public static async Task SetUnauthorizedResponseAsync(HttpContext context, CancellationToken cancellationToken)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync(string.Empty, cancellationToken);
        }

        public static async Task SetTokenExpiredResponseAsync(HttpContext context, CancellationToken cancellationToken)
        {
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("__TOKEN_EXPIRED__", cancellationToken);
        }
    }
}
