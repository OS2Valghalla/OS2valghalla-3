using Valghalla.Application.Web;

namespace Valghalla.External.Application.Modules.App.Responses
{
    public sealed record AppContextResponse
    {
        public bool ElectionActivated { get; init; }
        public bool FAQPageActivated { get; init; }
        public ElectionCommitteeContactInformationPage? WebPage { get; init; }
    }
}
