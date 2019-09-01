using System.Net.Http;
using System.Threading.Tasks;

namespace DotNet.InstallSdk.Acquirables
{
    public abstract class Acquirable
    {
        protected readonly string ReleaseIndex = "https://raw.githubusercontent.com/dotnet/core/master/release-notes/releases-index.json";
        
        public abstract Task<AcquireResult> Fetch(HttpClient httpClient);
    }
}