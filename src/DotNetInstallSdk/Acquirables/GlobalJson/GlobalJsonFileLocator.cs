using System.IO;
using System.Linq;
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
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());

            while (dir != null)
            {
                var searchFiles = dir.GetFiles(file);
                if (!searchFiles.Any())
                {
                    dir = dir.Parent;
                    continue;
                }
                
                var contents = File.ReadAllText(Path.Combine(dir.ToString(), file));
                var result = JsonSerializer.Deserialize<GlobalJsonFile>(contents);
                
                return GlobalJsonParseResult.Success(result);
            }
            
            return GlobalJsonParseResult.Failure($"A {file} file could not be found.");
        }
    }
}