using System;
using System.Runtime.InteropServices;
using static SimpleExec.Command;

namespace DotNet.InstallSdk
{
    public interface IInstallerLauncher
    {
        void Launch(string installerPath);
    }

    public class InstallerLauncher : IInstallerLauncher
    {
        readonly ITextWriter _writer;

        public InstallerLauncher(ITextWriter writer)
        {
            _writer = writer;
        }
        
        public void Launch(string installerPath)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _writer.WriteLine($"The SDK has been downloaded to: {installerPath}");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Run("sudo", $"installer -pkg {installerPath} -target /");
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Run(installerPath, $"/install /quiet /norestart");
            }
            
            throw new PlatformNotSupportedException();
        }
    }
}