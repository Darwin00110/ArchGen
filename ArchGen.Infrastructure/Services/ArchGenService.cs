using System.Diagnostics;
using System.Reflection;
using ArchGen.Application;
using ArchGen.Domain;

namespace ArchGen.Infrastructure;

public class ArchGenService : IArchGenService
{
    private string? NomeDoProjeto;
    private string? TipoDoProjeto;

    private string PathAPIFromInternEstructure = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure" ,"API"));
    private string PathConsoleFromInternEstructure = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure" ,"Console"));
    
    private string PathInternSolution = Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure");
    private readonly IArchGenRepository _repo;
    private readonly InternDomainGenerationService _Intern_domain;
    private readonly InternApplicationGenerationService _Intern_application;
    private readonly InternInfraStructureGenerationService _Intern_infrastructure;
    public ArchGenService(IArchGenRepository repo, 
    InternDomainGenerationService intern_Domain,
    InternApplicationGenerationService intern_Application,
    InternInfraStructureGenerationService intern_InfraStructure)
    {
        _repo = repo;
        _Intern_domain = intern_Domain;
        _Intern_application = intern_Application;
        _Intern_infrastructure = intern_InfraStructure;
    }
    public void SetConfiguracaoDoProjeto(string nomeDoProjeto, string TipoProjeto)
    {
        NomeDoProjeto = nomeDoProjeto;
        TipoDoProjeto = TipoProjeto;
        TipoDoProjeto = TipoDoProjeto.ToUpper();
    }
    public async Task<bool> VerifyInternStructure_Diretory()
    {
        if (!Directory.Exists(PathInternSolution))
        {
            Directory.CreateDirectory(PathInternSolution);
        }
        return true;
    }

    public async Task<bool> ExecInternStructure()
    {
        try
        {
            await _Intern_domain.VerifyDomainInternStructure();
            await _Intern_domain.CreateStructure();

            await _Intern_application.VerifyApplicationInternStructure();
            await _Intern_application.CreateInternApplicationFiles();

            await _Intern_infrastructure.VerifyInfraStructureInternStructure();
            await _Intern_infrastructure.CreateInternStructure();
        } catch(Exception e)
        {
            throw new ServiceException($"Error: {e.Message}");
        }
        if (!Directory.Exists(PathTestsFromInternEstructure))
        {
            await VerifyInternTestsFiles();
        } else
        {
            Console.WriteLine("Camada ja existente.");
            
        }
        if (TipoDoProjeto!.Equals("API") && !Directory.Exists(PathAPIFromInternEstructure))
        {
            await VerifyInternAPIFiles();
        } 
        if (TipoDoProjeto!.Equals("CONSOLE") && !Directory.Exists(PathConsoleFromInternEstructure))
        {
            await VerifyInternConsoleFiles();
        } 
        if(!File.Exists(Path.Combine(PathInternSolution, "Solution.sln")))
        {
            await VerifySoluctionFiles();
        } else
        {
            Console.WriteLine("Camada ja existente.");
        }
        Console.WriteLine("Estrutura interna criada com sucesso.");
        return true;
    }

    
    private class ProcessResponse
    {
        public string Saida {get; set;} = string.Empty;
        public string Error {get; set;} = string.Empty;
    }
    private async Task<ProcessResponse> StartProcesso(string Command, string Arguments, bool UseShell = false, string WorkingDiretory = "")
    {
        WorkingDiretory = (WorkingDiretory == string.Empty) ? Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure") : WorkingDiretory;
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = Command,
            Arguments = Arguments,
            WorkingDirectory = WorkingDiretory,
            UseShellExecute = UseShell,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        Process process = new Process
        {
            StartInfo = psi
        };
        process.Start();
        string saida = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        return new ProcessResponse
        {
            Error = error,
            Saida = saida
        };
    }
    public async Task<bool> VerifyInternTestsFiles() {
        if (!Directory.Exists(PathTestsFromInternEstructure))
        {
            await StartProcesso("dotnet", "new xunit -n Tests", false, PathInternSolution);
        }
        return true;
    }
    public async Task<bool> VerifySoluctionFiles()
    {
        if(!File.Exists(Path.Combine(PathInternSolution, "Solution.sln")))
        {
           await StartProcesso("dotnet", "new sln -n Solution", false, PathInternSolution);
           await StartProcesso("dotnet", "sln add Domain", false, PathInternSolution);
           await StartProcesso("dotnet", "sln add Application", false, PathInternSolution);
           await StartProcesso("dotnet", "sln add InfraStructure", false, PathInternSolution);
           await StartProcesso("dotnet", "sln add Tests", false, PathInternSolution);
            if (TipoDoProjeto!.Equals("API"))
            {
                await StartProcesso("dotnet", "sln add API", false, PathInternSolution);
                await StartProcesso("dotnet", "add API reference Domain", false, PathInternSolution);
                await StartProcesso("dotnet", "add API reference Application", false, PathInternSolution);
                await StartProcesso("dotnet", "add API reference InfraStructure", false, PathInternSolution);
            }
            if (TipoDoProjeto!.Equals("CONSOLE"))
            {
                await StartProcesso("dotnet", "sln add CONSOLE", false, PathInternSolution);
                await StartProcesso("dotnet", "add CONSOLE reference Domain", false, PathInternSolution);
                await StartProcesso("dotnet", "add CONSOLE reference Application", false, PathInternSolution);
                await StartProcesso("dotnet", "add CONSOLE reference InfraStructure", false, PathInternSolution);
            }
            await StartProcesso("dotnet", "add Application reference Domain", false, PathInternSolution);
            await StartProcesso("dotnet", "add InfraStructure reference Domain", false, PathInternSolution);
            await StartProcesso("dotnet", "add InfraStructure reference Application", false, PathInternSolution);
            await StartProcesso("dotnet", "add Tests reference Domain", false, PathInternSolution);
            await StartProcesso("dotnet", "add Tests reference Application", false, PathInternSolution);
        }
        return true;
    }
    public async Task<bool> VerifyInternConsoleFiles()
    {
        if (!Directory.Exists(PathConsoleFromInternEstructure))
        {
            await StartProcesso("dotnet", "new console -n Console", false, PathInternSolution);
            await File.WriteAllTextAsync(Path.Combine(PathConsoleFromInternEstructure, "Program.cs")!, $@"
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Application;
using InfraStructure;
using Domain;
var builder = Host.CreateApplicationBuilder(args);
var PathBanco = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, '..', 'ArchGen.Infrastructure', 'Data','ArchGen.db'));
builder.Services.AddDbContext<AppDbContext>(e =>
{{
    e.UseSqlite($'Data Source={{PathBanco}}');
}});
builder.Services.AddScoped<IUserUseCase, UserUseCase>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

using var host = builder.Build();
using var scope = host.Services.CreateScope();
if(args.Length == 0 || args.Length < 1)
{{
    Console.WriteLine('Por favor, forneça o nome do projeto e o tipo do projeto (API ou CONSOLE) como argumentos.');
    return;
}}

var Argumentos = args[0];
if (Argumentos.Equals('Parametro')) {{
    var UseCase = scope.ServiceProvider.GetRequiredService<IUserUseCase>();
    var Saida = UseCase.Exec();
    Console.WriteLine(Saida);
}}
");
            await StartProcesso("dotnet", @"add package Microsoft.Extensions.Hosting", false, PathConsoleFromInternEstructure);
        }
        return true;
    }
    public async Task<bool> VerifyInternAPIFiles()
    {
        if (!Directory.Exists(PathAPIFromInternEstructure))
        {
            await StartProcesso("dotnet", "new webapi -n API", false, PathInternSolution);
        }
        return true;
    }
}
