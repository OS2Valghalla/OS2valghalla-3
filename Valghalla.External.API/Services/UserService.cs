using System.Security.Claims;
using Valghalla.Application.Auth;
using Valghalla.Application.Cache;
using Valghalla.Application.Exceptions;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.Shared.User;

namespace Valghalla.External.API.Services
{
    internal class UserService : IUserService
    {
        private readonly IUserSharedQueryRepository userSharedQueryRepository;
        private readonly ITenantMemoryCache tenantMemoryCache;

        public UserService(IUserSharedQueryRepository userSharedQueryRepository, ITenantMemoryCache tenantMemoryCache)
        {
            this.userSharedQueryRepository = userSharedQueryRepository;
            this.tenantMemoryCache = tenantMemoryCache;
        }

        public async Task<UserInfo?> GetUserInfoAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            var cpr = claimsPrincipal.GetCpr();

            if (string.IsNullOrEmpty(cpr))
            {
                throw new UserException("CPR could not be found in claims principal.");
            }

            var cacheKey = UserContext.GetCacheKey(cpr);
            var cachedValue = await tenantMemoryCache.GetOrCreateAsync(cacheKey, cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(15);
                return userSharedQueryRepository.GetUserInfoAsync(cpr, cancellationToken);
            });

            if (cachedValue == null)
            {
                tenantMemoryCache.Remove(cacheKey);
            }

            return cachedValue;
        }
    }
}
