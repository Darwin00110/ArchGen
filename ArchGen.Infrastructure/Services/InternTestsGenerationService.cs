using ArchGen.Application;

namespace ArchGen.Infrastructure;

public class InternTestsGenerationService
{
    private readonly IDotnetService _dotnet;
    private string PathTestsFromInternEstructure = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure" ,"Tests"));
    private string PathInternSolution = Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure");

    public InternTestsGenerationService(IDotnetService dotnet)
    {
        _dotnet = dotnet;
    }
    public async Task<bool> CreateStructureInternTests() {
        if (!Directory.Exists(PathTestsFromInternEstructure))
        {
            await _dotnet.CriarCamada_xunit("Tests", PathInternSolution);
        }
        return true;
    }
}
