using System.Text.Json.Serialization;

namespace DotNet.InstallSdk.Acquirables.GlobalJson
{
    public class GlobalJsonParseResult
    {
        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;
        public GlobalJsonFile GlobalJsonFile { get; set; }
    }
    
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