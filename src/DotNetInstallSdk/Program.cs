using McMaster.Extensions.CommandLineUtils;

namespace DotNet.InstallSdk
{
    class Program
    {
        public static int Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);

/*
        [Option("-LP|--latest-preview", Description = "Install the latest preview version of the .NET Core SDK")]
        public bool LatestPreview { get; } = false;

        [Option("-L|--latest", Description = "Install the latest non-preview version of the .NET Core SDK")]
        public bool Latest { get; } = false;
*/
        private void OnExecute()
        {
            InstallSdkTool.Run(new CommandLineArguments
            {
                Latest = false,
                LatestPreview = false
            });
        }
    }
}