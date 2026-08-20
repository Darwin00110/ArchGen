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

using var host = builder.Build();
using var scope = host.Services.CreateScope();
if(args.Length == 0 || args.Length < 2)
{
    Console.WriteLine("Por favor, forneça o nome do projeto e o tipo do projeto (API ou CONSOLE) como argumentos.");
    return;
}
var nomeProjeto = args[0];
var tipoDoProjeto = args[1].ToUpper();

if (tipoDoProjeto.Equals("API") || tipoDoProjeto.Equals("CONSOLE"))
{
    var archGenService = scope.ServiceProvider.GetRequiredService<IArchGenService>();
    archGenService.SetNomeDoProjeto(nomeProjeto);
    await archGenService.VerifySoluctionFiles();
} else
{
    
}
