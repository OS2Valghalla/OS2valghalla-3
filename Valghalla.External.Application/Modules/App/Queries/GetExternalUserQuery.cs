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
        private readonly IAppMemoryCache appMemoryCache;

        public GetExternalUserQueryHandler(IAppQueryRepository appQueryRepository, IAppMemoryCache appMemoryCache)
        {
            this.appQueryRepository = appQueryRepository;
            this.appMemoryCache = appMemoryCache;
        }

        public async Task<Response> Handle(GetExternalUserQuery query, CancellationToken cancellationToken)
        {
            var cacheKey = UserContext.GetCacheKey(query.CprNumber);
            var cachedValue = await appMemoryCache.GetOrCreateAsync(cacheKey, () => appQueryRepository.GetUserAsync(query, cancellationToken));

            if (cachedValue == null)
            {
                appMemoryCache.Remove(cacheKey);
            }

            return Response.Ok(cachedValue);
        }
    }
}
