namespace ArchGen.Domain;

public class Paths_SystemArchGen
{
    public required DomainPathsArquivos_SystemArchGen Domain {get; set;}
    public required ApplicationPathsArquivos_SystemArchGen Application {get; set;}
    public required InfraStructurePathsArquivos_SystemArchGen InfraStructure {get; set;}
    public required TestsPathsArquivos_SystemArchGen Tests {get; set;}
    public required Console_OR_APIPathsArquivos_SystemArchGen Console_OR_API {get; set;}
}
