using System.Threading.Tasks;
using DotNet.InstallSdk.Acquirables;
using DotNet.InstallSdk.Acquirables.GlobalJson;
using McMaster.Extensions.CommandLineUtils;

namespace DotNet.InstallSdk
{
    class Program
    {
        public static int Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);

        [Option("-LP|--latest-preview", Description = "Optional. Install the latest preview version of the .NET Core SDK")]
        public bool LatestPreview { get; } = false;
        
        [Option("-H|--headless <Boolean>", Description = "Optional. Install .NET Core SDK in headless mode (default is true")]
        public string Headless { set; get; }

/*
        [Option("-L|--latest", Description = "Install the latest non-preview version of the .NET Core SDK")]
        public bool Latest { get; } = false;
*/
        private async Task OnExecuteAsync()
        {
            var args = new ToolArguments
            {
                Headless = bool.Parse(Headless),
                LatestPreview = LatestPreview
            };
            
            var writer = new ConsoleTextWriter();

            var acquirable = args.LatestPreview
                ? (Acquirable) new LatestPreviewVersion(writer)
                : new GlobalJsonVersion(writer); 

            await InstallSdkTool.RunAsync(acquirable, writer, args);
        }
    }
}