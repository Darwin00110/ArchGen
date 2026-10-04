using ArchGen.Domain;

namespace ArchGen.InfraStructure;

public class SystemOSService  : ISystemOSService
{
    public string GetSystemOS()
    {
        if (OperatingSystem.IsWindows())
        {
            return "Windows".ToUpper();
        }
        else if (OperatingSystem.IsLinux())
        {
            return "Linux".ToUpper();
        }
        else if (OperatingSystem.IsMacOS())
        {
            return "MacOS".ToUpper();
        }
        else
        {
            return "Unknown".ToUpper();
        }
    }
}
