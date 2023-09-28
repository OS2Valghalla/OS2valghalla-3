namespace Valghalla.Internal.Application.Modules.Shared.Participant.Responses
{
    public sealed record ParticipantSharedResponse
    {
        public Guid Id { get; init; }
        public string Cpr { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public DateTime Birthdate { get; init; }
    }
}
