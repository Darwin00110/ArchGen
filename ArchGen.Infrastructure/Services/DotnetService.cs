using System.Diagnostics;
using ArchGen.Application;

namespace ArchGen.Infrastructure;

public class DotnetService : IDotnetService
{
    public async Task<ResultTerminalResponse> CriarClasse(string nomeClasse, string Path)
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"new class -n {nomeClasse}",
            WorkingDirectory = Path,
            RedirectStandardError = true,
            RedirectStandardOutput = true   
        };
        var processo = new Process{
            StartInfo = psi
        };
        processo.Start();
        var saida = await processo.StandardOutput.ReadToEndAsync();
        var error = await processo.StandardError.ReadToEndAsync();
        await processo.WaitForExitAsync();
        return new ResultTerminalResponse
        {
          Error = error,
          Saida = saida  
        };
    }
    public async Task<ResultTerminalResponse> CriarInterface(string nomeInterface, string Path)
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"new interface -n {nomeInterface}",
            WorkingDirectory = Path,
            RedirectStandardError = true,
            RedirectStandardOutput = true   
        };
        var processo = new Process{
            StartInfo = psi
        };
        processo.Start();
        var saida = await processo.StandardOutput.ReadToEndAsync();
        var error = await processo.StandardError.ReadToEndAsync();
        await processo.WaitForExitAsync();
        return new ResultTerminalResponse
        {
          Error = error,
          Saida = saida  
        };
    }
    public async Task<ResultTerminalResponse> CriarCamadaConsole(string nomeCamada, string Path)
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"new console -n {nomeCamada}",
            WorkingDirectory = Path,
            RedirectStandardError = true,
            RedirectStandardOutput = true   
        };
        var processo = new Process{
            StartInfo = psi
        };
        processo.Start();
        var saida = await processo.StandardOutput.ReadToEndAsync();
        var error = await processo.StandardError.ReadToEndAsync();
        await processo.WaitForExitAsync();
        return new ResultTerminalResponse
        {
          Error = error,
          Saida = saida  
        };
    }
    public async Task<ResultTerminalResponse> CriarCamada_Classlib(string nomeCamada, string Path)
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"new classlib -n {nomeCamada}",
            WorkingDirectory = Path,
            
            RedirectStandardError = true,
            RedirectStandardOutput = true   
        };
        var processo = new Process{
            StartInfo = psi
        };
        processo.Start();
        var saida = await processo.StandardOutput.ReadToEndAsync();
        var error = await processo.StandardError.ReadToEndAsync();
        await processo.WaitForExitAsync();
        return new ResultTerminalResponse
        {
          Error = error,
          Saida = saida  
        };
    }
    public async Task<ResultTerminalResponse> CriarCamada_xunit(string nomeCamada, string Path)
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"new xunit -n {nomeCamada}",
            WorkingDirectory = Path,
            
            RedirectStandardError = true,
            RedirectStandardOutput = true   
        };
        var processo = new Process{
            StartInfo = psi
        };
        processo.Start();
        var saida = await processo.StandardOutput.ReadToEndAsync();
        var error = await processo.StandardError.ReadToEndAsync();
        await processo.WaitForExitAsync();
        return new ResultTerminalResponse
        {
          Error = error,
          Saida = saida  
        };
    }
    public async Task<ResultTerminalResponse> CriarCamada_API(string nomeCamada, string Path)
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"new webapi -n {nomeCamada}",
            WorkingDirectory = Path,
            
            RedirectStandardError = true,
            RedirectStandardOutput = true   
        };
        var processo = new Process{
            StartInfo = psi
        };
        processo.Start();
        var saida = await processo.StandardOutput.ReadToEndAsync();
        var error = await processo.StandardError.ReadToEndAsync();
        await processo.WaitForExitAsync();
        return new ResultTerminalResponse
        {
          Error = error,
          Saida = saida  
        };
    }
}
