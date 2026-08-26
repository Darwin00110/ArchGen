namespace ArchGen.Application;

public interface IDotnetService
{
    public Task<ResultTerminalResponse> CriarClasse(string nomeClasse, string Path);
    public Task<ResultTerminalResponse> CriarInterface(string nomeInterface, string Path);
    public Task<ResultTerminalResponse> CriarCamadaConsole(string nomeCamada, string Path);
    public Task<ResultTerminalResponse> CriarCamada_Classlib(string nomeCamada, string Path);
    public Task<ResultTerminalResponse> CriarCamada_xunit(string nomeCamada, string Path);
    public Task<ResultTerminalResponse> CriarCamada_API(string nomeCamada, string Path);
    public Task<ResultTerminalResponse> CriarCamada_Solucao(string nomeCamada, string Path);
    public Task<ResultTerminalResponse> AddProject_in_the_Solution(string PathSolucao, string WorkingPath);
    public Task<ResultTerminalResponse> ReferenceProject_in_the_Solution(string PathSolucao01,string PathSolucao02, string WorkingPath);
    public Task<ResultTerminalResponse> AddedPackageInTheProject(string NomePacote,string WorkingPath);
    public Task<ResultTerminalResponse> DbReferenceInfra_Migrations(string PathInfra, string Pathconsole_and_API, string Pathsolution);
    public Task<ResultTerminalResponse> DbReferenceInfra_Database(string PathInfra, string Pathconsole_and_API, string Pathsolution);
    
}
