namespace Valghalla.Internal.API.HealthChecks.Responses
{
    public record HealthCheckReponse(string Status, IDictionary<string, IndividualHealthCheckResponse> Entries, TimeSpan TotalDuration);
}
