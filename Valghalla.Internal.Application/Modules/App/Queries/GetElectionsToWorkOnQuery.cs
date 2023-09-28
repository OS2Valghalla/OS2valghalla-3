using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.App.Interfaces;

namespace Valghalla.Internal.Application.Modules.App.Queries
{
    public sealed record GetElectionsToWorkOnQuery() : IQuery<Response>;

    internal class GetElectionsToWorkOnQueryHandler : IQueryHandler<GetElectionsToWorkOnQuery>
    {
        private readonly IAppElectionQueryRepository appElectionQueryRepository;

        public GetElectionsToWorkOnQueryHandler(IAppElectionQueryRepository appElectionQueryRepository)
        {
            this.appElectionQueryRepository = appElectionQueryRepository;
        }

        public async Task<Response> Handle(GetElectionsToWorkOnQuery query, CancellationToken cancellationToken)
        {
            var result = await appElectionQueryRepository.GetElectionsToWorkOnAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
