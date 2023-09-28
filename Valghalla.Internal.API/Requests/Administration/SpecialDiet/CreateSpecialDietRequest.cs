namespace Valghalla.Internal.API.Requests.Administration.SpecialDiet
{
    public sealed record CreateSpecialDietRequest
    {
        public string Title { get; init; } = string.Empty;
    }
}
