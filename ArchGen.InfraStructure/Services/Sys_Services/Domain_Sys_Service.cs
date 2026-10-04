using ArchGen.Application;
using ArchGen.Domain;

namespace ArchGen.InfraStructure;

public class Domain_Sys_Service : IDomain_Sys_Service
{
    public async Task<string> GetContentFile_Entities(string SystemOS, string PathEntities)
    {
        string ContentFile;
        PathEntities = (PathEntities != String.Empty) ? PathEntities : (SystemOS == "WINDOWS") ?
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Domain", "Entities", "Entities.cs") : 
        (SystemOS == "MACOS" || SystemOS == "LINUX") ? 
        Path.Combine("~/.local/share", "ArchGen", "Domain", "Entities", "Entities.cs") : 
        throw new ServiceException("Sistema operacional não reconhecido, favor usar um dos compativeis (WINDOWS, MACOS, LINUX)");
        if (!File.Exists(PathEntities))
        {
            ContentFile = """

            """;
            await File.WriteAllTextAsync(PathEntities, ContentFile);
            
            return ContentFile;            
        } else
        {
            ContentFile = await File.ReadAllTextAsync(PathEntities);
            return ContentFile;
        }
    }
}
