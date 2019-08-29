namespace DotNet.InstallSdk.Tests
{
    public class TestPlatformIdentifier : IPlatformIdentifier
    {
        public string GetPlatform() => "osx-x64";
    }
}