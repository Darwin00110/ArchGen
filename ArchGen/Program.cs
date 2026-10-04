﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ArchGen.Domain;
using ArchGen.InfraStructure;
using Microsoft.EntityFrameworkCore;
using ArchGen.Application;
var builder = Host.CreateApplicationBuilder(args);
var PathBanco = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "Data", "ArchGen.db"));
builder.Services.AddDbContext<AppDbContext>(e =>
{
    e.UseSqlite($"Data Source={PathBanco}");
});
builder.Services.AddScoped<IArchGenRepository, ArchGenRepository>();
builder.Services.AddScoped<IDotNetService, DotNetService>();
builder.Services.AddScoped<IMainTemplateService, MainTemplateService>();
builder.Services.AddScoped<Create_MainTemplateUseCase>();
builder.Services.AddScoped<ISystemOSService, SystemOSService>();
using var host = builder.Build();
using var scope = host.Services.CreateScope();

if (args.Length == 0)
{
    try
    {
        var createProject_WithMainTemplate = scope.ServiceProvider.GetRequiredService<Create_MainTemplateUseCase>();
        var GetSystemOS = scope.ServiceProvider.GetRequiredService<ISystemOSService>();
        Console.WriteLine("Argumentos invalidos ou não existentes, iniciando modo padrão.\n");
        Console.WriteLine("Insira o Nome do projeto: ");
        var nomeprojeto = Console.ReadLine();
        Console.WriteLine("Insira o Tipo do projeto, ex: (API, CONSOLE)");
        var tipodoprojeto = Console.ReadLine();
        Console.WriteLine("Insira o Path do projeto, obs: (caso não informe usaremos o Path relativo ao local da execução\nDesse programa)");
        var path = Console.ReadLine();
        path = (path == string.Empty) ? Environment.CurrentDirectory : path;
        Console.WriteLine("Executando o programa");
        createProject_WithMainTemplate.SystemOS = GetSystemOS.GetSystemOS();
        await createProject_WithMainTemplate.CreateProject(nomeprojeto!, tipodoprojeto!, path!);
        Console.WriteLine($"Criação concluida em {path}.");
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
    }
    return;
}
if (args.Length < 2)
{
    Console.WriteLine("Painel de ajuda ArchGen");
    return;
}
var nomeProjeto = args[0];
var tipoDoProjeto = args[1].ToUpper();
var pathDoProjeto = (args.Length > 2) ? args[2] : Environment.CurrentDirectory;
if (tipoDoProjeto.Equals("API") || tipoDoProjeto.Equals("CONSOLE"))
{
    var archGenService = scope.ServiceProvider.GetRequiredService<Create_MainTemplateUseCase>();
    try
    {
        Console.WriteLine("Criando Estrutura.");
        await archGenService.CreateProject(nomeProjeto, tipoDoProjeto, pathDoProjeto);
        Console.WriteLine("Estrutura Criada com sucesso.");
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
    }
    return;
}
else
{
    Console.WriteLine("Info errada");
}