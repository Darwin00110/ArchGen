using ArchGen.Application;
using ArchGen.Domain;

namespace ArchGen.Infrastructure;

public class InternConsoleGenerationService : IInternConsoleGenerationService
{
    private string PathInternSolution = Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure");
    private string PathConsoleFromInternEstructure = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure" ,"Console"));
    private readonly IDotnetService _dotnet;
    public InternConsoleGenerationService(IDotnetService dotnet)
    {
        _dotnet = dotnet;
    }
    private string NomeProjeto = string.Empty;
    public async Task SetNomeProjeto(string nomeProjeto = "")
    {
        NomeProjeto = nomeProjeto;
    }
    public async Task SetConsoleInternDomain(string path)
    {
        if (Path.Exists(path))
        {
            PathConsoleFromInternEstructure = Path.Combine(path, $"{NomeProjeto}.Console");
            PathInternSolution = path;
        } else
        {
            throw new ServiceException("O path Não existe");
        }
    }
    public async Task VerifyInternConsoleFiles()
    {
        var PathProgram = Path.Combine(PathConsoleFromInternEstructure!, "Program.cs");
        if (Directory.Exists(PathConsoleFromInternEstructure))
        {
            throw new ServiceException("Impossivel continuar, a Pasta(Console) ja existe.");
        }
        if(File.Exists(Path.Combine(PathProgram)))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(Program.cs) ja existe.");           
        }
    }
    public async Task<bool> CreateInternConsoleFiles()
    {
        var Domain = (NomeProjeto == string.Empty) ? "Domain" : $"{NomeProjeto}.Domain";
        var Application = (NomeProjeto == string.Empty) ? "Application" : $"{NomeProjeto}.Application";
        var InfraStructure = (NomeProjeto == string.Empty) ? "InfraStructure" : $"{NomeProjeto}.InfraStructure";
        if (!Directory.Exists(PathConsoleFromInternEstructure))
        {
            await _dotnet.CriarCamadaConsole($"{NomeProjeto}.Console",  PathInternSolution);
            await File.WriteAllTextAsync(Path.Combine(PathConsoleFromInternEstructure, "Program.cs")!, $@"
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using {Application};
using {InfraStructure};
using {Domain};
var builder = Host.CreateApplicationBuilder(args);
var solutionDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "".."", "".."", "".."", ""..""));
var PathBanco = Path.Combine(solutionDirectory, ""{NomeProjeto}.InfraStructure"", ""Data"", ""database.db"");
builder.Services.AddDbContext<AppDbContext>(e =>
{{
    e.UseSqlite($""Data Source={{PathBanco}}"");
}});
builder.Services.AddScoped<IUserUseCase, UserUseCase>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

using var host = builder.Build();
using var scope = host.Services.CreateScope();
if(args.Length == 0 || args.Length < 1)
{{
    Console.WriteLine(""Por favor, forneça o nome do projeto e o tipo do projeto (API ou CONSOLE) como argumentos."");
    return;
}}

var Argumentos = args[0];
if (Argumentos.Equals(""Parametro"")) {{
    var UseCase = scope.ServiceProvider.GetRequiredService<IUserUseCase>();
    var Saida = UseCase.Exec();
    Console.WriteLine(Saida);
}}
");
            await _dotnet.AddedPackageInTheProject("Microsoft.Extensions.Hosting", PathConsoleFromInternEstructure);
            await _dotnet.AddedPackageInTheProject("Microsoft.Extensions.DependencyInjection", PathConsoleFromInternEstructure);
            await _dotnet.AddedPackageInTheProject("Microsoft.EntityFrameworkCore.Sqlite", PathConsoleFromInternEstructure);
            await _dotnet.AddedPackageInTheProject("Microsoft.EntityFrameworkCore.Design", PathConsoleFromInternEstructure);
            await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.Console", $"{NomeProjeto}.Domain", PathInternSolution);
            await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.Console", $"{NomeProjeto}.Application", PathInternSolution);
            await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.Console", $"{NomeProjeto}.InfraStructure", PathInternSolution);
        }
        return true;
    }
}
