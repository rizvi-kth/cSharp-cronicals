namespace ControllerAndWorkspacesExample.Services
{
    using System.Collections.Generic;

    public sealed class SystemInfoService : ISystemInfoService
    {
        public IDictionary<string, string> GetHardwareInfo()
        {
            var properties = new Dictionary<string, string>
                             {
                                 {
                                     "ProcessorCount",
                                     System.Environment.ProcessorCount.ToString()
                                 }
                             };

            return properties;
        }

        public IDictionary<string, string> GetSoftwareInfo()
        {
            var properties = new Dictionary<string, string>
                             {
                                 { "MachineName", System.Environment.MachineName },
                                 { "UserDomainName", System.Environment.UserDomainName },
                                 { "UserName", System.Environment.UserName },
                                 {
                                     "Is64BitOperatingSystem",
                                     System.Environment.Is64BitOperatingSystem.ToString()
                                 },
                                 {
                                     "Is64BitProcess",
                                     System.Environment.Is64BitProcess.ToString()
                                 },
                                 { "OSVersion", System.Environment.OSVersion.ToString() }
                             };

            return properties;
        }
    }
}