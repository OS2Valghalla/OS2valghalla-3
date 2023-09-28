using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Valghalla.Internal.API.HealthChecks.Responses
{
    public record IndividualHealthCheckResponse(HealthStatus Status, string Description, TimeSpan Duration, IReadOnlyDictionary<string, object> Data, IEnumerable<string> Tags);

}
