using Valghalla.Application.CPR;

namespace Valghalla.Internal.Application.Modules.Participant.Responses
{
    public record ParticipantDetailResponse : ParticipantPersonalRecord
    {
        public Guid Id { get; init; }
        public string Cpr { get; init; } = null!;
        public string? MobileNumber { get; init; }
        public string? Email { get; init; }
        public IEnumerable<Guid> SpecialDietIds { get; init; } = Enumerable.Empty<Guid>();
        public IEnumerable<Guid> MemberTeamIds { get; init; } = Enumerable.Empty<Guid>();
    }
}
