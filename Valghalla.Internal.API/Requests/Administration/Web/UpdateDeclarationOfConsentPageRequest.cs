namespace Valghalla.Internal.API.Requests.Administration.Web
{
    public class UpdateDeclarationOfConsentPageRequest
    {
        public string PageContent { get; init; } = string.Empty;
        public bool IsActivated { get; init; }
    }
}
