using Valghalla.Application.Auth;
using Valghalla.Application.User;
using Valghalla.Internal.API.Auth;

namespace Valghalla.Internal.API.Middlewares
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
                await AuthenticationUtilities.SetUnauthorizedResponseAsync(context, cancellationToken);
                return;
            }

            userContextInternalProvider.SetUserContext(new UserContext()
            {
                UserId = userInfo.Id,
                RoleIds = userInfo.RoleIds,
                Name = userInfo.Name,
                Cvr = context.User.GetCvr(),
                Serial = context.User.GetSerial()
            });

            await next(context);
        }
    }
}
