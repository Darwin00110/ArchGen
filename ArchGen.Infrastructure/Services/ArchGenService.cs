using ArchGen.Application;

namespace ArchGen.Infrastructure;

public class ArchGenService : IArchGenService
{
    private string PathInternSolution = Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure");
    
    public async Task<bool> VerifyInternStructure_Diretory(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            path = PathInternSolution;
        }
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return true;
    }
}
