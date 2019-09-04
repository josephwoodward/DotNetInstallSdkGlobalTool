namespace DotNet.InstallSdk.Acquirables.GlobalJson
{
    public class GlobalJsonParseResult
    {
        private GlobalJsonParseResult()
        {
        }

        public static GlobalJsonParseResult Failure(string error)
        {
            return new GlobalJsonParseResult
            {
                IsSuccess = false,
                ErrorMessage = error
            };
        }

        public static GlobalJsonParseResult Success(GlobalJsonFile result)
        {
            return new GlobalJsonParseResult
            {
                IsSuccess = true,
                GlobalJsonFile = result
            };
        }

        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;
        
        public GlobalJsonFile GlobalJsonFile { get; set; }
    }
}