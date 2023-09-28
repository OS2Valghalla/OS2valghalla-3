namespace Valghalla.Internal.API.Requests.Administration.User
{
    public sealed record UpdateUserRequest
    {
        public Guid Id { get; init; }
        public Guid RoleId { get; init; }
    }
}
