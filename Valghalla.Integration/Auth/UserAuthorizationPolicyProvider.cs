using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Valghalla.Application;

namespace Valghalla.Integration.Auth
{
    internal class UserAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider backupPolicyProvider;

        public UserAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            backupPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            var policy = new AuthorizationPolicyBuilder(Constants.Authentication.Scheme)
                .RequireAuthenticatedUser()
                .Build();

            return Task.FromResult(policy);
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => backupPolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(UserAuthorizeAttribute.POLICY_PREFIX, StringComparison.OrdinalIgnoreCase) &&
                Guid.TryParse(policyName.AsSpan(UserAuthorizeAttribute.POLICY_PREFIX.Length), out var roleId))
            {
                var policy = new AuthorizationPolicyBuilder(Constants.Authentication.Scheme)
                    .RequireAuthenticatedUser()
                    .AddRequirements(new UserAuthorizationRequirement(roleId))
                    .Build();

                return Task.FromResult(policy);
            }

            return backupPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
