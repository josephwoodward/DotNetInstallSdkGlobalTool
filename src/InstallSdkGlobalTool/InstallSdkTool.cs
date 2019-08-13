using System;
using System.IO;
using System.Net.Http;

namespace InstallSdkGlobalTool
{
    public static class InstallSdkTool
    {
        public static void Run()
        {
            var writer = new ConsoleTextWriter();
            var locator = new GlobalJsonLocator(writer);
            string version;

            try
            { 
                version = locator.Parse().Sdk.Version;
            }
            catch (FileNotFoundException e)
            {
                writer.WriteLine(e.Message);
                return;
            }
            
            var sdkAcquirer = new SdkAcquirer(new HttpClient(), writer, new InstallerLauncher(), new PlatformIdentifier());
            sdkAcquirer.Acquire(version).GetAwaiter().GetResult();
        }
    }
}