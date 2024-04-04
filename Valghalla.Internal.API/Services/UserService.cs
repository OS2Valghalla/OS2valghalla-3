using System.Security.Claims;
using Valghalla.Application.Auth;
using Valghalla.Application.Cache;
using Valghalla.Application.Exceptions;
using Valghalla.Application.User;
using Valghalla.Internal.Application.Modules.Shared.User;

namespace Valghalla.Internal.API.Services
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
            var cvrNumber = claimsPrincipal.GetCvr();
            var serial = claimsPrincipal.GetSerial();

            if (string.IsNullOrEmpty(cvrNumber) || string.IsNullOrEmpty(serial))
            {
                throw new UserException("CVR and Serial could not be found in claims principal.");
            }

            var cacheKey = $"Valghalla-Internal-User-{cvrNumber}__{serial}";

            var cachedValue = await tenantMemoryCache.GetOrCreateAsync(cacheKey, cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(15);
                return userSharedQueryRepository.GetUserInfoAsync(cvrNumber, serial, cancellationToken);
            });

            if (cachedValue == null)
            {
                tenantMemoryCache.Remove(cacheKey);
            }

            return cachedValue;
        }
    }
}
