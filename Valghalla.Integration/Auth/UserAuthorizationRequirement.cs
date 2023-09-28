using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Valghalla.Application.Exceptions;
using Valghalla.Application.User;

namespace Valghalla.Integration.Auth
{
    internal class UserAuthorizationRequirement : IAuthorizationRequirement
    {
        public Guid RoleId { get; }

        public UserAuthorizationRequirement(Guid roleId)
        {
            RoleId = roleId;
        }
    }

    internal class UserAuthorizationHandler : AuthorizationHandler<UserAuthorizationRequirement>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly ILogger<UserAuthorizationHandler> logger;

        public UserAuthorizationHandler(IUserContextProvider userContextProvider, ILogger<UserAuthorizationHandler> logger)
        {
            this.userContextProvider = userContextProvider;
            this.logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserAuthorizationRequirement requirement)
        {
            try
            {
                if (!userContextProvider.Registered)
                {
                    context.Fail();
                    logger.LogWarning("User does not exist in the system");
                    return Task.CompletedTask;
                }

                var valid = false;
                var user = userContextProvider.CurrentUser;

                foreach (var roleId in user.RoleIds)
                {
                    if (Role.TryParse(roleId, out var userRole))
                    {
                        var requiredRole = Role.Parse(requirement.RoleId);

                        if (requiredRole.IsAuthorized(userRole))
                        {
                            valid = true;
                            break;
                        }
                    }
                }

                if (!valid)
                {
                    context.Fail();
                    logger.LogWarning("User does not have permission to access this resource -- UserId: {@UserId}, Required RoleId: {@RoleId}", user.UserId, requirement.RoleId);
                    return Task.CompletedTask;
                }

                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new UserException(ex);
            }
        }
    }
}
