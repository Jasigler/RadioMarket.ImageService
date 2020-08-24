using HealthCheck.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace HealthCheck.Services
{
    public class Memory
    {
        public MemoryInfo GetInfo()
        {
            var info = new ProcessStartInfo();
            info.FileName = "wmic";
            info.Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value";
            info.RedirectStandardOutput = true;

            var queryProcess = Process.Start(info);
            var output = queryProcess.StandardOutput.ReadToEnd();

            var outputLines = output.Trim().Split("\n");
            var freeMemory = outputLines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
            var totalMemory = outputLines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);

            var status = new MemoryInfo();

            status.Total = Math.Round(double.Parse(totalMemory[1]) / 1024, 0);
            status.Free = Math.Round(double.Parse(freeMemory[1]) / 1024, 0);
            status.Used = status.Total - status.Free;

            return status;

        }
    }
}
