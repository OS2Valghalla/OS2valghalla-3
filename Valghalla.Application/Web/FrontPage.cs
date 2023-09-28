namespace Valghalla.Application.Web
{
    public sealed record FrontPage
    {
        public string PageContent { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
    }
}
