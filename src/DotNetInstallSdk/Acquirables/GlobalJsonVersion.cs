using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotNet.InstallSdk.Acquirables
{
    public class GlobalJsonVersion : Acquirable
    {
        readonly string _version;

        public GlobalJsonVersion(ITextWriter writer)
        {
            _version = new GlobalJsonLocator(writer).Parse().Sdk.Version;
        }

        
        static string ParseChannelVersion(string version)
        {
            var channelVersion = version.Substring(0, 3);
            if (!char.IsDigit(channelVersion[0]) || channelVersion[1] != '.' || !char.IsDigit(channelVersion[2]))
                throw new ArgumentException(@"Parsing channel version failed, expected a major.minor format. e.g. ""2.1""", nameof(version));
            return channelVersion;
        }

        public override async Task<AcquireResult> Fetch(HttpClient httpClient)
        {
            var channelVersion = ParseChannelVersion(_version);

            using var releasesResponse = await JsonDocument.ParseAsync(await httpClient.GetStreamAsync(ReleaseIndex));

            var channel = releasesResponse.RootElement.GetProperty("releases-index").EnumerateArray()
                .First(x => x.GetProperty("channel-version").GetString() == channelVersion);

            var channelJson = channel.GetProperty("releases.json").GetString();
            
            return new AcquireResult
            {
                ChannelJson = channelJson,
                Version = _version
            };
        }
    }

    public class AcquireResult
    {
        public string Version { get; set; }

        public string ChannelJson { get; set; }
    }
}