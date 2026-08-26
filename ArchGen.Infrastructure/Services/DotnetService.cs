using System.Diagnostics;
using ArchGen.Application;
using ArchGen.Domain;

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
    public async Task<ResultTerminalResponse> CriarCamada_Solucao(string nomeSolucao, string Path)
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"new sln -n {nomeSolucao}",
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
    public async Task<ResultTerminalResponse> AddProject_in_the_Solution(string PathSolucao, string WorkingPath)
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"sln add {PathSolucao}",
            WorkingDirectory = WorkingPath,
            
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
    public async Task<ResultTerminalResponse> ReferenceProject_in_the_Solution(string PathSolucao01,string PathSolucao02, string WorkingPath)
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"add {PathSolucao01} reference {PathSolucao02}",
            WorkingDirectory = WorkingPath,
            
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
    public async Task<ResultTerminalResponse> AddedPackageInTheProject(string NomePacote,string WorkingPath)
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"add package {NomePacote}",
            WorkingDirectory = WorkingPath,
            
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
    public async Task<ResultTerminalResponse> DbReferenceInfra_Migrations(string PathInfra, string Pathconsole_and_API, string Pathsolution)
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            WorkingDirectory = Pathsolution,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true   
        };
        psi.ArgumentList.Add("ef");
        psi.ArgumentList.Add("migrations");
        psi.ArgumentList.Add("add");
        psi.ArgumentList.Add("Initial_DB");
        psi.ArgumentList.Add("--project");
        psi.ArgumentList.Add(PathInfra);
        psi.ArgumentList.Add("--startup-project");
        psi.ArgumentList.Add(Pathconsole_and_API);

        var processo = new Process{
            StartInfo = psi
        };
        processo.Start();
        var saidaTask = processo.StandardOutput.ReadToEndAsync();
        var errorTask = processo.StandardError.ReadToEndAsync();
        await Task.WhenAll(saidaTask, errorTask, processo.WaitForExitAsync());

        var saida = await saidaTask;
        var error = await errorTask;
        if (processo.ExitCode != 0)
        {
            var detalhes = string.Join(Environment.NewLine,
                new[] { saida, error }.Where(texto => !string.IsNullOrWhiteSpace(texto)));
            throw new ServiceException($"Falha ao criar a migration. Exit code: {processo.ExitCode}.{Environment.NewLine}{detalhes}");
        }

        return new ResultTerminalResponse
        {
          Error = error,
          Saida = saida  
        }; 
    }
    public async Task<ResultTerminalResponse> DbReferenceInfra_Database(string PathInfra, string Pathconsole_and_API, string Pathsolution)
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            WorkingDirectory = Pathsolution,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true   
        };
        psi.ArgumentList.Add("ef");
        psi.ArgumentList.Add("database");
        psi.ArgumentList.Add("update");
        psi.ArgumentList.Add("--project");
        psi.ArgumentList.Add(PathInfra);
        psi.ArgumentList.Add("--startup-project");
        psi.ArgumentList.Add(Pathconsole_and_API);

        var processo = new Process{
            StartInfo = psi
        };
        processo.Start();
        var saidaTask = processo.StandardOutput.ReadToEndAsync();
        var errorTask = processo.StandardError.ReadToEndAsync();
        await Task.WhenAll(saidaTask, errorTask, processo.WaitForExitAsync());

        var saida = await saidaTask;
        var error = await errorTask;
        if (processo.ExitCode != 0)
        {
            var detalhes = string.Join(Environment.NewLine,
                new[] { saida, error }.Where(texto => !string.IsNullOrWhiteSpace(texto)));
            throw new ServiceException($"Falha ao criar database. Exit code: {processo.ExitCode}.{Environment.NewLine}{detalhes}");
        }

        return new ResultTerminalResponse
        {
          Error = error,
          Saida = saida  
        }; 
    }
}
