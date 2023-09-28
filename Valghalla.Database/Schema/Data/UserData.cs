using Valghalla.Application.User;

namespace Valghalla.Database.Schema.Data
{
    internal class UserData
    {
        internal static string InitAppUserData()
           => $@"
                insert into ""User"" values(
                    '{UserContext.App.UserId}',
                    '{UserContext.App.RoleIds.First()}',
                    '{UserContext.App.Name}',
                    null,
                    null,
                    null)
                on conflict do nothing;
                ";
    }
}
