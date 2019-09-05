using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using DotNet.InstallSdk.Acquirables;
using static SimpleExec.Command;

namespace DotNet.InstallSdk
{
    public class SdkAcquirer
    {
        readonly HttpClient _httpClient;
        readonly ITextWriter _textWriter;
        readonly IInstallerLauncher _installerLauncher;
        readonly IPlatformIdentifier _platformIdentifier;

        public SdkAcquirer(HttpClient httpClient, ITextWriter textWriter, IInstallerLauncher installerLauncher, IPlatformIdentifier platformIdentifier)
        {
            _httpClient = httpClient;
            _textWriter = textWriter;
            _installerLauncher = installerLauncher;
            _platformIdentifier = platformIdentifier;
        }

        public async Task Acquire(Acquirable acquirable)
        {
            var result = await acquirable.Fetch(_httpClient);
            if (!result.IsSuccess)
                return;
            
            if (await CheckSdkExists(result.Version))
            {
                _textWriter.WriteLine($"SDK version {result.Version} is already installed.");
                return;
            }

            using var channelResponse = await JsonDocument.ParseAsync(await _httpClient.GetStreamAsync(result.ChannelJson));

            var file = channelResponse
                .RootElement.GetProperty("releases").EnumerateArray()
                .SelectMany(x =>
                {
                    IEnumerable<JsonElement> GetSdks()
                    {
                        yield return x.GetProperty("sdk");
                        if (x.TryGetProperty("sdks", out var sdks))
                        {
                            foreach (var y in sdks.EnumerateArray())
                                yield return y;
                        }
                    }

                    return GetSdks();
                })
                .First(x => x.GetProperty("version").GetString() == result.Version)
                .GetProperty("files")
                .EnumerateArray()
                .First(x => x.GetProperty("rid").GetString() == _platformIdentifier.GetPlatform());

            var name = file.GetProperty("name").GetString();
            var installerUrl = file.GetProperty("url").GetString();
            var fileHash = file.GetProperty("hash").GetString();
            
            var filePath = Path.Combine(Path.GetTempPath(), name);
            _textWriter.WriteLine($"Starting download of .NET Core SDK Version {result.Version}");
            using (var installerStream = await _httpClient.GetStreamAsync(installerUrl))
            {
                using var fileStream = new FileStream(filePath, FileMode.Create);
                var progress = new Progress<long>();

                var lastReportedBytesMbs = 0;
                progress.ProgressChanged += (sender, totalBytes) =>
                {
                    var currentBytesMbs = (int) Math.Floor(totalBytes / Math.Pow(2, 20));
                    if (currentBytesMbs <= lastReportedBytesMbs) return;
                    lastReportedBytesMbs = currentBytesMbs;
                    _textWriter.SetCursorPosition(0, Console.CursorTop);
                    _textWriter.Write($"Downloading: {currentBytesMbs}MB");
                };
                
                await CopyToWithProgress(installerStream, fileStream, progress);
            }

            CheckHash(filePath, fileHash);
            _installerLauncher.Launch(filePath);
        }

        static async Task CopyToWithProgress(Stream source, Stream destination, IProgress<long> progress)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(81920);
            long totalBytesRead = 0;
            try
            {
                while (true)
                {
                    var bytesRead = await source.ReadAsync(new Memory<byte>(buffer));
                    if (bytesRead == 0) break;
                    await destination.WriteAsync(new ReadOnlyMemory<byte>(buffer, 0, bytesRead));
                    totalBytesRead += bytesRead;
                    progress.Report(totalBytesRead);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        void CheckHash(string filePath, string fileHash)
        {
            using var sha512 = new SHA512Managed();
            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var hash = sha512.ComputeHash(stream);
            var hashString = BitConverter.ToString(hash).Replace("-", "");
            if (hashString != fileHash)
                _textWriter.WriteLine("The downloaded file contents did not match expected hash.");
        }
        
        static async Task<bool> CheckSdkExists(string version)
        {
            try
            {
                var dotnetInfo =
                    (await ReadAsync("dotnet", "--info", noEcho: true)).Split(new[] {'\r', '\n'},
                        StringSplitOptions.RemoveEmptyEntries);

                var existingSdks =
                    dotnetInfo
                        .SkipWhile(x => !x.Contains(".NET Core SDKs installed:"))
                        .TakeWhile(x => !string.IsNullOrWhiteSpace(x))
                        .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                        .Where(x => x.Length > 0)
                        .Select(x => x.First());

                return existingSdks.Contains(version);
            }
            catch
            {
                return false; // If we fail to detect installed sdks, just assume we need to acquire it
            }
        }
    }
}