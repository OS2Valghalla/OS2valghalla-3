namespace Valghalla.Application.TaskValidation
{
    public sealed record EvaluatedParticipant
    {
        public Guid Id { get; init; }
        public DateTime Birthdate { get; init; }
        public string? MunicipalityCode { get; set; }
        public string? CountryCode { get; set; }
        public bool Deceased { get; init; }
        public bool Disenfranchised { get; init; }
    }
}
