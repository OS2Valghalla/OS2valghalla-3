﻿namespace Valghalla.Application.User
{
    public sealed record UserInfo
    {
        public Guid Id { get; init; }
        public Guid? ParticipantId { get; init; }
        public string Name { get; init; } = null!;
        public IEnumerable<Guid> RoleIds { get; init; } = Enumerable.Empty<Guid>();
    }
}
