namespace ArchGen.Domain.Interfaces;

public interface IFileService
{
    void CreateDirectory(string path);
    bool DirectoryExists(string path);
    bool FileExists(string path);
    Task WriteAllTextAsync(string path, string content, CancellationToken ct = default);
    Task<string> ReadAllTextAsync(string path, CancellationToken ct = default);
    string GetCurrentDirectory();
    string Combine(params string[] paths);
    string? GetDirectoryName(string path);
    void DeleteDirectory(string path, bool recursive);
    IEnumerable<string> GetFiles(string path, string searchPattern = "*.*", SearchOption searchOption = SearchOption.TopDirectoryOnly);
}
