using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServerTool.Models
{
    public class LLS
    {
        public string Version { get; set; }
        public string BuildDate { get; set; }
        public string BuildVersion { get; set; }
        public string PatchLevel { get; set; }
        public string Branch { get; set; }
        public Database Database { get; set; }
    }

    public class Database
    {
        public string ConnectionCheck { get; set; }
    }
}
