using HealthCheck.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck.HealthChecks
{
    public class DiskHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var disk = new Disk();
            var metrics = disk.GetInfo();

            var used =  100 * metrics.Used / metrics.Total; 

            var status = HealthStatus.Healthy;

            if (used >= 75)
            {
                status = HealthStatus.Degraded;
            }
            if (used >= 80)
            {
                status = HealthStatus.Unhealthy;
            }

            var payload = new Dictionary<string, object>();
            payload.Add("Total", metrics.Total);
            payload.Add("Used", used);
            payload.Add("Free", metrics.Free);
        
            var result = new HealthCheckResult(status, null, null, payload);
            return await Task.FromResult(result);
            
        }
    }
}
