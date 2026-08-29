using ArchGen.Application;
using ArchGen.Domain;

namespace ArchGen.Infrastructure;

public class InternTestsGenerationService : IInternTestsGenerationService
{
    private readonly IDotnetService _dotnet;
    private string PathTestsFromInternEstructure = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure" ,"Tests"));
    private string PathInternSolution = Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure");

    public InternTestsGenerationService(IDotnetService dotnet)
    {
        _dotnet = dotnet;
    }
    private string NomeProjeto = string.Empty;
    public async Task SetNomeProjeto(string nomeProjeto = "")
    {
        NomeProjeto = nomeProjeto;
    }
    public async Task SetPathInternTests(string path)
    {
        if (Path.Exists(path))
        {
            PathTestsFromInternEstructure = Path.Combine(path, $"{NomeProjeto}.Tests");   
            PathInternSolution = path;
        } else
        {
            throw new ServiceException("O path Não existe");
        }
    }
    public async Task<bool> CreateStructureInternTests() {
        if (!Directory.Exists(PathTestsFromInternEstructure))
        {
            await _dotnet.CriarCamada_xunit($"{NomeProjeto}.Tests", PathInternSolution);
            await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.Tests", $"{NomeProjeto}.Domain", PathInternSolution);
            await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.Tests", $"{NomeProjeto}.Application", PathInternSolution);
        }
        return true;
    }
}
