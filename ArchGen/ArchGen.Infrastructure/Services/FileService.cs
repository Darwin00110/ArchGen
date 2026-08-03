using System.Security.Cryptography;
using ArchGen.Application;

namespace ArchGen.Infrastructure;

public class FileService : IFileService
{
    public async Task<bool> WriteContent(string Content, string PathFile)
    {
        var directory = Path.GetDirectoryName(PathFile);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var hashBefore = File.Exists(PathFile) ? await ObterHashAsync(PathFile) : string.Empty;
        await File.WriteAllTextAsync(PathFile, Content);
        var hashAfter = await ObterHashAsync(PathFile);

        return hashBefore != hashAfter;
    }

    public async Task<string> ObterHashAsync(string path)
    {
        using var sha256 = SHA256.Create();
        await using var stream = File.OpenRead(path);
        var hash = await sha256.ComputeHashAsync(stream);
        return Convert.ToHexString(hash);
    }
}
