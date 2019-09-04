using System.IO;
using System.Text.Json;
using DotNet.InstallSdk.Acquirables.GlobalJson;

namespace DotNet.InstallSdk
{
    public class GlobalJsonLocator
    {
        readonly ITextWriter _textWriter;

        public GlobalJsonLocator(ITextWriter textWriter)
        {
            _textWriter = textWriter;
        }
        
        public GlobalJsonParseResult Parse()
        {
            const string path = "global.json";

            if (!File.Exists(path))
                return new GlobalJsonParseResult
                {
                    ErrorMessage = "A global.json could not be found in the current directory...",
                    IsSuccess = false
                };
            
            var contents = File.ReadAllText(path);
            var result = JsonSerializer.Deserialize<GlobalJsonFile>(contents);

            return new GlobalJsonParseResult
            {
                GlobalJsonFile = result,
                IsSuccess = true
            };
        }
    }
}