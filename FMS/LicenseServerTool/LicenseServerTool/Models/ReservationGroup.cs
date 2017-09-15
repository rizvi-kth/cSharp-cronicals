using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServerTool.Models
{
    public class ReservationGroup
    {
        public List<Reservation> Reservations { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public string CreationDate { get; set; }
    }

    public class Reservation
    {
        public HostId HostId { get; set; }
        public List<ToolsTalkReservationEntry> ReservationEntries { get; set; }
    }

    public class HostId
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class ToolsTalkReservationEntry
    {
        public int Id { get; set; }
        public int FeatureCount { get; set; }
        public FeatureType FeatureType { get; set; }
        public string State { get; set; }
        public string FeatureVersion { get; set; }
    }

    public class AddReservationEntry
    {
        public string featureName { get; set; }
        public int featureCount { get; set; }
        public string featureVersion { get; set; }
    }

    public enum FeatureType
    {
        // Named as in the license server, do not change!
        VirtualStation,
        TurboTight,
        TrueAngle,
        Line_Manager,
        Line_Configurator,
        SoftPLC,
        Yield,
        Gradient,
        Unknown
    }
}
