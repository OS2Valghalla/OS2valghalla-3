using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Cache;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.App.Interfaces;

namespace Valghalla.External.Application.Modules.App.Queries
{
    public sealed record GetExternalUserQuery(string CprNumber) : IQuery<Response>;

    public sealed class GetExternalUserQueryValidator : AbstractValidator<GetExternalUserQuery>
    {
        public GetExternalUserQueryValidator()
        {
            RuleFor(x => x.CprNumber)
                .NotEmpty();
        }
    }

    internal class GetExternalUserQueryHandler : IQueryHandler<GetExternalUserQuery>
    {
        private readonly IAppQueryRepository appQueryRepository;
        private readonly ITenantMemoryCache tenantMemoryCache;

        public GetExternalUserQueryHandler(IAppQueryRepository appQueryRepository, ITenantMemoryCache tenantMemoryCache)
        {
            this.appQueryRepository = appQueryRepository;
            this.tenantMemoryCache = tenantMemoryCache;
        }

        public async Task<Response> Handle(GetExternalUserQuery query, CancellationToken cancellationToken)
        {
            var cacheKey = UserContext.GetCacheKey(query.CprNumber);
            var cachedValue = await tenantMemoryCache.GetOrCreateAsync(cacheKey, () => appQueryRepository.GetUserAsync(query, cancellationToken));

            if (cachedValue == null)
            {
                tenantMemoryCache.Remove(cacheKey);
            }

            return Response.Ok(cachedValue);
        }
    }
}
