using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Area.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Area.Queries
{
    public sealed record GetAllAreasQuery() : IQuery<Response>;

    public sealed class GetAllAreasQueryValidator : AbstractValidator<GetAllAreasQuery>
    {
        public GetAllAreasQueryValidator()
        {
        }
    }

    internal sealed class GetAllAreasQueryHandler : IQueryHandler<GetAllAreasQuery>
    {
        private readonly IAreaQueryRepository areaQueryRepository;

        public GetAllAreasQueryHandler(IAreaQueryRepository areaQueryRepository)
        {
            this.areaQueryRepository = areaQueryRepository;
        }

        public async Task<Response> Handle(GetAllAreasQuery query, CancellationToken cancellationToken)
        {
            var result = await areaQueryRepository.GetAllAreasAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
