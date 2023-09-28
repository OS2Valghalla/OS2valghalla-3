namespace Valghalla.Application.Configuration.Interfaces
{
    public interface IConfigurationRepository
    {
        Task<T> GetConfigurationAsync<T>(CancellationToken cancellationToken) where T : IConfiguration;
        Task<object> GetConfigurationAsync(Type type, CancellationToken cancellationToken);
    }
}
