using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.User.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.User.Queries
{
    public sealed record GetUsersQuery : IQuery<Response>;

    internal sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery>
    {
        private readonly IUserQueryRepository userQueryRepository;

        public GetUsersQueryHandler(IUserQueryRepository userQueryRepository)
        {
            this.userQueryRepository = userQueryRepository;
        }

        public async Task<Response> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await userQueryRepository.GetUsersAsync(cancellationToken);

            return Response.Ok(result);
        }
    }
}
