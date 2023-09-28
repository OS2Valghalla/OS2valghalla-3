using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;

namespace Valghalla.Internal.Application.Modules.Administration.User.Queries
{
    public sealed record GetUserRolesQuery : IQuery<Response>;

    internal sealed class GetUserRolesQueryHandler : IQueryHandler<GetUserRolesQuery>
    {
        public async Task<Response> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            var result = Role.GetAllRoles();
            await Task.CompletedTask;

            return Response.Ok(result);
        }
    }
}
