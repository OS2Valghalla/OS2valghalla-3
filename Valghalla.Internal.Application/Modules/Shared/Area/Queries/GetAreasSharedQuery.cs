using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Shared.Area.Interfaces;

namespace Valghalla.Internal.Application.Modules.Shared.Area.Queries
{

    public sealed record GetAreasSharedQuery() : IQuery<Response>;

    internal class GetAreasSharedQueryHandler : IQueryHandler<GetAreasSharedQuery>
    {
        private readonly IAreaSharedQueryRepository areaQueryRepository;

        public GetAreasSharedQueryHandler(IAreaSharedQueryRepository areaQueryRepository)
        {
            this.areaQueryRepository = areaQueryRepository;
        }

        public async Task<Response> Handle(GetAreasSharedQuery query, CancellationToken cancellationToken)
        {
            var result = await areaQueryRepository.GetAreasAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
