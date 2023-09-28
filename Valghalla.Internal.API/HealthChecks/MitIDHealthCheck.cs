using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Valghalla.Internal.API.HealthChecks
{
    public class MitIDHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            try
            {
                return Task.FromResult(HealthCheckResult.Healthy("The service is up and running."));
            }
            catch (Exception)
            {
                return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, "The service is down."));
            }
        }
    }
}
