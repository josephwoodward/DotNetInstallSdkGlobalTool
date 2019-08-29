using System;
using System.IO;
using System.Text;
using JustEat.HttpClientInterception;
using Shouldly;
using Xunit;

namespace DotNet.InstallSdk.Tests
{
    public class UnitTest1
    {
        [Fact(Skip="Manual test for now")]
        public void NotifyUserGlobalJsonNotFound()
        {
            using var sw = new StringWriter();
            Console.SetOut(sw);

            var tool = new GlobalJsonLocator(new ConsoleTextWriter());
            tool.Parse();

            sw.ToString().ShouldContain("global.json could not be found in the current directory");
        }

        [Fact]
        public void ParseGlobalJson()
        {
            var textWriter = new ConsoleTextWriter();
            var tool = new GlobalJsonLocator(textWriter);
            var globalJson = tool.Parse();
            (globalJson?.Sdk?.Version ?? throw new ArgumentNullException()).ShouldBe("2.2.100");
        }
        
        [Fact]
        public void AcquireSdk()
        {
            using var sw = new StringWriter();
            Console.SetOut(sw);

            var textWriter = new ConsoleTextWriter();
            var installerLauncher = new TestInstallerLauncher();
            var platformIdentifier = new TestPlatformIdentifier();
            
            var options = new HttpClientInterceptorOptions();

            _ = new HttpRequestInterceptionBuilder()
                .Requests()
                .ForHttps()
                .ForGet()
                .ForHost("raw.githubusercontent.com")
                .ForPath("/dotnet/core/master/release-notes/releases-index.json")
                .Responds()
                .WithContentStream(() => File.OpenRead("./TestReleaseData/releases-index.json"))
                .RegisterWith(options);

            _ = new HttpRequestInterceptionBuilder()
                .Requests()
                .ForHttps()
                .ForHost("dotnetcli.blob.core.windows.net")
                .ForGet()
                .ForPath("/dotnet/release-metadata/2.2/releases.json")
                .Responds()
                .WithContentStream(() => File.OpenRead("./TestReleaseData/2.2-releases.json"))
                .RegisterWith(options);
            
            _ = new HttpRequestInterceptionBuilder()
                .Requests()
                .ForHttps()
                .ForHost("download.visualstudio.microsoft.com")
                .ForGet()
                .ForPath("/download/pr/29457b8f-6262-4c4b-8a54-eef308346842/3c7ec575796a2ef0e826a07ca4d13084/dotnet-sdk-2.2.100-osx-x64.pkg")
                .Responds()
                .WithContentStream(() => new MemoryStream(Encoding.UTF8.GetBytes("install me")))
                .RegisterWith(options);
            
            using var httpClient = options.ThrowsOnMissingRegistration().CreateHttpClient();

            new SdkAcquirer(httpClient, textWriter, installerLauncher, platformIdentifier)
                .Acquire("2.2.100").Wait();

            var installerPath = installerLauncher.LastLaunchedInstaller;
            installerPath.ShouldNotBeNullOrEmpty();
            File.ReadAllLines(installerPath).ShouldHaveSingleItem().ShouldBe("install me");
        }
    }
}