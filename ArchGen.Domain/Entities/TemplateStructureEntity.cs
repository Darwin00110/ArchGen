namespace ArchGen.Domain;
 
public class TemplateStructure 
{
    public Guid ID {get; set;} = Guid.NewGuid();
    public required string NomeProjeto {get; set;}
    public required string TipoDoProjeto {get; set;}
    public required string PathSolucao {get; set;}
    public Paths_CamadasEntity paths {get; set;} = new Paths_CamadasEntity
    {
        PathApplication = string.Empty,
        PathDomain = string.Empty,
        PathConsole_OR_API = string.Empty,
        PathInfraStructure = string.Empty,
        PathTests = string.Empty,
        Domain = new DomainPathsArquivosEntity
        {
            PathEntities = string.Empty,
            PathEnums = string.Empty,
            PathExceptions = string.Empty,
            PathInterfaces = string.Empty
        },
        Application = new ApplicationPathsArquivosEntity
        {
            PathDTOs = string.Empty,
            PathInterfaces = string.Empty,
            PathUseCases = string.Empty
        },
        InfraStructure = new InfraStructurePathsArquivosEntity
        {
            PathData = string.Empty,
            PathMigrations = string.Empty,
            PathRepository = string.Empty,
            PathServices = string.Empty
        },
        Tests = new TestsPathsArquivosEntity
        {
            ApplicationPathTests = string.Empty,
            DomainPathTests = string.Empty
        },
        Console_OR_API = new Console_OR_APIPathsArquivosEntity
        {
            ControllerArquivo = string.Empty
        }
    };
    public required Paths_SystemArchGen paths_sys {get; set;}    
    public void VerifyProject()
    {
        VerificarPathSolucao();
        Verificar_NomeProjeto();
        Verificar_TipoProjeto();
        Verificar_DomainFiles();
        Verificar_ApplicationFiles();
        Verificar_InfraStructureFiles();
        Verificar_TestsFiles();
        Verificar_Console_OR_APIFiles();
    }
    private void Verificar_DomainFiles()
    {
        if(string.IsNullOrEmpty(paths.Domain.PathEntities) 
        || string.IsNullOrEmpty(paths.Domain.PathEnums) 
        || string.IsNullOrEmpty(paths.Domain.PathExceptions) 
        || string.IsNullOrEmpty(paths.Domain.PathInterfaces))
        {
            throw new DomainException("Impossivel criar o projeto, arquivos do Domain não foram definidos corretamente.");
        }
    }
    private void Verificar_ApplicationFiles()
    {
        if(string.IsNullOrEmpty(paths.Application.PathDTOs) 
        || string.IsNullOrEmpty(paths.Application.PathInterfaces) 
        || string.IsNullOrEmpty(paths.Application.PathUseCases))
        {
            throw new DomainException("Impossivel criar o projeto, arquivos do Application não foram definidos corretamente.");
        }
    }
    private void Verificar_InfraStructureFiles()
    {
        if(string.IsNullOrEmpty(paths.InfraStructure.PathData) 
        || string.IsNullOrEmpty(paths.InfraStructure.PathMigrations) 
        || string.IsNullOrEmpty(paths.InfraStructure.PathRepository) 
        || string.IsNullOrEmpty(paths.InfraStructure.PathServices))
        {
            throw new DomainException("Impossivel criar o projeto, arquivos do InfraStructure não foram definidos corretamente.");
        }
    }
    private void Verificar_TestsFiles()
    {
        if(string.IsNullOrEmpty(paths.Tests.ApplicationPathTests) 
        || string.IsNullOrEmpty(paths.Tests.DomainPathTests))
        {
            throw new DomainException("Impossivel criar o projeto, arquivos do Tests não foram definidos corretamente.");
        }
    }
    private void Verificar_Console_OR_APIFiles()
    {
        if(string.IsNullOrEmpty(paths.Console_OR_API.ControllerArquivo))
        {
            throw new DomainException("Impossivel criar o projeto, arquivos do Console ou API não foram definidos corretamente.");
        }
    }
    private void Verificar_TipoProjeto()
    {
        if(!TipoDoProjeto.Equals(TipoProjetosEnum.CONSOLE.ToString()) && !TipoDoProjeto.Equals(TipoProjetosEnum.API.ToString()))
        {
            throw new DomainException("Tipo de projeto invalido, ex: ('API' ou 'CONSOLE').");           
        }
    }
    private void VerificarPathSolucao()
    {
        if (string.IsNullOrEmpty(PathSolucao))
        {
            throw new DomainException("impossivel criar o projeto, Path do projeto vazio.");           
        }
    }
    private void Verificar_NomeProjeto()
    {
        if (string.IsNullOrEmpty(NomeProjeto))
        {
            throw new DomainException("Impossivel criar o projeto com o nome vazio, favor preencha o nome do projeto corretamente.");
        }
    }
}
