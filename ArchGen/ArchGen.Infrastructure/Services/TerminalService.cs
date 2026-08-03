using System.Diagnostics;
using ArchGen.Application;
using ArchGen.Domain;

namespace ArchGen.Infrastructure;

public class TerminalService : ITerminalService
{
    public Task<ResultadoComandoResponse> ExecutarComando(string[] Comandos, string diretorioTrabalho = "")
    {
        if (string.IsNullOrEmpty(diretorioTrabalho))
        {
            diretorioTrabalho = Environment.CurrentDirectory;
        }

        var Comando = string.Join(" ", Comandos);
        var psi = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c {Comando}",
            WorkingDirectory = diretorioTrabalho,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var process = new Process
        {
            StartInfo = psi
        };

        process.Start();
        var saida = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        return Task.FromResult(new ResultadoComandoResponse
        {
            Error = error,
            Saida = saida
        });
    }

    public Task<bool> CreateFolder(string NomeDaPasta)
    {
        if (string.IsNullOrEmpty(NomeDaPasta))
        {
            throw new ServiceException("Nome da pasta está vazio, campo obrigatorio");
        }

        Directory.CreateDirectory(NomeDaPasta);
        return Task.FromResult(Path.Exists(NomeDaPasta));
    }
}
