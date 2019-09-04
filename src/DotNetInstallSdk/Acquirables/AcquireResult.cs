using System.ComponentModel;

namespace DotNet.InstallSdk.Acquirables
{
    public class AcquireResult
    {
        public bool IsSuccess { get; set; }
        public string Version { get; set; }

        public string ChannelJson { get; set; }
    }
}