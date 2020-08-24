using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCheck.Models
{
    public class DiskInfo
    {
       public long Total { get; set; }
        public long Used { get; set; }
        public long Free { get; set; }

    }
}
