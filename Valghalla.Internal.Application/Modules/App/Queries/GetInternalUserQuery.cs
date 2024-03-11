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
        private readonly ITenantMemoryCache tenantMemoryCache;

        public GetInternalUserQueryHandler(IAppUserQueryRepository appUserQueryRepository, ITenantMemoryCache tenantMemoryCache)
        {
            this.appUserQueryRepository = appUserQueryRepository;
            this.tenantMemoryCache = tenantMemoryCache;
        }

        public async Task<Response> Handle(GetInternalUserQuery query, CancellationToken cancellationToken)
        {
            var cacheKey = $"Valghalla-Internal-User-{query.CvrNumber}__{query.Serial}";
            var cachedValue = await tenantMemoryCache.GetOrCreateAsync(cacheKey, () => appUserQueryRepository.GetUserAsync(query, cancellationToken));

            if (cachedValue == null)
            {
                tenantMemoryCache.Remove(cacheKey);
            }

            return Response.Ok(cachedValue);
        }
    }
}
