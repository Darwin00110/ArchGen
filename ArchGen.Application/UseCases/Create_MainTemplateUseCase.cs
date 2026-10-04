using ArchGen.Domain;
using Microsoft.VisualBasic;

namespace ArchGen.Application;

public class Create_MainTemplateUseCase
{
    private readonly IArchGenRepository _repo;
    private readonly IMainTemplateService _main;
    public required string SystemOS { get; set; }
    public Create_MainTemplateUseCase(IArchGenRepository repo, IMainTemplateService main)
    {
        _repo = repo;
        _main = main;
    }
    public async Task CreateProject(string NomeProjeto, string TipoProjeto, string PathProjeto = "")
    {
        NomeProjeto = (NomeProjeto == string.Empty) ? "ArchGenProject" : NomeProjeto;
        TipoProjeto = (TipoProjeto == string.Empty) ? "API" : TipoProjeto;
        var pathDomain = Path.Combine(PathProjeto, $"{NomeProjeto}.Domain");
        var pathApplication = Path.Combine(PathProjeto, $"{NomeProjeto}.Application");
        var pathInfraStructure = Path.Combine(PathProjeto, $"{NomeProjeto}.InfraStructure");
        var pathTests = Path.Combine(PathProjeto, $"{NomeProjeto}.Tests");
        var pathConsole_OR_API = Path.Combine(PathProjeto, $"{NomeProjeto}.{TipoProjeto}");
        
        ProjetoUsuario.VerifyProject();
        var VerificarSeExisteProjetoNoBanco = (await _repo.VerifyExists_OnThe_Databank(NomeProjeto) == true) ? throw new ApplicationException("Error: Impossivel criar um projeto que ja exista, execute (ArchGen <NomeDoProjeto> --force-create)"
        + "para apagar os arquivos restantes e criar o projeto com esse nome.") : false;
    }
}