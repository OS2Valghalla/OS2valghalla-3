using Microsoft.AspNetCore.Authorization;
using Valghalla.Application.User;

namespace Valghalla.Integration.Auth
{
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        public static readonly string POLICY_PREFIX = "User";

        public UserAuthorizeAttribute(RoleEnum role)
        {
            RoleId = role switch
            {
                RoleEnum.Administrator =>  Role.Administrator.Id,
                RoleEnum.TeamResponsible => Role.TeamResponsible.Id,
                RoleEnum.WorkLocationResponsible => Role.WorkLocationResponsible.Id,
                _ => Role.Participant.Id,
            };
        }

        public Guid RoleId
        {
            get
            {
                if (Guid.TryParse(Policy.AsSpan(POLICY_PREFIX.Length), out var roleId))
                {
                    return roleId;
                }

                return Guid.Empty;
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value}";
            }
        }
    }
}
