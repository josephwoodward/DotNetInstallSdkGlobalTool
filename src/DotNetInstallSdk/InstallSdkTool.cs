using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using DotNet.InstallSdk.Acquirables;

namespace DotNet.InstallSdk
{
    public static class InstallSdkTool
    {
        public static async Task RunAsync(Acquirable acquirable, ITextWriter writer, ToolArguments args)
        {
            try
            {
                var sdkAcquirer = new SdkAcquirer(new HttpClient(), writer, new InstallerLauncher(writer, args), new PlatformIdentifier(), new DotnetInfo());
                await sdkAcquirer.Acquire(acquirable);
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