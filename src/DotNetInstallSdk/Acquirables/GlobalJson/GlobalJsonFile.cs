using System.Text.Json.Serialization;

namespace DotNet.InstallSdk.Acquirables.GlobalJson
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class GlobalJsonFile
    {
        [JsonPropertyName("sdk")]
        public Sdk Sdk { get; set; }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class Sdk
    {
        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;
    }
}