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
if(args.Length < 3)
{
    try
    {
        
    var archGenService = scope.ServiceProvider.GetRequiredService<ArchGenUseCase>();
    Console.WriteLine("Argumentos invalidos ou não existentes, iniciando modo padrão.\n");
    Console.WriteLine("Insira o Nome do projeto: ");
    var nomeprojeto = Console.ReadLine();
    Console.WriteLine("Insira o Tipo do projeto, ex: (API, CONSOLE)");
    var tipodoprojeto = Console.ReadLine();
    
    Console.WriteLine("Insira o Path do projeto, obs: (caso não informe usaremos o Path relativo ao local da execução\nDesse programa)");
    var path = Console.ReadLine();
    path = (path == string.Empty) ? Environment.CurrentDirectory : path;
    Console.WriteLine("Executando o programa");
    archGenService.SetConfiguracaoDoProjeto(nomeprojeto!, tipodoprojeto!);
    await archGenService.ExecInternStructure(path!);
    Console.WriteLine($"Criação concluida em {path}.");
    return;
    } catch(Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
    }
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
