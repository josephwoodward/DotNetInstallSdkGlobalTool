using System;
using System.IO;
using System.Net.Http;
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

                var textWriter = new ConsoleTextWriter();
                var tool = new GlobalJsonLocator(textWriter);
                var globalJson = tool.Parse();
                
                new SdkAcquirer(new HttpClient(), textWriter).Acquire(globalJson?.Sdk?.Version).Wait();

                (globalJson?.Sdk?.Version ?? throw new ArgumentNullException()).ShouldBe("2.2.100");
            }
        }
    }
}