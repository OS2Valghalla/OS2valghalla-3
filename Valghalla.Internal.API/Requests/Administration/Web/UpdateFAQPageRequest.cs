namespace Valghalla.Internal.API.Requests.Administration.Web
{
    public class UpdateFAQPageRequest
    {
        public string PageContent { get; init; } = string.Empty;
        public bool IsActivated { get; init; }
    }
}
