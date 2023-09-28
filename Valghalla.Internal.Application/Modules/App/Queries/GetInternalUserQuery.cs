using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Cache;
using Valghalla.Internal.Application.Modules.App.Interfaces;

namespace Valghalla.Internal.Application.Modules.App.Queries
{
    public sealed record GetInternalUserQuery(string CvrNumber, string Serial) : IQuery<Response>;

    public sealed class GetInternalUserQueryValidator : AbstractValidator<GetInternalUserQuery>
    {
        public GetInternalUserQueryValidator()
        {
            RuleFor(x => x.CvrNumber)
                .NotEmpty();

            RuleFor(x => x.Serial)
                .NotEmpty();
        }
    }

    internal sealed class GetInternalUserQueryHandler : IQueryHandler<GetInternalUserQuery>
    {
        private readonly IAppUserQueryRepository appUserQueryRepository;
        private readonly IAppMemoryCache memoryCache;

        public GetInternalUserQueryHandler(IAppUserQueryRepository appUserQueryRepository, IAppMemoryCache memoryCache)
        {
            this.appUserQueryRepository = appUserQueryRepository;
            this.memoryCache = memoryCache;
        }

        public async Task<Response> Handle(GetInternalUserQuery query, CancellationToken cancellationToken)
        {
            var cacheKey = $"Valghalla-Internal-User-{query.CvrNumber}__{query.Serial}";
            var cachedValue = await memoryCache.GetOrCreateAsync(cacheKey, () => appUserQueryRepository.GetUserAsync(query, cancellationToken));

            if (cachedValue == null)
            {
                memoryCache.Remove(cacheKey);
            }

            return Response.Ok(cachedValue);
        }
    }
}
