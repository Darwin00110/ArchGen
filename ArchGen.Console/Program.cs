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
builder.Services.AddScoped<IArchGenService, ArchGenService>();
builder.Services.AddScoped<IDotnetService, DotnetService>();
builder.Services.AddScoped<InternDomainGenerationService>();
builder.Services.AddScoped<InternApplicationGenerationService>();
builder.Services.AddScoped<InternInfraStructureGenerationService>();
builder.Services.AddScoped<InternTestsGenerationService>();
builder.Services.AddScoped<InternSolutionGenerationService>();
builder.Services.AddScoped<InternAPIGenerationService>();
builder.Services.AddScoped<InternConsoleGenerationService>();

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
    var archGenService = scope.ServiceProvider.GetRequiredService<IArchGenService>();
    archGenService.SetConfiguracaoDoProjeto(nomeProjeto, tipoDoProjeto);
    try
    {
        Console.WriteLine("Criando Estrutura Interna.");
        await archGenService.ExecInternStructure(args[2]);
    } catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
    }
} else
{
    Console.WriteLine("Info errada");
}
