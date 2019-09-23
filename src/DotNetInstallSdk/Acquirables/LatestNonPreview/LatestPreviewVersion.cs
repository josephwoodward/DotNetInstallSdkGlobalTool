using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotNet.InstallSdk.Acquirables.LatestNonPreview
{
    public class LatestNonPreviewVersion : Acquirable
    {
        public LatestNonPreviewVersion(ITextWriter writer)
        {
        }

        public override async Task<AcquireResult> Fetch(HttpClient httpClient)
        {
            using var releasesResponse = await JsonDocument.ParseAsync(await httpClient.GetStreamAsync(ReleaseIndex));

            var channel = releasesResponse.RootElement.GetProperty("releases-index").EnumerateArray()
                .First(x => x.GetProperty("support-phase").GetString() == "current");

            return new AcquireResult
            {
                ChannelJson = channel.GetProperty("releases.json").GetString(),
                Version = channel.GetProperty("latest-sdk").GetString()
            };
        }
    }
}