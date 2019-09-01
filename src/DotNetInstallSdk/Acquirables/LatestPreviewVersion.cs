using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotNet.InstallSdk.Acquirables
{
    public class LatestPreviewVersion : Acquirable
    {
        public LatestPreviewVersion(ITextWriter writer)
        {
        }

        public override async Task<AcquireResult> Fetch(HttpClient httpClient)
        {
            using var releasesResponse = await JsonDocument.ParseAsync(await httpClient.GetStreamAsync(ReleaseIndex));

            var channel = releasesResponse.RootElement.GetProperty("releases-index").EnumerateArray()
                .First(x => x.GetProperty("support-phase").GetString() == "preview");

            return new AcquireResult
            {
                ChannelJson = channel.GetProperty("releases.json").GetString(),
                Version = channel.GetProperty("latest-sdk").GetString()
            };
        }
    }
}