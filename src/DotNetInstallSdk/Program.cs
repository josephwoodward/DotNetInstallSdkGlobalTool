using System.Threading.Tasks;
using DotNet.InstallSdk.Acquirables;
using DotNet.InstallSdk.Acquirables.GlobalJson;
using DotNet.InstallSdk.Acquirables.LatestNonPreview;
using DotNet.InstallSdk.Acquirables.LatestPreview;
using McMaster.Extensions.CommandLineUtils;

namespace DotNet.InstallSdk
{
    class Program
    {
        public static int Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);

        [Option("-LP|--latest-preview", Description = "Optional. Install the latest preview version of the .NET Core SDK")]
        public bool LatestPreview { get; } = false;

        [Option("-L|--latest", Description = "Install the latest non-preview version of the .NET Core SDK")]
        public bool Latest { get; } = false;
        
        [Option("-H|--headless <Boolean>", Description =
            "Optional. Install .NET Core SDK in headless mode (default: true)")]
        public string Headless { set; get; } = "true";
        
        private async Task OnExecuteAsync()
        {
            var args = new ToolArguments
            {
                LatestPreview = LatestPreview,
                Headless = bool.TryParse(Headless, out var headless) && headless,
                Latest = Latest
            };

            var writer = new ConsoleTextWriter();
            
            if (args.Headless)
                writer.WriteLine("Running in headless mode");

            Acquirable acquirable = new GlobalJsonVersion(writer);
            if (args.LatestPreview)
                acquirable = new LatestPreviewVersion(writer);
            if (args.Latest)
                acquirable = new LatestNonPreviewVersion(writer);

            await InstallSdkTool.RunAsync(acquirable, writer, args);
        }
    }
}