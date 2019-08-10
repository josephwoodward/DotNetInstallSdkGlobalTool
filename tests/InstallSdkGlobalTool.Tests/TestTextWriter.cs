using System;

namespace InstallSdkGlobalTool.Tests
{
    public class TestTextWriter : ITextWriter
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public void SetCursorPosition(int left, int top)
        {
            Console.SetCursorPosition(left, top);
        }

        public void Write(string value)
        {
            Console.Write(value);
        }
    }
}