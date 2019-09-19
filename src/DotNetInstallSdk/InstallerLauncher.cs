using System;
using System.Diagnostics;
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
        private readonly ToolArguments _args;

        public InstallerLauncher(ITextWriter writer, ToolArguments args)
        {
            _writer = writer;
            _args = args;
        }


        void LaunchInstallerProcess(string installerPath)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = installerPath,
                UseShellExecute = true
            };
            
            Process.Start(processStartInfo)?.WaitForExit();
        }
        
        public void Launch(string installerPath)
        {
            if (!_args.Headless)
            {
                LaunchInstallerProcess(installerPath);
                return;
            }
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _writer.WriteLine($"The SDK has been downloaded to: {installerPath}");
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Run("sudo", $"installer -pkg {installerPath} -target /");
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Run(installerPath, $"/install /quiet /norestart");
                return;
            }
            
            throw new PlatformNotSupportedException();
        }
    }
}