namespace ArchGen.Application;

public interface ITerminalService
{
    public Task<ResultadoComandoResponse> ExecutarComando(string[] Comandos, string diretorioTrabalho = "");
    public Task<bool> CreateFolder(string NomeDaPasta);
}
