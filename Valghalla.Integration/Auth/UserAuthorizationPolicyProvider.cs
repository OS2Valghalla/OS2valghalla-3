using ITfoxtec.Identity.Saml2.Schemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

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
            var policy = new AuthorizationPolicyBuilder(Saml2Constants.AuthenticationScheme)
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
                var policy = new AuthorizationPolicyBuilder(Saml2Constants.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .AddRequirements(new UserAuthorizationRequirement(roleId))
                    .Build();

                return Task.FromResult(policy);
            }

            return backupPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
