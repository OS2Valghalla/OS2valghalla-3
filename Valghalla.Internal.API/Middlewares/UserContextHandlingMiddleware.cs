using MediatR;
using System.Security.Claims;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Exceptions;
using Valghalla.Application.User;
using Valghalla.Internal.API.Auth;
using Valghalla.Internal.Application.Modules.Administration.User.Commands;
using Valghalla.Internal.Application.Modules.App.Queries;
using Valghalla.Internal.Application.Modules.App.Responses;

namespace Valghalla.Internal.API.Middlewares
{
    internal class UserContextHandlingMiddleware : IMiddleware
    {
        private readonly ISender sender;
        private readonly ILogger<UserContextHandlingMiddleware> logger;
        private readonly UserContextInternalProvider internalProvider;

        public UserContextHandlingMiddleware(
            ISender sender,
            ILogger<UserContextHandlingMiddleware> logger,
            UserContextInternalProvider internalProvider)
        {
            this.sender = sender;
            this.logger = logger;
            this.internalProvider = internalProvider;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var principal = context.User;
            var name = principal.FindFirstValue(AppClaimTypes.Name);
            var cvr = principal.FindFirstValue(AppClaimTypes.Cvr);
            var serial = principal.FindFirstValue(AppClaimTypes.Serial);

            if (!string.IsNullOrEmpty(cvr) && !string.IsNullOrEmpty(serial))
            {
                Guid userId;
                IEnumerable<Guid> roleIds;

                var user = await GetUserAsync(cvr, serial);

                if (user == null)
                {
                    userId = await CreateUserAsync(name ?? "Unknown", cvr, serial);
                    roleIds = new List<Guid> { Role.Administrator.Id };
                }
                else
                {
                    userId = user.Id;
                    roleIds = user.RoleIds;
                }

                internalProvider.SetUserContext(new UserContext()
                {
                    UserId = userId,
                    RoleIds = roleIds,
                    Name = name,
                    Cvr = cvr,
                    Serial = serial
                });
            }

            await next(context);
        }

        private async Task<UserResponse?> GetUserAsync(string cvrNumber, string serial)
        {
            try
            {
                var query = new GetInternalUserQuery(cvrNumber, serial);
                var response = await sender.Send(query);

                if (response is not Response<UserResponse> userResponse)
                {
                    throw new UnableToCastToUserResponseException();
                }

                return userResponse.Data;
            }
            catch (Exception ex)
            {
                throw new UserException(ex);
            }
        }

        private async Task<Guid> CreateUserAsync(string name, string cvrNumber, string serial)
        {
            try
            {
                var command = new CreateUserCommand(SystemRole.Administrator.Id, name, cvrNumber, serial);
                var response = await sender.Send(command);

                if (response is not Response<Guid> userResponse)
                {
                    throw new UnableToCastToUserResponseException();
                }

                return userResponse.Data;
            }
            catch (Exception ex)
            {
                throw new UserException(ex);
            }
        }
    }
}
