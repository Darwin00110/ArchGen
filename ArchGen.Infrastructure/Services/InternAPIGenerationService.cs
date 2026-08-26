using ArchGen.Application;
using ArchGen.Domain;

namespace ArchGen.Infrastructure;

public class InternAPIGenerationService
{
    private string PathAPIFromInternEstructure = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure" ,"API"));
    private string PathInternSolution = Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure");
    private readonly IDotnetService _dotnet;
    public InternAPIGenerationService(IDotnetService dotnet)
    {
        _dotnet = dotnet;
    }
    public async Task SetPathInternAPI(string path)
    {
        if (Path.Exists(path))
        {
            PathAPIFromInternEstructure = Path.Combine(path, "API");
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
        if (!Directory.Exists(PathAPIFromInternEstructure))
        {
            await _dotnet.CriarCamada_API("API", PathInternSolution);
            await File.WriteAllTextAsync(Path.Combine(PathAPIFromInternEstructure, "Program.cs"), @"

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Domain;
using Application;
using InfraStructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
var PathDatabase = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "".."", ""InfraStructure"", ""Data"", ""database.db""));
Console.WriteLine(PathDatabase);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($""Data Source={PathDatabase}""));
builder.Services.AddScoped<IUserUseCase, UserUseCase>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.Run();");
        }
        await _dotnet.AddedPackageInTheProject("Microsoft.EntityFrameworkCore", PathAPIFromInternEstructure!);
        await _dotnet.AddedPackageInTheProject("Microsoft.EntityFrameworkCore.Design", PathAPIFromInternEstructure!);
        await _dotnet.AddedPackageInTheProject("Microsoft.EntityFrameworkCore.Sqlite", PathAPIFromInternEstructure!);
        await _dotnet.AddedPackageInTheProject("Swashbuckle.AspNetCore", PathAPIFromInternEstructure!);
        return true;
    }
}
