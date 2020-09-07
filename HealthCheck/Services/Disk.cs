using HealthCheck.Models;
using System.IO;

namespace HealthCheck.Services
{
    public class Disk
    {
        public DiskInfo GetInfo()
        {
            DriveInfo hostingDrive = new DriveInfo("D:\\");

            var totalSpace = hostingDrive.TotalSize / 1048576;
            var freeSpace = hostingDrive.TotalFreeSpace / 1048576;
            var UsedSpace = totalSpace - freeSpace;

            var result = new DiskInfo();
            result.Total = totalSpace;
            result.Free = freeSpace;
            result.Used = UsedSpace;

            return result;
        }
    }
}