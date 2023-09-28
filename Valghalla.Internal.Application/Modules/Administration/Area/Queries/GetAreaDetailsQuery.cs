using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Area.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Area.Queries
{
    public sealed record GetAreaDetailsQuery(Guid Id) : IQuery<Response>;

    public sealed class GetAreaDetailsQueryValidator : AbstractValidator<GetAreaDetailsQuery>
    {
        public GetAreaDetailsQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

        }
    }

    internal class GetAreaDetailsQueryHandler : IQueryHandler<GetAreaDetailsQuery>
    {
        private readonly IAreaQueryRepository areaQueryRepository;

        public GetAreaDetailsQueryHandler(IAreaQueryRepository areaQueryRepository)
        {
            this.areaQueryRepository = areaQueryRepository;
        }

        public async Task<Response> Handle(GetAreaDetailsQuery query, CancellationToken cancellationToken)
        {
            var result = await areaQueryRepository.GetAreaAsync(query, cancellationToken);

            if (result == null)
            {
                return Response.FailWithItemNotFoundError();
            }

            return Response.Ok(result);
        }
    }
}
