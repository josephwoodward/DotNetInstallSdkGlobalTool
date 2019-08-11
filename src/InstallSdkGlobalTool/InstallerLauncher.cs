using System.Diagnostics;

namespace InstallSdkGlobalTool
{
    public interface IInstallerLauncher
    {
        void Launch(string installerPath);
    }

    public class InstallerLauncher : IInstallerLauncher
    {
        public void Launch(string installerPath)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = installerPath,
                UseShellExecute = true
            };

            Process.Start(processStartInfo);
        }
    }
}