using System.Text.Json.Serialization;

namespace Valghalla.Internal.Application.Modules.Administration.User.Responses
{
    public sealed record GetUsersResponse
    {
        public List<GetUsersItem> Items { get; set; } = new List<GetUsersItem>();
        public int Total { get; set; } = 0;
    }

    public sealed record GetUsersItem
    {
        public Guid Id { get; private set; }

        public Guid RoleId { get; private set; }

        public string Name { get; private set; } = null!;

        public bool Activated
        {
            get
            {
                return !string.IsNullOrEmpty(Identifier) || !string.IsNullOrEmpty(SocialSecurityNumber);
            }
        }

        [JsonIgnore]
        public string Identifier { get; private set; } = null!;

        [JsonIgnore]
        public string SocialSecurityNumber { get; private set; } = null!;
    }
}
