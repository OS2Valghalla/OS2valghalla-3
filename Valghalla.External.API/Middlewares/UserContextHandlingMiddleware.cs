using MediatR;
using System.Security.Claims;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Exceptions;
using Valghalla.Application.User;
using Valghalla.External.API.Auth;
using Valghalla.External.Application.Modules.App.Queries;
using Valghalla.External.Application.Modules.App.Responses;

namespace Valghalla.External.API.Middlewares
{
    internal class UserContextHandlingMiddleware : IMiddleware
    {
        private readonly ISender sender;
        private readonly UserContextInternalProvider userContextInternalProvider;

        public UserContextHandlingMiddleware(
            ISender sender,
            UserContextInternalProvider userContextInternalProvider)
        {
            this.sender = sender;
            this.userContextInternalProvider = userContextInternalProvider;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var principal = context.User;
            var cpr = principal.FindFirstValue(AppClaimTypes.Cpr);

            if (!string.IsNullOrEmpty(cpr))
            {
                var user = await GetUserAsync(cpr);

                if (user != null)
                {
                    userContextInternalProvider.SetUserContext(new UserContext()
                    {
                        UserId = user.Id,
                        RoleIds = user.RoleIds,
                        ParticipantId = user.ParticipantId,
                        Name = user.Name,
                        Cpr = cpr
                    });
                }
            }

            await next(context);
        }

        private async Task<UserResponse?> GetUserAsync(string cprNumber)
        {
            try
            {
                var query = new GetExternalUserQuery(cprNumber);
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
    }
}
