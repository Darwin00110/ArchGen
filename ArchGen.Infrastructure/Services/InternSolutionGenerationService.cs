using ArchGen.Application;
using ArchGen.Domain;

namespace ArchGen.Infrastructure;

public class InternSolutionGenerationService : IInternSolutionGenerationService
{
    private readonly IDotnetService _dotnet;
    public string TipoDoProjeto = string.Empty;
    private string PathInternSolution = Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure");
    private string PathInternConsole_OR_API = string.Empty;
    private string PathInternInfraStructure = string.Empty;
    public InternSolutionGenerationService(IDotnetService dotnet)
    {
        _dotnet = dotnet;
    }
    private string NomeProjeto = string.Empty;
    public async Task SetNomeProjeto(string nomeProjeto = "")
    {
        NomeProjeto = nomeProjeto;
    }
    public async Task SetPathInternSolution(string path)
    {
        if (Path.Exists(path))
        {
            TipoDoProjeto = TipoDoProjeto.ToUpper();
            PathInternSolution = path;
            PathInternInfraStructure = Path.Combine(PathInternSolution, $"{NomeProjeto}.InfraStructure");
            if (TipoDoProjeto.Equals("API"))
            {
                PathInternConsole_OR_API = Path.Combine(PathInternSolution, $"{NomeProjeto}.API");
            }
            if (TipoDoProjeto.Equals("CONSOLE"))
            {
                PathInternConsole_OR_API = Path.Combine(PathInternSolution, $"{NomeProjeto}.Console");
            }
        } else
        {
            throw new ServiceException("O path Não existe");
        }
    }
    public void SetTipoDoProjeto(string tipodoprojeto)
    {
        TipoDoProjeto = tipodoprojeto;
    }
    public async Task VerifyFileSolution()
    {
        if (File.Exists(Path.Combine(PathInternSolution, $"{NomeProjeto}.slnx")))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(Solution.slnx) ja existe.");
        }
    }
    public async Task<bool> CreateSoluctionFiles()
    {
        if(!File.Exists(Path.Combine(PathInternSolution, $"{NomeProjeto}.slnx")))
        {
           await _dotnet.CriarCamada_Solucao(NomeProjeto, PathInternSolution);
           await _dotnet.AddProject_in_the_Solution($"{NomeProjeto}.Domain", PathInternSolution);
           await _dotnet.AddProject_in_the_Solution($"{NomeProjeto}.Application", PathInternSolution);
           await _dotnet.AddProject_in_the_Solution($"{NomeProjeto}.InfraStructure", PathInternSolution);
           await _dotnet.AddProject_in_the_Solution($"{NomeProjeto}.Tests" ,PathInternSolution);
            if (TipoDoProjeto!.Equals("API"))
            {
                await _dotnet.AddProject_in_the_Solution($"{NomeProjeto}.API", PathInternSolution);
                await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.API", $"{NomeProjeto}.Domain", PathInternSolution);
                await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.API", $"{NomeProjeto}.Application", PathInternSolution);
                await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.API",  $"{NomeProjeto}.InfraStructure",PathInternSolution);
            }
            if (TipoDoProjeto!.Equals("CONSOLE"))
            {
                await _dotnet.AddProject_in_the_Solution($"{NomeProjeto}.Console", PathInternSolution);
                await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.Console" , $"{NomeProjeto}.Domain", PathInternSolution);
                await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.Console" , $"{NomeProjeto}.Application", PathInternSolution);
                await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.Console" , $"{NomeProjeto}.InfraStructure", PathInternSolution);
            }
            await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.Application", $"{NomeProjeto}.Domain", PathInternSolution);
            await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.InfraStructure", $"{NomeProjeto}.Domain", PathInternSolution);
            await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.InfraStructure", $"{NomeProjeto}.Application", PathInternSolution);
            await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.Tests", $"{NomeProjeto}.Domain", PathInternSolution);
            await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.Tests" , $"{NomeProjeto}.Application", PathInternSolution);

            try
            {
                await _dotnet.DbReferenceInfra_Migrations(PathInternInfraStructure, PathInternConsole_OR_API, PathInternSolution);
                await _dotnet.DbReferenceInfra_Database(PathInternInfraStructure, PathInternConsole_OR_API, PathInternSolution);
            }
            catch (Exception e)
            {
                throw new ServiceException(e.Message);
            }
        }
        return true;
    }
}
