using ArchGen.Application;
using ArchGen.Domain;

namespace ArchGen.Infrastructure;

public class InternAPIGenerationService : IInternAPIGenerationService
{
    private string PathAPIFromInternEstructure = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure" ,"API"));
    private string PathInternSolution = Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure");
    private readonly IDotnetService _dotnet;
    public InternAPIGenerationService(IDotnetService dotnet)
    {
        _dotnet = dotnet;
    }
    private string NomeProjeto = string.Empty;
    public async Task SetNomeProjeto(string nomeProjeto = "")
    {
        NomeProjeto = nomeProjeto;
        NomeProjeto = (NomeProjeto == string.Empty) ? "API" : NomeProjeto; 
    }
    public async Task SetPathInternAPI(string path)
    {
        if (Path.Exists(path))
        {
            PathAPIFromInternEstructure = Path.Combine(path, $"{NomeProjeto}.API");
            PathInternSolution = path;
        } else
        {
            throw new ServiceException("O path Não existe");
        }
    }
    public async Task Validate_InternAPI()
    {
        var PathProgram = Path.Combine(PathAPIFromInternEstructure!, "Program.cs");
        var PathControllerDir = Path.Combine(PathAPIFromInternEstructure, "Controllers");
        var PathControllerFile = Path.Combine(PathControllerDir, "UserController.cs");
        if (Directory.Exists(PathAPIFromInternEstructure))
        {
            throw new ServiceException("Impossivel continuar, a Pasta(API) ja existe.");
        }
        if(Directory.Exists(Path.Combine(PathControllerDir)))
        {
            throw new ServiceException("Impossivel continuar, a Pasta(Controllers) ja existe.");
        }

        if(File.Exists(Path.Combine(PathProgram)))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(Program.cs) ja existe.");           
        }
        if(File.Exists(Path.Combine(PathControllerFile)))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(UserControllers.cs) ja existe.");           
        }
    }
    public async Task<bool> CreateInternAPI()
    {
        var Domain = (NomeProjeto == string.Empty) ? "Domain" : $"{NomeProjeto}.Domain";
        var Application = (NomeProjeto == string.Empty) ? "Application" : $"{NomeProjeto}.Application";
        var InfraStructure = (NomeProjeto == string.Empty) ? "InfraStructure" : $"{NomeProjeto}.InfraStructure";
        if (!Directory.Exists(PathAPIFromInternEstructure))
        {
            await _dotnet.CriarCamada_API($"{NomeProjeto}.API", PathInternSolution);
            await File.WriteAllTextAsync(Path.Combine(PathAPIFromInternEstructure, "Program.cs"), $@"
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using {Domain};
using {Application};
using {InfraStructure};

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
var solutionDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "".."", "".."", "".."", ""..""));
var PathDatabase = Path.Combine(solutionDirectory, ""{NomeProjeto}.InfraStructure"", ""Data"", ""database.db"");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($""Data Source={{PathDatabase}}""));
builder.Services.AddScoped<IUserUseCase, UserUseCase>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{{
    app.UseSwagger();
    app.UseSwaggerUI();
}}

app.MapControllers();
app.UseHttpsRedirection();
app.Run();");
        }
        await _dotnet.AddedPackageInTheProject("Microsoft.EntityFrameworkCore", PathAPIFromInternEstructure!);
        await _dotnet.AddedPackageInTheProject("Microsoft.EntityFrameworkCore.Design", PathAPIFromInternEstructure!);
        await _dotnet.AddedPackageInTheProject("Microsoft.EntityFrameworkCore.Sqlite", PathAPIFromInternEstructure!);
        await _dotnet.AddedPackageInTheProject("Swashbuckle.AspNetCore", PathAPIFromInternEstructure!);
        await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.API", $"{NomeProjeto}.Domain", PathInternSolution);
        await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.API", $"{NomeProjeto}.Application", PathInternSolution);
        await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.API", $"{NomeProjeto}.InfraStructure", PathInternSolution);
        return true;
    }
}
