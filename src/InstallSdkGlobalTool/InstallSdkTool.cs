namespace InstallSdkGlobalTool
{
    public static class InstallSdkTool
    {
        public static void Run()
        {
            var writer = new ConsoleTextWriter();
            var locator = new GlobalJsonLocator(writer);

            var contents = locator.Parse();
            if (string.IsNullOrWhiteSpace(contents?.Sdk?.Version));
                writer.WriteLine("Contents of global.json are invalid");
        }
    }
}