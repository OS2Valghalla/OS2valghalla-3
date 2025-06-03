namespace Valghalla.Application.Saml
{
    public interface ISaml2AuthContextProvider
    {
        Task<Saml2AuthAppConfiguration> GetSaml2AuthAppConfigurationAsync(CancellationToken cancellationToken);
        Task<Saml2AuthAppConfiguration> GetSaml2AuthAppFallbackConfigurationAsync(CancellationToken cancellationToken);
    }
}
