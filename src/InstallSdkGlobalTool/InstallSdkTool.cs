using System.Net.Http;

namespace InstallSdkGlobalTool
{
    public static class InstallSdkTool
    {
        public static void Run()
        {
            var writer = new ConsoleTextWriter();
            var locator = new GlobalJsonLocator(writer);

            var version = locator.Parse().Sdk.Version;
            var sdkAcquirer = new SdkAcquirer(new HttpClient(), writer);
            sdkAcquirer.Acquire(version).GetAwaiter().GetResult();
        }
    }
}