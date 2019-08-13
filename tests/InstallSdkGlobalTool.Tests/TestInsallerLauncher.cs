namespace InstallSdkGlobalTool.Tests
{
    public class TestInstallerLauncher : IInstallerLauncher
    {
        public void Launch(string installerPath)
        {
            LastLaunchedInstaller = installerPath;
        }

        public string LastLaunchedInstaller { get; private set; }
    }
}