namespace InstallSdkGlobalTool
{
    public class GlobalJsonFile
    {
        public Sdk Sdk { get; set; }
    }

    public class Sdk
    {
        public string Version { get; set; } = string.Empty;
    }
}