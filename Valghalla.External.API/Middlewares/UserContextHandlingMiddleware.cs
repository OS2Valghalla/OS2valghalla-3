using Valghalla.Application.Auth;
using Valghalla.Application.User;
using Valghalla.External.API.Auth;

namespace Valghalla.External.API.Middlewares
{
    internal class UserContextHandlingMiddleware : IMiddleware
    {
        private readonly IUserService userService;
        private readonly UserContextInternalProvider userContextInternalProvider;

        public UserContextHandlingMiddleware(IUserService userService, UserContextInternalProvider userContextInternalProvider)
        {
            this.userService = userService;
            this.userContextInternalProvider = userContextInternalProvider;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var cancellationToken = context.RequestAborted;

            if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
            {
                if (AuthenticationUtilities.IsAnonymousEndpoint(context))
                {
                    await next(context);
                }
                else
                {
                    await AuthenticationUtilities.SetTokenExpiredResponseAsync(context, cancellationToken);
                }

                return;
            }

            var userInfo = await userService.GetUserInfoAsync(context.User, cancellationToken);

            if (userInfo == null)
            {
                if (IsRegistrationPath(context))
                {
                    await next(context);
                    return;
                }

                await AuthenticationUtilities.SetUnauthorizedResponseAsync(context, cancellationToken);
                return;
            }

            userContextInternalProvider.SetUserContext(new UserContext()
            {
                UserId = userInfo.Id,
                RoleIds = userInfo.RoleIds,
                ParticipantId = userInfo.ParticipantId,
                Name = userInfo.Name,
                Cpr = context.User.GetCpr(),
            });

            await next(context);
        }

        private static bool IsRegistrationPath(HttpContext context) =>
            context.Request.Path.HasValue &&
            (context.Request.Path.Value.Contains("api/registration/registerwithteam") || context.Request.Path.Value.Contains("api/registration/registerwithtask"));
    }
}
