using System;

namespace InstallSdkGlobalTool
{
    public interface ITextWriter
    {
        void WriteLine(string message);
        void SetCursorPosition(int left, int top);
        void Write(string value);
    }

    public class ConsoleTextWriter : ITextWriter
    {
        public void WriteLine(string message) 
            =>  Console.WriteLine(message);

        public void SetCursorPosition(int left, int top) => Console.SetCursorPosition(left, top);

        public void Write(string value) => Console.Write(value);
    }
}