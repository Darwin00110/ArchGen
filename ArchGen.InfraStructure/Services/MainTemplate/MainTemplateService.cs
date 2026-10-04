using ArchGen.Application;
using ArchGen.Domain;

namespace ArchGen.InfraStructure;

public class MainTemplateService : IMainTemplateService
{
    private readonly IArchGenRepository _repo;
    private readonly IDotNetService _dotnet;
    private readonly ISystemOSService _systemOS;
    private readonly IDomain_Sys_Service _sys_domain;
    private readonly IApplication_Sys_Service _sys_application;
    private readonly IInfraStructure_Sys_Service _sys_infrastructure;
    private readonly ITests_Sys_Service _sys_tests;
    private readonly IConsole_OR_API_Sys_Service _sys_console_or_api;
    public required string NomeProjeto { get; set; }
    public required string PathProjeto { get; set; }
    public required string TipoProjeto { get; set; }
    public MainTemplateService(IArchGenRepository repo,
    IDotNetService dotnet,
    ISystemOSService systemOS,
    IDomain_Sys_Service sys_domain,
    IApplication_Sys_Service sys_application,
    IInfraStructure_Sys_Service sys_infrastructure,
    IConsole_OR_API_Sys_Service sys_console_OR_api,
    ITests_Sys_Service sys_tests)
    {
        _repo = repo;
        _dotnet = dotnet;
        _systemOS = systemOS;
        _sys_domain = sys_domain;
        _sys_application = sys_application;
        _sys_infrastructure = sys_infrastructure;
        _sys_tests = sys_tests;
        _sys_console_or_api = sys_console_OR_api;
    }
    public async Task<TemplateStructure> CreateEntityArchGen(string NomeProjeto, string PathProjeto, string TipoProjeto)
    {
        var SystemOS = _systemOS.GetSystemOS();
        var pathDomain = Path.Combine(PathProjeto, $"{NomeProjeto}.Domain");
        var pathApplication = Path.Combine(PathProjeto, $"{NomeProjeto}.Application");
        var pathInfraStructure = Path.Combine(PathProjeto, $"{NomeProjeto}.InfraStructure");
        var pathTests = Path.Combine(PathProjeto, $"{NomeProjeto}.Tests");
        var pathConsole_OR_API = Path.Combine(PathProjeto, $"{NomeProjeto}.{TipoProjeto}");

        var ProjetoUsuario = new TemplateStructure
        {
            NomeProjeto = NomeProjeto,
            PathSolucao = PathProjeto,
            TipoDoProjeto = TipoProjeto,
            paths = {
                Domain = {
                    PathEntities = Path.Combine(pathDomain, "Entities", "Entities.cs"),
                    PathEnums = Path.Combine(pathDomain, "Enums", "Enums.cs"),
                    PathExceptions = Path.Combine(pathDomain, "Exceptions", "Exceptions.cs"),
                    PathInterfaces = Path.Combine(pathDomain, "Interfaces", "Interfaces.cs")
                },
                Application = {
                    PathDTOs = Path.Combine(pathApplication, "Services", "Services.cs"),
                    PathInterfaces = Path.Combine(pathApplication, "Interfaces", "Interfaces.cs"),
                    PathUseCases = Path.Combine(pathApplication, "UseCases", "UseCases.cs")
                },
                InfraStructure =
                {
                    PathRepository = Path.Combine(pathInfraStructure, "Repository", "Repository.cs"),
                    PathServices = Path.Combine(pathInfraStructure, "Services", "SystemOSService.cs"),
                    PathData = Path.Combine(pathInfraStructure, "Data", "AppDbContext.cs"),
                    PathMigrations = Path.Combine(pathInfraStructure, "Migrations"),
                },
                Tests =
                {
                    ApplicationPathTests = Path.Combine(pathTests, "Application", "ApplicationTests.cs"),
                    DomainPathTests = Path.Combine(pathTests, "Domain", "DomainTests.cs"),
                },
                Console_OR_API =
                {
                    ControllerArquivo = Path.Combine(pathConsole_OR_API, "Controllers", "Controller.cs"),
                },
                PathApplication = pathApplication,
                PathDomain = pathDomain,
                PathInfraStructure = pathInfraStructure,
                PathTests = pathTests,
                PathConsole_OR_API = pathConsole_OR_API,
            },
            paths_sys = new Paths_SystemArchGen
            {
                Application = new ApplicationPathsArquivos_SystemArchGen
                {
                    Application_Dir = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Application"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Application"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Application"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    DTOS_Dir = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Application", "DTOS"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Application", "DTOS"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Application", "DTOS"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    Interfaces_Dir = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Application", "Interfaces"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Application", "Interfaces"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Application", "Interfaces"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    UseCases_Dir = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Application", "UseCases"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Application", "UseCases"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Application", "UseCases"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    DTOS_File = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Application", "DTOS", "CreateUserRequest.cs"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Application", "DTOS", "DTOS.cs"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Application", "DTOS", "DTOS.cs"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    Interface_File = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Application", "Interfaces", "Interfaces.cs"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Application", "Interfaces", "Interfaces.cs"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Application", "Interfaces", "Interfaces.cs"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    UseCase_File = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Application", "UseCases", "UseCases.cs"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Application", "UseCases", "UseCases.cs"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Application", "UseCases", "UseCases.cs"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },

                },
                Domain = new DomainPathsArquivos_SystemArchGen
                {
                    Domain_Dir = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Domain"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Domain"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Domain"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    Entities_Dir = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Domain", "Entities"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Domain", "Entities"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Domain", "Entities"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    Entities_File = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Domain", "Entities", "Entities.cs"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Domain", "Entities", "Entities.cs"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Domain", "Entities", "Entities.cs"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    Enums_Dir = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Domain", "Enums"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Domain", "Enums"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Domain", "Enums"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    Enums_File = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Domain", "Enums", "Enums.cs"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Domain", "Enums", "Enums.cs"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Domain", "Enums", "Enums.cs"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    Exceptions_Dir = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Domain", "Exceptions"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Domain", "Exceptions"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Domain", "Exceptions"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    Exceptions_File = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Domain", "Exceptions", "Exceptions.cs"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Domain", "Exceptions", "Exceptions.cs"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Domain", "Exceptions", "Exceptions.cs"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    Interfaces_Dir = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Domain", "Interfaces"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Domain", "Interfaces"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Domain", "Interfaces"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    Interfaces_File = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Domain", "Interfaces", "Interfaces.cs"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Domain", "Interfaces", "Interfaces.cs"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Domain", "Interfaces", "Interfaces.cs"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                },
                Console_OR_API = new Console_OR_APIPathsArquivos_SystemArchGen
                {
                    Console_OR_API_Dir = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Console_OR_API"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Console_OR_API"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Console_OR_API"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    Program_File = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Console_OR_API", "Program.cs"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Console_OR_API", "Program.cs"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Console_OR_API", "Program.cs"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    Controllers_OR_Args_Dir = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Console_OR_API", "Controllers_OR_Args"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Console_OR_API", "Controllers_OR_Args"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Console_OR_API", "Controllers_OR_Args"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    Controllers_OR_Args_File = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Console_OR_API", "Controllers_OR_Args", "Controllers_OR_Args.cs"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Console_OR_API", "Controllers_OR_Args", "Controllers_OR_Args.cs"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Console_OR_API", "Controllers_OR_Args", "Controllers_OR_Args.cs"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                },
                InfraStructure = new InfraStructurePathsArquivos_SystemArchGen
                {
                    Contexts_File = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "InfraStructure", "Contexts", "AppDbContext.cs"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "InfraStructure", "Contexts", "AppDbContext.cs"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "InfraStructure", "Contexts", "AppDbContext.cs"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    Data_Dir = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "InfraStructure", "Data"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "InfraStructure", "Data"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "InfraStructure", "Data"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    InfraStructure_Dir = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "InfraStructure"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "InfraStructure"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "InfraStructure"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    Repositories_Dir = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "InfraStructure", "Repositories"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "InfraStructure", "Repositories"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "InfraStructure", "Repositories"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    Repositories_File = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "InfraStructure", "Repositories", "Repository.cs"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "InfraStructure", "Repositories", "Repository.cs"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "InfraStructure", "Repositories", "Repository.cs"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    Services_Dir = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "InfraStructure", "Services"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "InfraStructure", "Services"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "InfraStructure", "Services"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    Services_File = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "InfraStructure", "Services", "SystemOSService.cs"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "InfraStructure", "Services", "SystemOSService.cs"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "InfraStructure", "Services", "SystemOSService.cs"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                },
                Tests = new TestsPathsArquivos_SystemArchGen
                {
                    ApplicationTests_Dir = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Tests", "Application"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Tests", "Application"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Tests", "Application"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    ApplicationTests_File = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Tests", "Application", "ApplicationTests.cs"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Tests", "Application", "ApplicationTests.cs"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Tests", "Application", "ApplicationTests.cs"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    DomainTests_Dir = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Tests", "Domain"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Tests", "Domain"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Tests", "Domain"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    DomainTests_File = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Tests", "Domain", "DomainTests.cs"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Tests", "Domain", "DomainTests.cs"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Tests", "Domain", "DomainTests.cs"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                    Tests_Dir = SystemOS switch
                    {
                        "WINDOWS" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ArchGen", "Tests"),
                        "LINUX" => Path.Combine("~/.local/share", "ArchGen", "Tests"),
                        "MACOS" => Path.Combine("~/.local/share", "ArchGen", "Tests"),
                        _ => throw new ApplicationException("Sistema Operacional não suportado, apenas Windows, Linux e MacOS são suportados."),
                    },
                },
            },
        };
        return ProjetoUsuario;
    }
    public async void ReconstruirEstrutura_MainTemplate_Sys(TemplateStructure Data)
    {
        var SystemOS = _systemOS.GetSystemOS();
        List<string> ReconstruirSystem = new List<string>();
        string AddValueReconstruirSystem(string path)
        {
            ReconstruirSystem.Add(path);
            return path;
        }
        //SystemOS
        //Application
        Data.paths_sys.Application.Application_Dir = (Path.Exists(Data.paths_sys.Application.Application_Dir)) ? Data.paths_sys.Application.Application_Dir : AddValueReconstruirSystem(Data.paths_sys.Application.Application_Dir);
        Data.paths_sys.Application.DTOS_Dir = (Path.Exists(Data.paths_sys.Application.DTOS_Dir)) ? Data.paths_sys.Application.DTOS_Dir : AddValueReconstruirSystem(Data.paths_sys.Application.DTOS_Dir);
        Data.paths_sys.Application.Interfaces_Dir = (Path.Exists(Data.paths_sys.Application.Interfaces_Dir)) ? Data.paths_sys.Application.Interfaces_Dir : AddValueReconstruirSystem(Data.paths_sys.Application.Interfaces_Dir);
        Data.paths_sys.Application.UseCases_Dir = (Path.Exists(Data.paths_sys.Application.UseCases_Dir)) ? Data.paths_sys.Application.UseCases_Dir : AddValueReconstruirSystem(Data.paths_sys.Application.UseCases_Dir);
        Data.paths_sys.Application.DTOS_File = (Path.Exists(Data.paths_sys.Application.DTOS_File)) ? Data.paths_sys.Application.DTOS_File : AddValueReconstruirSystem(Data.paths_sys.Application.DTOS_File);
        Data.paths_sys.Application.Interface_File = (Path.Exists(Data.paths_sys.Application.Interface_File)) ? Data.paths_sys.Application.Interface_File : AddValueReconstruirSystem(Data.paths_sys.Application.Interface_File);
        Data.paths_sys.Application.UseCase_File = (Path.Exists(Data.paths_sys.Application.UseCase_File)) ? Data.paths_sys.Application.UseCase_File : AddValueReconstruirSystem(Data.paths_sys.Application.UseCase_File);

        //Domain
        Data.paths_sys.Domain.Domain_Dir = (Path.Exists(Data.paths_sys.Domain.Domain_Dir)) ? Data.paths_sys.Domain.Domain_Dir : AddValueReconstruirSystem(Data.paths_sys.Domain.Domain_Dir);
        Data.paths_sys.Domain.Entities_Dir = (Path.Exists(Data.paths_sys.Domain.Entities_Dir)) ? Data.paths_sys.Domain.Entities_Dir : AddValueReconstruirSystem(Data.paths_sys.Domain.Entities_Dir);
        Data.paths_sys.Domain.Entities_File = (Path.Exists(Data.paths_sys.Domain.Entities_File)) ? Data.paths_sys.Domain.Entities_File : AddValueReconstruirSystem(Data.paths_sys.Domain.Entities_File);
        Data.paths_sys.Domain.Enums_Dir = (Path.Exists(Data.paths_sys.Domain.Enums_Dir)) ? Data.paths_sys.Domain.Enums_Dir : AddValueReconstruirSystem(Data.paths_sys.Domain.Enums_Dir);
        Data.paths_sys.Domain.Enums_File = (Path.Exists(Data.paths_sys.Domain.Enums_File)) ? Data.paths_sys.Domain.Enums_File : AddValueReconstruirSystem(Data.paths_sys.Domain.Enums_File);
        Data.paths_sys.Domain.Exceptions_Dir = (Path.Exists(Data.paths_sys.Domain.Exceptions_Dir)) ? Data.paths_sys.Domain.Exceptions_Dir : AddValueReconstruirSystem(Data.paths_sys.Domain.Exceptions_Dir);
        Data.paths_sys.Domain.Exceptions_File = (Path.Exists(Data.paths_sys.Domain.Exceptions_File)) ? Data.paths_sys.Domain.Exceptions_File : AddValueReconstruirSystem(Data.paths_sys.Domain.Exceptions_File);
        Data.paths_sys.Domain.Interfaces_Dir = (Path.Exists(Data.paths_sys.Domain.Interfaces_Dir)) ? Data.paths_sys.Domain.Interfaces_Dir : AddValueReconstruirSystem(Data.paths_sys.Domain.Interfaces_Dir);
        Data.paths_sys.Domain.Interfaces_File = (Path.Exists(Data.paths_sys.Domain.Interfaces_File)) ? Data.paths_sys.Domain.Interfaces_File : AddValueReconstruirSystem(Data.paths_sys.Domain.Interfaces_File);

        //Console_OR_API
        Data.paths_sys.Console_OR_API.Console_OR_API_Dir = (Path.Exists(Data.paths_sys.Console_OR_API.Console_OR_API_Dir)) ? Data.paths_sys.Console_OR_API.Console_OR_API_Dir : AddValueReconstruirSystem(Data.paths_sys.Console_OR_API.Console_OR_API_Dir);
        Data.paths_sys.Console_OR_API.Program_File = (Path.Exists(Data.paths_sys.Console_OR_API.Program_File)) ? Data.paths_sys.Console_OR_API.Program_File : AddValueReconstruirSystem(Data.paths_sys.Console_OR_API.Program_File);
        Data.paths_sys.Console_OR_API.Controllers_OR_Args_Dir = (Path.Exists(Data.paths_sys.Console_OR_API.Controllers_OR_Args_Dir)) ? Data.paths_sys.Console_OR_API.Controllers_OR_Args_Dir : AddValueReconstruirSystem(Data.paths_sys.Console_OR_API.Controllers_OR_Args_Dir);
        Data.paths_sys.Console_OR_API.Controllers_OR_Args_File = (Path.Exists(Data.paths_sys.Console_OR_API.Controllers_OR_Args_File)) ? Data.paths_sys.Console_OR_API.Controllers_OR_Args_File : AddValueReconstruirSystem(Data.paths_sys.Console_OR_API.Controllers_OR_Args_File);

        //InfraStructure
        Data.paths_sys.InfraStructure.Contexts_File = (Path.Exists(Data.paths_sys.InfraStructure.Contexts_File)) ? Data.paths_sys.InfraStructure.Contexts_File : AddValueReconstruirSystem(Data.paths_sys.InfraStructure.Contexts_File);
        Data.paths_sys.InfraStructure.Data_Dir = (Path.Exists(Data.paths_sys.InfraStructure.Data_Dir)) ? Data.paths_sys.InfraStructure.Data_Dir : AddValueReconstruirSystem(Data.paths_sys.InfraStructure.Data_Dir);
        Data.paths_sys.InfraStructure.InfraStructure_Dir = (Path.Exists(Data.paths_sys.InfraStructure.InfraStructure_Dir)) ? Data.paths_sys.InfraStructure.InfraStructure_Dir : AddValueReconstruirSystem(Data.paths_sys.InfraStructure.InfraStructure_Dir);
        Data.paths_sys.InfraStructure.Repositories_Dir = (Path.Exists(Data.paths_sys.InfraStructure.Repositories_Dir)) ? Data.paths_sys.InfraStructure.Repositories_Dir : AddValueReconstruirSystem(Data.paths_sys.InfraStructure.Repositories_Dir);
        Data.paths_sys.InfraStructure.Repositories_File = (Path.Exists(Data.paths_sys.InfraStructure.Repositories_File)) ? Data.paths_sys.InfraStructure.Repositories_File : AddValueReconstruirSystem(Data.paths_sys.InfraStructure.Repositories_File);
        Data.paths_sys.InfraStructure.Services_Dir = (Path.Exists(Data.paths_sys.InfraStructure.Services_Dir)) ? Data.paths_sys.InfraStructure.Services_Dir : AddValueReconstruirSystem(Data.paths_sys.InfraStructure.Services_Dir);
        Data.paths_sys.InfraStructure.Services_File = (Path.Exists(Data.paths_sys.InfraStructure.Services_File)) ? Data.paths_sys.InfraStructure.Services_File : AddValueReconstruirSystem(Data.paths_sys.InfraStructure.Services_File);

        //Tests
        Data.paths_sys.Tests.ApplicationTests_Dir = (Path.Exists(Data.paths_sys.Tests.ApplicationTests_Dir)) ? Data.paths_sys.Tests.ApplicationTests_Dir : AddValueReconstruirSystem(Data.paths_sys.Tests.ApplicationTests_Dir);
        Data.paths_sys.Tests.ApplicationTests_File = (Path.Exists(Data.paths_sys.Tests.ApplicationTests_File)) ? Data.paths_sys.Tests.ApplicationTests_File : AddValueReconstruirSystem(Data.paths_sys.Tests.ApplicationTests_File);
        Data.paths_sys.Tests.DomainTests_Dir = (Path.Exists(Data.paths_sys.Tests.DomainTests_Dir)) ? Data.paths_sys.Tests.DomainTests_Dir : AddValueReconstruirSystem(Data.paths_sys.Tests.DomainTests_Dir);
        Data.paths_sys.Tests.DomainTests_File = (Path.Exists(Data.paths_sys.Tests.DomainTests_File)) ? Data.paths_sys.Tests.DomainTests_File : AddValueReconstruirSystem(Data.paths_sys.Tests.DomainTests_File);
        foreach (var PathsSys in ReconstruirSystem)
        {
            if (Path.Exists(PathsSys))
            {
                _ = (PathsSys.Contains(".cs") && PathsSys.Contains("Domain") && PathsSys.Contains("Entities")) ? await File.AppendAllTextAsync(await _sys_domain.GetContentFile_Entities(), "") : ;
            }
        }
    }
    public void VerifyEntityArchGen(TemplateStructure Data)
    {
        var SystemOS = _systemOS.GetSystemOS();
        Data.PathSolucao = (Path.Exists(Data.PathSolucao)) ? Data.PathSolucao : throw new ServiceException("Caminho da solução não encontrado.");
        Data.paths.PathDomain = (Path.Exists(Data.paths.PathDomain)) ? Data.paths.PathDomain : throw new ServiceException("Caminho do domínio não encontrado.");
        Data.paths.PathApplication = (Path.Exists(Data.paths.PathApplication)) ? Data.paths.PathApplication : throw new ServiceException("Caminho da aplicação não encontrado.");
        Data.paths.PathInfraStructure = (Path.Exists(Data.paths.PathInfraStructure)) ? Data.paths.PathInfraStructure : throw new ServiceException("Caminho da infraestrutura não encontrado.");
        Data.paths.PathTests = (Path.Exists(Data.paths.PathTests)) ? Data.paths.PathTests : throw new ServiceException("Caminho dos testes não encontrado.");
        Data.paths.PathConsole_OR_API = (Path.Exists(Data.paths.PathConsole_OR_API)) ? Data.paths.PathConsole_OR_API : throw new ServiceException("Caminho do console ou API não encontrado.");
        //Domain
        Data.paths.Domain.PathEntities = (Path.Exists(Data.paths.Domain.PathEntities)) ? Data.paths.Domain.PathEntities : throw new ServiceException("Caminho das entidades não encontrado.");
        Data.paths.Domain.PathEnums = (Path.Exists(Data.paths.Domain.PathEnums)) ? Data.paths.Domain.PathEnums : throw new ServiceException("Caminho dos enums não encontrado.");
        Data.paths.Domain.PathExceptions = (Path.Exists(Data.paths.Domain.PathExceptions)) ? Data.paths.Domain.PathExceptions : throw new ServiceException("Caminho das exceptions não encontrado.");
        Data.paths.Domain.PathInterfaces = (Path.Exists(Data.paths.Domain.PathInterfaces)) ? Data.paths.Domain.PathInterfaces : throw new ServiceException("Caminho das interfaces não encontrado.");
        //Application
        Data.paths.Application.PathDTOs = (Path.Exists(Data.paths.Application.PathDTOs)) ? Data.paths.Application.PathDTOs : throw new ServiceException("Caminho dos DTOs não encontrado.");
        Data.paths.Application.PathInterfaces = (Path.Exists(Data.paths.Application.PathInterfaces)) ? Data.paths.Application.PathInterfaces : throw new ServiceException("Caminho das interfaces não encontrado.");
        Data.paths.Application.PathUseCases = (Path.Exists(Data.paths.Application.PathUseCases)) ? Data.paths.Application.PathUseCases : throw new ServiceException("Caminho dos use cases não encontrado.");
        //InfraStructure
        Data.paths.InfraStructure.PathRepository = (Path.Exists(Data.paths.InfraStructure.PathRepository)) ? Data.paths.InfraStructure.PathRepository : throw new ServiceException("Caminho do repositório não encontrado.");
        Data.paths.InfraStructure.PathServices = (Path.Exists(Data.paths.InfraStructure.PathServices)) ? Data.paths.InfraStructure.PathServices : throw new ServiceException("Caminho dos serviços não encontrado.");
        Data.paths.InfraStructure.PathData = (Path.Exists(Data.paths.InfraStructure.PathData)) ? Data.paths.InfraStructure.PathData : throw new ServiceException("Caminho do contexto não encontrado.");
        Data.paths.InfraStructure.PathMigrations = (Path.Exists(Data.paths.InfraStructure.PathMigrations)) ? Data.paths.InfraStructure.PathMigrations : throw new ServiceException("Caminho das migrations não encontrado.");
        //Tests
        Data.paths.Tests.ApplicationPathTests = (Path.Exists(Data.paths.Tests.ApplicationPathTests)) ? Data.paths.Tests.ApplicationPathTests : throw new ServiceException("Caminho dos testes da aplicação não encontrado.");
        Data.paths.Tests.DomainPathTests = (Path.Exists(Data.paths.Tests.DomainPathTests)) ? Data.paths.Tests.DomainPathTests : throw new ServiceException("Caminho dos testes do domínio não encontrado.");
        //Console_OR_API
        Data.paths.Console_OR_API.ControllerArquivo = (Path.Exists(Data.paths.Console_OR_API.ControllerArquivo)) ? Data.paths.Console_OR_API.ControllerArquivo : throw new ServiceException("Caminho do controller não encontrado.");



        //SystemOS
        //Application
        Data.paths_sys.Application.Application_Dir = (Path.Exists(Data.paths_sys.Application.Application_Dir)) ? Data.paths_sys.Application.Application_Dir : throw new ServiceException("Caminho do diretório da aplicação não encontrado.");
        Data.paths_sys.Application.DTOS_Dir = (Path.Exists(Data.paths_sys.Application.DTOS_Dir)) ? Data.paths_sys.Application.DTOS_Dir : throw new ServiceException("Caminho do diretório dos DTOs não encontrado.");
        Data.paths_sys.Application.Interfaces_Dir = (Path.Exists(Data.paths_sys.Application.Interfaces_Dir)) ? Data.paths_sys.Application.Interfaces_Dir : throw new ServiceException("Caminho do diretório das interfaces não encontrado.");
        Data.paths_sys.Application.UseCases_Dir = (Path.Exists(Data.paths_sys.Application.UseCases_Dir)) ? Data.paths_sys.Application.UseCases_Dir : throw new ServiceException("Caminho do diretório dos use cases não encontrado.");
        Data.paths_sys.Application.DTOS_File = (Path.Exists(Data.paths_sys.Application.DTOS_File)) ? Data.paths_sys.Application.DTOS_File : throw new ServiceException("Caminho do arquivo dos DTOs não encontrado.");
        Data.paths_sys.Application.Interface_File = (Path.Exists(Data.paths_sys.Application.Interface_File)) ? Data.paths_sys.Application.Interface_File : throw new ServiceException("Caminho do arquivo das interfaces não encontrado.");
        Data.paths_sys.Application.UseCase_File = (Path.Exists(Data.paths_sys.Application.UseCase_File)) ? Data.paths_sys.Application.UseCase_File : throw new ServiceException("Caminho do arquivo dos use cases não encontrado.");

        //Domain
        Data.paths_sys.Domain.Domain_Dir = (Path.Exists(Data.paths_sys.Domain.Domain_Dir)) ? Data.paths_sys.Domain.Domain_Dir : throw new ServiceException("Caminho do diretório do domínio não encontrado.");
        Data.paths_sys.Domain.Entities_Dir = (Path.Exists(Data.paths_sys.Domain.Entities_Dir)) ? Data.paths_sys.Domain.Entities_Dir : throw new ServiceException("Caminho do diretório das entidades não encontrado.");
        Data.paths_sys.Domain.Entities_File = (Path.Exists(Data.paths_sys.Domain.Entities_File)) ? Data.paths_sys.Domain.Entities_File : throw new ServiceException("Caminho do arquivo das entidades não encontrado.");
        Data.paths_sys.Domain.Enums_Dir = (Path.Exists(Data.paths_sys.Domain.Enums_Dir)) ? Data.paths_sys.Domain.Enums_Dir : throw new ServiceException("Caminho do diretório dos enums não encontrado.");
        Data.paths_sys.Domain.Enums_File = (Path.Exists(Data.paths_sys.Domain.Enums_File)) ? Data.paths_sys.Domain.Enums_File : throw new ServiceException("Caminho do arquivo dos enums não encontrado.");
        Data.paths_sys.Domain.Exceptions_Dir = (Path.Exists(Data.paths_sys.Domain.Exceptions_Dir)) ? Data.paths_sys.Domain.Exceptions_Dir : throw new ServiceException("Caminho do diretório das exceptions não encontrado.");
        Data.paths_sys.Domain.Exceptions_File = (Path.Exists(Data.paths_sys.Domain.Exceptions_File)) ? Data.paths_sys.Domain.Exceptions_File : throw new ServiceException("Caminho do arquivo das exceptions não encontrado.");
        Data.paths_sys.Domain.Interfaces_Dir = (Path.Exists(Data.paths_sys.Domain.Interfaces_Dir)) ? Data.paths_sys.Domain.Interfaces_Dir : throw new ServiceException("Caminho do diretório das interfaces não encontrado.");
        Data.paths_sys.Domain.Interfaces_File = (Path.Exists(Data.paths_sys.Domain.Interfaces_File)) ? Data.paths_sys.Domain.Interfaces_File : throw new ServiceException("Caminho do arquivo das interfaces não encontrado.");

        //Console_OR_API
        Data.paths_sys.Console_OR_API.Console_OR_API_Dir = (Path.Exists(Data.paths_sys.Console_OR_API.Console_OR_API_Dir)) ? Data.paths_sys.Console_OR_API.Console_OR_API_Dir : throw new ServiceException("Caminho do diretório do console ou API não encontrado.");
        Data.paths_sys.Console_OR_API.Program_File = (Path.Exists(Data.paths_sys.Console_OR_API.Program_File)) ? Data.paths_sys.Console_OR_API.Program_File : throw new ServiceException("Caminho do arquivo do console ou API não encontrado.");
        Data.paths_sys.Console_OR_API.Controllers_OR_Args_Dir = (Path.Exists(Data.paths_sys.Console_OR_API.Controllers_OR_Args_Dir)) ? Data.paths_sys.Console_OR_API.Controllers_OR_Args_Dir : throw new ServiceException("Caminho do diretório dos controllers ou args não encontrado.");
        Data.paths_sys.Console_OR_API.Controllers_OR_Args_File = (Path.Exists(Data.paths_sys.Console_OR_API.Controllers_OR_Args_File)) ? Data.paths_sys.Console_OR_API.Controllers_OR_Args_File : throw new ServiceException("Caminho do arquivo dos controllers ou args não encontrado.");

        //InfraStructure
        Data.paths_sys.InfraStructure.Contexts_File = (Path.Exists(Data.paths_sys.InfraStructure.Contexts_File)) ? Data.paths_sys.InfraStructure.Contexts_File : throw new ServiceException("Caminho do arquivo do contexto não encontrado.");
        Data.paths_sys.InfraStructure.Data_Dir = (Path.Exists(Data.paths_sys.InfraStructure.Data_Dir)) ? Data.paths_sys.InfraStructure.Data_Dir : throw new ServiceException("Caminho do diretório dos dados não encontrado.");
        Data.paths_sys.InfraStructure.InfraStructure_Dir = (Path.Exists(Data.paths_sys.InfraStructure.InfraStructure_Dir)) ? Data.paths_sys.InfraStructure.InfraStructure_Dir : throw new ServiceException("Caminho do diretório da infraestrutura não encontrado.");
        Data.paths_sys.InfraStructure.Repositories_Dir = (Path.Exists(Data.paths_sys.InfraStructure.Repositories_Dir)) ? Data.paths_sys.InfraStructure.Repositories_Dir : throw new ServiceException("Caminho do diretório dos repositórios não encontrado.");
        Data.paths_sys.InfraStructure.Repositories_File = (Path.Exists(Data.paths_sys.InfraStructure.Repositories_File)) ? Data.paths_sys.InfraStructure.Repositories_File : throw new ServiceException("Caminho do arquivo dos repositórios não encontrado.");
        Data.paths_sys.InfraStructure.Services_Dir = (Path.Exists(Data.paths_sys.InfraStructure.Services_Dir)) ? Data.paths_sys.InfraStructure.Services_Dir : throw new ServiceException("Caminho do diretório dos serviços não encontrado.");
        Data.paths_sys.InfraStructure.Services_File = (Path.Exists(Data.paths_sys.InfraStructure.Services_File)) ? Data.paths_sys.InfraStructure.Services_File : throw new ServiceException("Caminho do arquivo dos serviços não encontrado.");

        //Tests
        Data.paths_sys.Tests.ApplicationTests_Dir = (Path.Exists(Data.paths_sys.Tests.ApplicationTests_Dir)) ? Data.paths_sys.Tests.ApplicationTests_Dir : throw new ServiceException("Caminho do diretório dos testes da aplicação não encontrado.");
        Data.paths_sys.Tests.ApplicationTests_File = (Path.Exists(Data.paths_sys.Tests.ApplicationTests_File)) ? Data.paths_sys.Tests.ApplicationTests_File : throw new ServiceException("Caminho do arquivo dos testes da aplicação não encontrado.");
        Data.paths_sys.Tests.DomainTests_Dir = (Path.Exists(Data.paths_sys.Tests.DomainTests_Dir)) ? Data.paths_sys.Tests.DomainTests_Dir : throw new ServiceException("Caminho do diretório dos testes do domínio não encontrado.");
        Data.paths_sys.Tests.DomainTests_File = (Path.Exists(Data.paths_sys.Tests.DomainTests_File)) ? Data.paths_sys.Tests.DomainTests_File : throw new ServiceException("Caminho do arquivo dos testes do domínio não encontrado.");

    }
}
