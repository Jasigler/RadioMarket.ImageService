using HealthCheck.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck.HealthChecks
{
    public class MemoryHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var memory = new Memory();
            var metrics = memory.GetInfo();
            var used = 100 * metrics.Used / metrics.Total;

            var status = HealthStatus.Healthy;

            if (used >= 80)
            {
                status = HealthStatus.Degraded;
            }
            if (used >= 90)
            {
                status = HealthStatus.Unhealthy;
            }

            var responseData = new Dictionary<string, object>();
            responseData.Add("Total", metrics.Total);
            responseData.Add("Used", metrics.Used);
            responseData.Add("Free", metrics.Free);

            var result = new HealthCheckResult(status, null, null, responseData);

            return await Task.FromResult(result);
        }
    }
}