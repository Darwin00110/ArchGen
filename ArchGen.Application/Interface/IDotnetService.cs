namespace ArchGen.Application;

public interface IDotnetService
{
    public Task<ResultTerminalResponse> CriarClasse(string nomeClasse, string Path);
    public Task<ResultTerminalResponse> CriarInterface(string nomeInterface, string Path);
    public Task<ResultTerminalResponse> CriarCamadaConsole(string nomeCamada, string Path);
    public Task<ResultTerminalResponse> CriarCamada_Classlib(string nomeCamada, string Path);
    public Task<ResultTerminalResponse> CriarCamada_xunit(string nomeCamada, string Path);
    public Task<ResultTerminalResponse> CriarCamada_API(string nomeCamada, string Path);
}
