using System.Diagnostics;
using ArchGen.Application;
using ArchGen.Domain;

namespace ArchGen.InfraStructure;

public class DotNetService : IDotNetService
{
    private async Task<DotnetServiceResponse> ExecCommand(string Command, string arguments, string WorkingPath = "")
    {
        WorkingPath = (WorkingPath == string.Empty) ? Environment.CurrentDirectory : WorkingPath;
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = Command,
            Arguments = arguments,
            WorkingDirectory = WorkingPath,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false  
        };
        Process processo = new Process
        {
          StartInfo = psi  
        };
        processo.Start();
        var saida = processo.StandardOutput.ReadToEnd();
        var error = processo.StandardError.ReadToEnd();
        error = (error == string.Empty) ? throw new ServiceException(error) : error;
        await processo.WaitForExitAsync();
        return new DotnetServiceResponse
        {
            Saida = saida,
            Error = error
        };
    }
    public async Task<DotnetServiceResponse> CreateClass(string NomeClasse, string path) => await ExecCommand("dotnet", $"new class -n {NomeClasse}", path);
    public async Task<DotnetServiceResponse> CreateClassLib(string NomeClasse, string path) => await ExecCommand("dotnet", $"new classlib -n {NomeClasse}", path);
    public async Task<DotnetServiceResponse> CreateConsole(string NomeCamada, string path) => await ExecCommand("dotnet", $"new console -n {NomeCamada}", path);
    public async Task<DotnetServiceResponse> CreateAPI(string NomeCamada, string path) => await ExecCommand("dotnet", $"new webapi -n {NomeCamada}", path);
    public async Task<DotnetServiceResponse> CreateTests(string NomeCamada, string path) => await ExecCommand("dotnet", $"new xunit -n {NomeCamada}", path);
}
