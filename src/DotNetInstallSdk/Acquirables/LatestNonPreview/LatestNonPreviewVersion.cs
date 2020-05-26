using System;
using System.IO;
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
                .FirstOrDefault(x => x.GetProperty("support-phase").GetString() == "lts" || x.GetProperty("support-phase").GetString() == "current");

            if (channel.ValueKind == JsonValueKind.Undefined)
            {
                throw new FileNotFoundException("No non-preview version could be found");
            }

            return new AcquireResult
            {
                ChannelJson = channel.GetProperty("releases.json").GetString(),
                Version = channel.GetProperty("latest-sdk").GetString()
            };
        }
    }
}