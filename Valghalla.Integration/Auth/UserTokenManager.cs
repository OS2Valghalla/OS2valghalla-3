using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Valghalla.Application;
using Valghalla.Application.Auth;
using Valghalla.Application.Cache;

namespace Valghalla.Integration.Auth
{
    internal class UserTokenManager : IUserTokenManager
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ITenantMemoryCache tenantMemoryCache;
        private readonly IUserTokenRepository userTokenRepository;

        private HttpContext HttpContext
        {
            get
            {
                return httpContextAccessor.HttpContext!;
            }
        }

        private UserToken.TokenKey? _currentTokenKey = null;
        private UserToken.TokenKey? CurrentTokenKey
        {
            get
            {
                if (_currentTokenKey == null)
                {
                    var stringValue = HttpContext.Request.Cookies[Constants.Authentication.Cookie];
                    if (stringValue != null)
                    {
                        _currentTokenKey = UserToken.Decode(stringValue);
                    }
                }

                return _currentTokenKey;
            }
            set
            {
                _currentTokenKey = value;
            }
        }

        public UserTokenManager(
            IHttpContextAccessor httpContextAccessor,
            ITenantMemoryCache tenantMemoryCache,
            IUserTokenRepository userTokenRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.tenantMemoryCache = tenantMemoryCache;
            this.userTokenRepository = userTokenRepository;
        }

        public void ExpireUserToken() => EnsureTokenExpiredInResponseCookie();

        public async Task<UserToken?> EnsureUserTokenAsync(CancellationToken cancellationToken)
        {
            if (CurrentTokenKey == null) return null;

            var cachedToken = await EnsureTokenCacheAsync(CurrentTokenKey, cancellationToken);

            if (cachedToken == null || !cachedToken.Valid)
            {
                EnsureTokenExpiredInResponseCookie();
                return null;
            }

            if (cachedToken.Renewable)
            {
                return await EnsureUserTokenAsync(cachedToken.ToClaimsPrincipal(), cancellationToken);
            }

            return cachedToken;
        }

        public async Task<UserToken?> EnsureUserTokenAsync(ClaimsPrincipal principal, CancellationToken cancellationToken)
        {
            var newToken = UserToken.CreateToken(principal);
            await userTokenRepository.AddUserTokenAsync(newToken, cancellationToken);

            CurrentTokenKey = newToken.Key;
            EnsureTokenInResponseCookie(newToken);

            return await EnsureTokenCacheAsync(newToken.Key, cancellationToken);
        }

        private async Task<UserToken?> EnsureTokenCacheAsync(UserToken.TokenKey tokenKey, CancellationToken cancellationToken)
        {
            return await tenantMemoryCache.GetOrCreateAsync(BuildCacheKey(tokenKey), async cacheEntry =>
            {
                var tokens = await userTokenRepository.GetUserTokensAsync(tokenKey.Identifier, cancellationToken);
                var matchToken = tokens.FirstOrDefault(i => i.Key.Identifier == tokenKey.Identifier && i.Key.Code == tokenKey.Code);

                if (matchToken != null && matchToken.Valid)
                {
                    cacheEntry.AbsoluteExpiration = matchToken.ExpiredAt;
                }
                else
                {
                    // avoid calling to DB too much for non exist or expired token
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                }

                return matchToken;
            });
        }

        private void EnsureTokenInResponseCookie(UserToken token)
        {
            var cookieValue = UserToken.Encode(token.Key);
            var cookieOptions = GetCookieOptions();
            cookieOptions.Expires = token.ExpiredAt;

            HttpContext.Response.Cookies.Append(Constants.Authentication.Cookie, cookieValue, cookieOptions);
        }

        private void EnsureTokenExpiredInResponseCookie()
        {
            var cookieOptions = GetCookieOptions();
            cookieOptions.Expires = DateTime.UtcNow.AddYears(-1);

            HttpContext.Response.Cookies.Append(Constants.Authentication.Cookie, string.Empty, cookieOptions);
        }

        private CookieOptions GetCookieOptions() => new()
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
            Domain = HttpContext.Request.Host.Host
        };

        private static string BuildCacheKey(UserToken.TokenKey tokenKey) => $"{Constants.Authentication.Cookie}::{tokenKey.Identifier}";
    }
}
