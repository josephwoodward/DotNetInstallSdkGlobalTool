using System.IO;
using System.Text.Json;

namespace DotNet.InstallSdk
{
    public class GlobalJsonLocator
    {
        readonly ITextWriter _textWriter;

        public GlobalJsonLocator(ITextWriter textWriter)
        {
            _textWriter = textWriter;
        }
        
        public GlobalJsonFile Parse()
        {
            const string path = "global.json";

            if (!File.Exists(path))
                throw new FileNotFoundException("A global.json could not be found in the current directory...");
            
            var contents = File.ReadAllText(path);
            return JsonSerializer.Deserialize<GlobalJsonFile>(contents);
        }
    }
}