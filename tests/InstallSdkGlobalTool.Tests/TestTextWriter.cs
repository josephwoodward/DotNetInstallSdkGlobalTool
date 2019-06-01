using System;

namespace InstallSdkGlobalTool.Tests
{
    public class TestTextWriter : ITextWriter
    {
        
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}