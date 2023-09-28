namespace Valghalla.Internal.API.Requests.Administration.SpecialDiet
{
    public sealed record UpdateSpecialDietRequest
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
    }
}
