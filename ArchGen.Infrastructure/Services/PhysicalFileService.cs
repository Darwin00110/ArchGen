using ArchGen.Domain.Interfaces;

namespace ArchGen.InfraStructure.Services;

public class PhysicalFileService : IFileService
{
    public void CreateDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public bool DirectoryExists(string path)
    {
        return Directory.Exists(path);
    }

    public bool FileExists(string path)
    {
        return File.Exists(path);
    }

    public async Task WriteAllTextAsync(string path, string content, CancellationToken ct = default)
    {
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        await File.WriteAllTextAsync(path, content, ct);
    }

    public async Task<string> ReadAllTextAsync(string path, CancellationToken ct = default)
    {
        return await File.ReadAllTextAsync(path, ct);
    }

    public string GetCurrentDirectory()
    {
        return Directory.GetCurrentDirectory();
    }

    public string Combine(params string[] paths)
    {
        return Path.Combine(paths);
    }

    public string? GetDirectoryName(string path)
    {
        return Path.GetDirectoryName(path);
    }

    public void DeleteDirectory(string path, bool recursive)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, recursive);
        }
    }

    public IEnumerable<string> GetFiles(string path, string searchPattern = "*.*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        if (!Directory.Exists(path)) return Enumerable.Empty<string>();
        return Directory.GetFiles(path, searchPattern, searchOption);
    }
}
