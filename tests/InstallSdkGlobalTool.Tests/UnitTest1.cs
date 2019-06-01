using System;
using System.IO;
using Shouldly;
using Xunit;

namespace InstallSdkGlobalTool.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void NotifyUserGlobalJsonNotFound()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                var tool = new GlobalJsonLocator(new ConsoleTextWriter());
                tool.Parse();
                
                sw.ToString().ShouldContain("global.json could not be found in the current directory");
            }
        }
        
        [Fact]
        public void ParsesGlobalJson()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                var tool = new GlobalJsonLocator(new ConsoleTextWriter());
                var globalJson = tool.Parse();

                globalJson.Sdk.Version.ShouldBe("2.2.100");                
            }
        }
    }
}