using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotNet.InstallSdk.Acquirables.GlobalJson
{
    public class GlobalJsonVersion : Acquirable
    {
        private readonly ITextWriter _writer;

        public GlobalJsonVersion(ITextWriter writer)
        {
            _writer = writer;
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
            var parse = new GlobalJsonLocator(_writer).Parse();
            if (!parse.IsSuccess)
            {
                _writer.WriteLine(parse.ErrorMessage);
                return new AcquireResult();
            }

            var version = parse.GlobalJsonFile.Sdk.Version;
            var channelVersion = ParseChannelVersion(version);

            using var releasesResponse = await JsonDocument.ParseAsync(await httpClient.GetStreamAsync(ReleaseIndex));

            var channel = releasesResponse.RootElement.GetProperty("releases-index").EnumerateArray()
                .First(x => x.GetProperty("channel-version").GetString() == channelVersion);

            var channelJson = channel.GetProperty("releases.json").GetString();
            
            return new AcquireResult
            {
                ChannelJson = channelJson,
                Version = version
            };
        }
    }
}