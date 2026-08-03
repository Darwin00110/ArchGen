using ArchGen.Application;
using ArchGen.Domain;
using ArchGen.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddScoped<IArchGenUseCase, ArchGenUseCase>();
builder.Services.AddScoped<ITerminalService, TerminalService>();
builder.Services.AddScoped<IFileService, FileService>();
using var host = builder.Build();

if (args.Length == 0)
{
    Console.WriteLine("Uso: ArchGen <NomeProjeto> <TipoDoProjeto(API|CONSOLE)>");
    return;
}

if (args.Length > 1 && (args[1].Equals("CONSOLE", StringComparison.OrdinalIgnoreCase) || args[1].Equals("API", StringComparison.OrdinalIgnoreCase)))
{
    using var scope = host.Services.CreateScope();
    var useCase = scope.ServiceProvider.GetRequiredService<IArchGenUseCase>();

    try
    {
        var result = await useCase.Exec(new CreateCleanArchRequest
        {
            NomeDoProjeto = args[0],
            TipoDoProjeto = args[1]
        });

        if (!string.IsNullOrWhiteSpace(result.Saida))
        {
            Console.WriteLine($"Saida: {result.Saida}");
        }

        if (!string.IsNullOrWhiteSpace(result.Error))
        {
            Console.WriteLine($"Erro: {result.Error}");
        }
    }
    catch (DomainException ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}
else
{
    Console.WriteLine($"Comando não reconhecido: {string.Join(" ", args)}");
}
