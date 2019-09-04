namespace DotNet.InstallSdk.Acquirables
{
    public class AcquireResult
    {
        public bool IsSuccess 
            => !string.IsNullOrWhiteSpace(Version) && !string.IsNullOrWhiteSpace(ChannelJson);
        
        public string Version { get; set; }

        public string ChannelJson { get; set; }
    }
}