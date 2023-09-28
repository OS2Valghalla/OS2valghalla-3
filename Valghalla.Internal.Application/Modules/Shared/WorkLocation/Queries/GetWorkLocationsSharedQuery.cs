using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Shared.WorkLocation.Interfaces;

namespace Valghalla.Internal.Application.Modules.Shared.WorkLocation.Queries
{
    public sealed record GetWorkLocationsSharedQuery() : IQuery<Response>;

    internal class GetWorkLocationsSharedQueryHandler : IQueryHandler<GetWorkLocationsSharedQuery>
    {
        private readonly IWorkLocationSharedQueryRepository workLocationSharedQueryRepository;

        public GetWorkLocationsSharedQueryHandler(IWorkLocationSharedQueryRepository workLocationSharedQueryRepository)
        {
            this.workLocationSharedQueryRepository = workLocationSharedQueryRepository;
        }

        public async Task<Response> Handle(GetWorkLocationsSharedQuery query, CancellationToken cancellationToken)
        {
            var result = await workLocationSharedQueryRepository.GetWorkLocationsAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
