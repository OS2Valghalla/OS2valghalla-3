namespace Valghalla.Application.Web
{
    public sealed record WebPage
    {
        public string PageName { get; set; } = null!;

        public string PageInfo { get; set; } = null!;
    }
}
