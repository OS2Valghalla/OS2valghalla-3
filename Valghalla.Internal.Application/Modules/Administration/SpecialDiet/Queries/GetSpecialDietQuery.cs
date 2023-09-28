using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Queries
{
    public sealed record GetSpecialDietQuery (Guid SpecialDietId) : IQuery<Response>;
    public sealed class GetSpecialDietQueryValidator : AbstractValidator<GetSpecialDietQuery>
    {
        public GetSpecialDietQueryValidator()
        {
            RuleFor(x => x.SpecialDietId)
                .NotEmpty();
        }
    }

    internal sealed class GetSpecialDietQueryHandler : IQueryHandler<GetSpecialDietQuery>
    {
        private readonly ISpecialDietQueryRepository specialDietQueryRepository;

        public GetSpecialDietQueryHandler(ISpecialDietQueryRepository specialDietQueryRepository)
        {
            this.specialDietQueryRepository = specialDietQueryRepository;
        }

        public async Task<Response> Handle(GetSpecialDietQuery query, CancellationToken cancellationToken)
        {
            var result = await specialDietQueryRepository.GetSpecialDietAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }

}
