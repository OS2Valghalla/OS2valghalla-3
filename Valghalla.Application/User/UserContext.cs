namespace Valghalla.Application.User
{
    public sealed class UserContext
    {
        public static readonly UserContext App = new()
        {
            RoleIds = new[] { Role.Administrator.Id },
            UserId = new Guid("6504020c-3261-41f4-9ba7-ec380f7ad200"),
            Name = "$$AppUser$$"
        };

        public IEnumerable<Guid> RoleIds { get; init; } = Enumerable.Empty<Guid>();
        public Guid UserId { get; init; }
        public Guid? ParticipantId { get; init; }
        public string Name { get; init; } = null!;
        public string? Cpr { get; init; }
        public string? Cvr { get; init; }
        public string? Serial { get; init; }

        public static string GetCacheKey(string cpr) => $"User_{cpr}";
    }
}
