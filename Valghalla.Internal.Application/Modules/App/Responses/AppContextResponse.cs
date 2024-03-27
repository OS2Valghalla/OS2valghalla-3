using Valghalla.Application.User;

namespace Valghalla.Internal.Application.Modules.App.Responses
{
    public sealed record AppContextResponse
    {
        public UserInfo User { get; init; } = null!;
        public AppElectionResponse? Election { get; init; } = null!;
        public string? MunicipalityName { get; init; } = null!;
        public string? ExternalWebUrl { get; init; } = null!;
    }
}
