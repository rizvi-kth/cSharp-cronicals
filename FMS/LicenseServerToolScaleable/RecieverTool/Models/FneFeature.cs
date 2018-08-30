using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServerTool.Models
{
    public class FneFeature
    {
        public int Id { get; set; }
        public string Issuer { get; set; }
        public int Used { get; set; }
        public string FeatureName { get; set; }
        public string Expiry { get; set; }
        public string Issued { get; set; }
        public string Type { get; set; }
        public int OverdraftCount { get; set; }
        public int RenewInterval { get; set; }
        public string Starts { get; set; }
        public int MeteredUndoInterval { get; set; }
        public string FeatureVersion { get; set; }
        public string Notice { get; set; }
        public bool MeteredReusable { get; set; }
        public string Vendor { get; set; }
        public string ReceivedTime { get; set; }
        public int BorrowInterval { get; set; }
        public string FeatureId { get; set; }
        public string FeatureKind { get; set; }
        public object FeatureCount { get; set; }
        public int Reserved { get; set; }
        public bool Uncounted { get; set; }
        public bool Metered { get; set; }
        public bool Concurrent { get; set; }
        public bool UncappedOverdraft { get; set; }
    }
}
