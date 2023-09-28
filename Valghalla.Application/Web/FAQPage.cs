namespace Valghalla.Application.Web
{
    public sealed record FAQPage
    {
        public string PageContent { get; set; } = string.Empty;
        public bool IsActivated { get; set; }
    }
}
