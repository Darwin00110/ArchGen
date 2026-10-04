namespace ArchGen.Domain;

public class Paths_CamadasEntity
{
    public DomainPathsArquivosEntity Domain {get; set;} = new DomainPathsArquivosEntity
    {
        PathEntities = string.Empty,
        PathEnums = string.Empty,
        PathExceptions = string.Empty,
        PathInterfaces = string.Empty
    };
    public ApplicationPathsArquivosEntity Application {get; set;} = new ApplicationPathsArquivosEntity
    {
        PathDTOs = string.Empty,
        PathInterfaces = string.Empty,
        PathUseCases = string.Empty
    };
    public InfraStructurePathsArquivosEntity InfraStructure {get; set;} = new InfraStructurePathsArquivosEntity
    {
        PathData = string.Empty,
        PathMigrations = string.Empty,
        PathRepository = string.Empty,
        PathServices = string.Empty,
    };
    public TestsPathsArquivosEntity Tests {get; set;} = new TestsPathsArquivosEntity
    {
        ApplicationPathTests = string.Empty,
        DomainPathTests = string.Empty
    };
    public Console_OR_APIPathsArquivosEntity Console_OR_API {get; set;} = new Console_OR_APIPathsArquivosEntity
    {
        ControllerArquivo = string.Empty
    };

    public required string PathDomain {get; set;}
    public required string PathApplication {get; set;}
    public required string PathInfraStructure {get; set;}
    public required string PathTests {get; set;}
    public required string PathConsole_OR_API {get; set;}
}
