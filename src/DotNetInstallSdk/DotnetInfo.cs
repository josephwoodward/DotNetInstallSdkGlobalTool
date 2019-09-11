using System.Threading.Tasks;
using SimpleExec;

namespace DotNet.InstallSdk
{
    public interface IDotnetInfo
    {
        Task<string> GetInfo();
    }
    
    public class DotnetInfo : IDotnetInfo
    {
        public async Task<string> GetInfo()
        {
            return await Command.ReadAsync("dotnet", "--info", noEcho: true);
        }
    }
}