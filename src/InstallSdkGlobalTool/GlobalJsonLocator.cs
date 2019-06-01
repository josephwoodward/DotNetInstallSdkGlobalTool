using System.IO;
using Newtonsoft.Json;

namespace InstallSdkGlobalTool
{
    public class GlobalJsonLocator
    {
        private readonly ITextWriter _textWriter;

        public GlobalJsonLocator(ITextWriter textWriter)
        {
            _textWriter = textWriter;
        }
        
        public GlobalJsonFile Parse()
        {
            var path = "global.json";
            if (!File.Exists(path))
                _textWriter.WriteLine("global.json could not be found in the current directory");
            
            var contents = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<GlobalJsonFile>(contents);
        }
    }
}