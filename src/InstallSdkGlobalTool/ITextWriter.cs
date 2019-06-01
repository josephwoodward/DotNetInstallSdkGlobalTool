using System;

namespace InstallSdkGlobalTool
{
    public interface ITextWriter
    {
        void WriteLine(string message);
    }

    public class ConsoleTextWriter : ITextWriter
    {
        public void WriteLine(string message) 
            =>  Console.WriteLine(message);
    }
}