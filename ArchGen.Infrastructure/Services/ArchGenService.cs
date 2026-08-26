using System.Diagnostics;
using System.Reflection;
using ArchGen.Application;
using ArchGen.Domain;

namespace ArchGen.Infrastructure;

public class ArchGenService : IArchGenService
{
    private string? NomeDoProjeto;
    private string? TipoDoProjeto;

    
    private string PathInternSolution = Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure");
    private string PathAPIFromInternEstructure = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure" ,"API"));
    private string PathConsoleFromInternEstructure = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure" ,"Console"));
    
    private readonly IArchGenRepository _repo;
    private readonly InternDomainGenerationService _Intern_domain;
    private readonly InternApplicationGenerationService _Intern_application;
    private readonly InternInfraStructureGenerationService _Intern_infrastructure;
    private readonly InternAPIGenerationService _Intern_API;
    private readonly InternTestsGenerationService _Intern_tests;
    private readonly InternSolutionGenerationService _Intern_Solution;
    private readonly InternConsoleGenerationService _Intern_Console;
    public ArchGenService(IArchGenRepository repo, 
    InternDomainGenerationService intern_Domain,
    InternApplicationGenerationService intern_Application,
    InternInfraStructureGenerationService intern_InfraStructure,
    InternTestsGenerationService intern_Tests,
    InternAPIGenerationService intern_api,
    InternSolutionGenerationService intern_solution,
    InternConsoleGenerationService intern_console)
    {
        _repo = repo;
        _Intern_domain = intern_Domain;
        _Intern_application = intern_Application;
        _Intern_infrastructure = intern_InfraStructure;
        _Intern_tests = intern_Tests;
        _Intern_API = intern_api;
        _Intern_Solution = intern_solution;
        _Intern_Console = intern_console;
    }
    public void SetConfiguracaoDoProjeto(string nomeDoProjeto, string TipoProjeto)
    {
        NomeDoProjeto = nomeDoProjeto;
        TipoDoProjeto = TipoProjeto;
        TipoDoProjeto = TipoDoProjeto.ToUpper();
    }
    public async Task<bool> VerifyInternStructure_Diretory(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            path = PathInternSolution;
        }
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return true;
    }

    public async Task<bool> ExecInternStructure(string Path)
    {
        try
        {
            await VerifyInternStructure_Diretory(Path);

            await _Intern_domain.SetPathInternDomain(Path);
            await _Intern_domain.VerifyDomainInternStructure();
            await _Intern_domain.CreateStructure();

            await _Intern_application.SetPathInternApplication(Path);
            await _Intern_application.VerifyApplicationInternStructure();
            await _Intern_application.CreateInternApplicationFiles();

            await _Intern_infrastructure.SetPathInternInfraStructure(Path);
            await _Intern_infrastructure.VerifyInfraStructureInternStructure();
            await _Intern_infrastructure.CreateInternStructure();

            //Adicionar a verificação dos arquivos do Teste aqui 
            await _Intern_tests.SetPathInternTests(Path);
            await _Intern_tests.CreateStructureInternTests();


            if (TipoDoProjeto!.Equals("API"))
            {
                await _Intern_API.SetPathInternAPI(Path);
                await _Intern_API.Validate_InternAPI();
                await _Intern_API.CreateInternAPI();
            }    
            if (TipoDoProjeto!.Equals("CONSOLE"))
            {
                await _Intern_Console.SetConsoleInternDomain(Path);
                await _Intern_Console.VerifyInternConsoleFiles();
                await _Intern_Console.CreateInternConsoleFiles();
            }
            _Intern_Solution.SetTipoDoProjeto(TipoDoProjeto);
            await _Intern_Solution.SetPathInternSolution(Path); 
            await _Intern_Solution.VerifyFileSolution();
            await _Intern_Solution.CreateSoluctionFiles();
        } catch(Exception e)
        {
            throw new ServiceException($"Error: {e.Message}");
        } 
        return true;
    }
    
    
}
