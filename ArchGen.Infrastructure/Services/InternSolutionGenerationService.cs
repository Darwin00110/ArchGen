using ArchGen.Application;
using ArchGen.Domain;

namespace ArchGen.Infrastructure;

public class InternSolutionGenerationService
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
    public async Task SetPathInternSolution(string path)
    {
        if (Path.Exists(path))
        {
            TipoDoProjeto = TipoDoProjeto.ToUpper();
            PathInternSolution = path;
            PathInternInfraStructure = Path.Combine(PathInternSolution, "InfraStructure");
            if (TipoDoProjeto.Equals("API"))
            {
                PathInternConsole_OR_API = Path.Combine(PathInternSolution, "API");
            }
            if (TipoDoProjeto.Equals("CONSOLE"))
            {
                PathInternConsole_OR_API = Path.Combine(PathInternSolution, "Console");
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
        if (File.Exists(Path.Combine(PathInternSolution, "Solution.slnx")))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(Solution.slnx) ja existe.");
        }
    }
    public async Task<bool> CreateSoluctionFiles()
    {
        if(!File.Exists(Path.Combine(PathInternSolution, "Solution.slnx")))
        {
           await _dotnet.CriarCamada_Solucao("Solution", PathInternSolution);
           await _dotnet.AddProject_in_the_Solution("Domain", PathInternSolution);
           await _dotnet.AddProject_in_the_Solution("Application", PathInternSolution);
           await _dotnet.AddProject_in_the_Solution("InfraStructure", PathInternSolution);
           await _dotnet.AddProject_in_the_Solution("Tests" ,PathInternSolution);
            if (TipoDoProjeto!.Equals("API"))
            {
                await _dotnet.AddProject_in_the_Solution("API", PathInternSolution);
                await _dotnet.ReferenceProject_in_the_Solution("API", "Domain", PathInternSolution);
                await _dotnet.ReferenceProject_in_the_Solution("API", "Application", PathInternSolution);
                await _dotnet.ReferenceProject_in_the_Solution("API",  "InfraStructure",PathInternSolution);
            }
            if (TipoDoProjeto!.Equals("CONSOLE"))
            {
                await _dotnet.AddProject_in_the_Solution("CONSOLE", PathInternSolution);
                await _dotnet.ReferenceProject_in_the_Solution("CONSOLE" , "Domain", PathInternSolution);
                await _dotnet.ReferenceProject_in_the_Solution("CONSOLE" , "Application", PathInternSolution);
                await _dotnet.ReferenceProject_in_the_Solution("CONSOLE" , "InfraStructure", PathInternSolution);
            }
            await _dotnet.ReferenceProject_in_the_Solution("Application", "Domain", PathInternSolution);
            await _dotnet.ReferenceProject_in_the_Solution("InfraStructure", "Domain", PathInternSolution);
            await _dotnet.ReferenceProject_in_the_Solution("InfraStructure", "Application", PathInternSolution);
            await _dotnet.ReferenceProject_in_the_Solution("Tests", "Domain", PathInternSolution);
            await _dotnet.ReferenceProject_in_the_Solution("Tests" , "Application", PathInternSolution);

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
