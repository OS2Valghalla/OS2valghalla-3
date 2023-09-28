namespace Valghalla.External.Application.Modules.Shared.SpecialDiet.Responses
{
    public sealed record SpecialDietSharedResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
    }
}
