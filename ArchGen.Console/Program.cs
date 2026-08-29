using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ArchGen.Domain;
using ArchGen.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ArchGen.Application;
var builder = Host.CreateApplicationBuilder(args);
var PathBanco = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "Data","ArchGen.db"));
builder.Services.AddDbContext<AppDbContext>(e =>
{
    e.UseSqlite($"Data Source={PathBanco}");
});
builder.Services.AddScoped<IArchGenRepository, ArchGenRepository>();
builder.Services.AddScoped<ArchGenUseCase>();
builder.Services.AddScoped<IArchGenService, ArchGenService>();
builder.Services.AddScoped<IDotnetService, DotnetService>();
builder.Services.AddScoped<IInternDomainGenerationService, InternDomainGenerationService>();
builder.Services.AddScoped<IInternApplicationGenerationService, InternApplicationGenerationService>();
builder.Services.AddScoped<IInternInfraStructureGenerationService, InternInfraStructureGenerationService>();
builder.Services.AddScoped<IInternTestsGenerationService, InternTestsGenerationService>();
builder.Services.AddScoped<IInternSolutionGenerationService, InternSolutionGenerationService>();
builder.Services.AddScoped<IInternAPIGenerationService, InternAPIGenerationService>();
builder.Services.AddScoped<IInternConsoleGenerationService, InternConsoleGenerationService>();

using var host = builder.Build();
using var scope = host.Services.CreateScope();
if(args.Length == 0 || args.Length < 3)
{
    Console.WriteLine("Por favor, forneça o nome do projeto e o tipo do projeto (API ou CONSOLE) como argumentos.");
    return;
}
var nomeProjeto = args[0];
var tipoDoProjeto = args[1].ToUpper();

if (tipoDoProjeto.Equals("API") || tipoDoProjeto.Equals("CONSOLE"))
{
    var archGenService = scope.ServiceProvider.GetRequiredService<ArchGenUseCase>();
    archGenService.SetConfiguracaoDoProjeto(nomeProjeto, tipoDoProjeto);
    try
    {
        Console.WriteLine("Criando Estrutura.");
        await archGenService.ExecInternStructure(args[2]);
        Console.WriteLine("Estrutura Criada com sucesso.");
    } catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
    }
} else
{
    Console.WriteLine("Info errada");
}
