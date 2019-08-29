using System.Text.Json.Serialization;

namespace DotNet.InstallSdk
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class GlobalJsonFile
    {
        [JsonPropertyName("sdk")]
        public Sdk Sdk { get; set; }
    }

    public class Sdk
    {
        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;
    }
}