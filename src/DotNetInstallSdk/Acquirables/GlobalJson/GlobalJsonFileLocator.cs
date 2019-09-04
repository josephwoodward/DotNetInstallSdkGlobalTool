using System.IO;
using System.Text.Json;

namespace DotNet.InstallSdk.Acquirables.GlobalJson
{
    public class GlobalJsonFileLocator
    {
        public GlobalJsonFileLocator(ITextWriter _)
        {
        }
        
        public GlobalJsonParseResult Parse(string file = "global.json")
        {
            if (!File.Exists(file))
                return GlobalJsonParseResult.Failure($"A {file} file could not be found in the current directory.");
            
            var contents = File.ReadAllText(file);
            var result = JsonSerializer.Deserialize<GlobalJsonFile>(contents);
            
            return GlobalJsonParseResult.Success(result);
        }
    }
}