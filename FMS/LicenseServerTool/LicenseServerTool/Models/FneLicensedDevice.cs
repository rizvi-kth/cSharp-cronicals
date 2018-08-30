using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServerTool.Models
{
    
    public class FneLicensedDevice
    {
        public int Id { get; set; }
        public List<ReservationEntry> ReservationEntries { get; set; }
        public HostId HostId { get; set; }
    }

    public class ReservationEntry
    {
        public int Id { get; set; }
        public int FeatureCount { get; set; }
        public string FeatureName { get; set; }
        public string State { get; set; }
        public string FeatureVersion { get; set; }
    }

    //public class FneHostId
    //{
    //    public string Type { get; set; }
    //    public string Value { get; set; }
    //}
}
