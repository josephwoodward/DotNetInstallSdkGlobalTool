using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using DotNet.InstallSdk.Acquirables;

namespace DotNet.InstallSdk
{
    public static class InstallSdkTool
    {
        public static void Run(CommandLineArguments args)
        {
            var writer = new ConsoleTextWriter();

            try
            {
                var v = new GlobalJsonVersion(writer);
                
                var sdkAcquirer = new SdkAcquirer(new HttpClient(), writer, new InstallerLauncher(), new PlatformIdentifier());
                sdkAcquirer.Acquire(v).GetAwaiter().GetResult();
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