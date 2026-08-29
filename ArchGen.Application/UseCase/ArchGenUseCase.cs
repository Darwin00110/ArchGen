using ArchGen.Domain;

namespace ArchGen.Application;

public class ArchGenUseCase
{
    private string? NomeDoProjeto;
    private string? TipoDoProjeto;
        
    private readonly IArchGenRepository _repo;
    private readonly IArchGenService _archgen;
    private readonly IInternDomainGenerationService _Intern_domain;
    private readonly IInternApplicationGenerationService _Intern_application;
    private readonly IInternInfraStructureGenerationService _Intern_infrastructure;
    private readonly IInternAPIGenerationService _Intern_API;
    private readonly IInternTestsGenerationService _Intern_tests;
    private readonly IInternSolutionGenerationService _Intern_Solution;
    private readonly IInternConsoleGenerationService _Intern_Console;
    public ArchGenUseCase(IArchGenRepository repo,
    IArchGenService _ArchGenService,
    IInternDomainGenerationService intern_Domain,
    IInternApplicationGenerationService intern_Application,
    IInternInfraStructureGenerationService intern_InfraStructure,
    IInternTestsGenerationService intern_Tests,
    IInternAPIGenerationService intern_api,
    IInternSolutionGenerationService intern_solution,
    IInternConsoleGenerationService intern_console)
    {
        _repo = repo;
        _archgen = _ArchGenService;
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

    public async Task<bool> ExecInternStructure(string Path)
    {
        try
        {
            await _archgen.VerifyInternStructure_Diretory(Path);

            await _Intern_domain.SetNomeProjeto(NomeDoProjeto!);
            await _Intern_domain.SetPathInternDomain(Path);
            await _Intern_domain.VerifyDomainInternStructure();
            await _Intern_domain.CreateStructure();

            await _Intern_application.SetNomeProjeto(NomeDoProjeto!);
            await _Intern_application.SetPathInternApplication(Path);
            await _Intern_application.VerifyApplicationInternStructure();
            await _Intern_application.CreateInternApplicationFiles();

            await _Intern_infrastructure.SetNomeProjeto(NomeDoProjeto!);
            await _Intern_infrastructure.SetPathInternInfraStructure(Path);
            await _Intern_infrastructure.VerifyInfraStructureInternStructure();
            await _Intern_infrastructure.CreateInternStructure();

            //Adicionar a verificação dos arquivos do Teste aqui 
            await _Intern_tests.SetNomeProjeto(NomeDoProjeto!);
            await _Intern_tests.SetPathInternTests(Path);
            await _Intern_tests.CreateStructureInternTests();

            if (TipoDoProjeto!.Equals("API"))
            {
                await _Intern_API.SetNomeProjeto(NomeDoProjeto!);
                await _Intern_API.SetPathInternAPI(Path);
                await _Intern_API.Validate_InternAPI();
                await _Intern_API.CreateInternAPI();
            }    
            if (TipoDoProjeto!.Equals("CONSOLE"))
            {
                await _Intern_Console.SetNomeProjeto(NomeDoProjeto!);
                await _Intern_Console.SetConsoleInternDomain(Path);
                await _Intern_Console.VerifyInternConsoleFiles();
                await _Intern_Console.CreateInternConsoleFiles();
            }
            _Intern_Solution.SetTipoDoProjeto(TipoDoProjeto);
            await _Intern_Solution.SetNomeProjeto(NomeDoProjeto!);
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
