using System;
using System.Runtime.InteropServices;

namespace InstallSdkGlobalTool
{
    public interface IPlatformIdentifier
    {
        string GetPlatform();
    }

    public class PlatformIdentifier : IPlatformIdentifier
    {
        public string GetPlatform()
        {
            var architecture = Environment.Is64BitOperatingSystem ? "x64" : "x32";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return $"linux-{architecture}";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return $"osx-{architecture}";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return $"win-{architecture}";
            throw new PlatformNotSupportedException();
        }
    }
}