namespace Valghalla.Application.Web
{
    public sealed record DeclarationOfConsentPage
    {
        public string PageContent { get; init; } = string.Empty;
        public bool IsActivated { get; init; }
    }
}
