using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotNet.InstallSdk
{
    public static class InstallSdkTool
    {
        public static void Run()
        {
            var writer = new ConsoleTextWriter();
            var locator = new GlobalJsonLocator(writer);

            try
            {
                var version = locator.Parse().Sdk.Version;

                var sdkAcquirer = new SdkAcquirer(new HttpClient(), writer, new InstallerLauncher(), new PlatformIdentifier());
                sdkAcquirer.Acquire(version).GetAwaiter().GetResult();
            }
            catch (FileNotFoundException e)
            {
                writer.WriteLine(e.Message);
            }
            catch (TaskCanceledException e)
            {
                if (!e.CancellationToken.IsCancellationRequested)
                    writer.WriteLine("Connection to acquire .NET SDK timed out, please try again");
            }
        }
    }
}