namespace ControllerAndWorkspacesExample.Services
{
    using System.Collections.Generic;

    public interface ISystemInfoService
    {
        IDictionary<string, string> GetHardwareInfo();

        IDictionary<string, string> GetSoftwareInfo();
    }
}