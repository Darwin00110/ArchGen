using System.Diagnostics;
using ArchGen.Application;
using ArchGen.Domain;

namespace ArchGen.Infrastructure;

public class DotnetService : IDotnetService
{
    public Task<ResultTerminalResponse> CriarClasse(string nomeClasse, string path) =>
        ExecuteDotnetAsync(path, "criar a classe", "new", "class", "-n", nomeClasse);

    public Task<ResultTerminalResponse> CriarInterface(string nomeInterface, string path) =>
        ExecuteDotnetAsync(path, "criar a interface", "new", "interface", "-n", nomeInterface);

    public Task<ResultTerminalResponse> CriarCamadaConsole(string nomeCamada, string path) =>
        ExecuteDotnetAsync(path, "criar o projeto Console", "new", "console", "-n", nomeCamada);

    public Task<ResultTerminalResponse> CriarCamada_Classlib(string nomeCamada, string path) =>
        ExecuteDotnetAsync(path, "criar a biblioteca de classes", "new", "classlib", "-n", nomeCamada);

    public Task<ResultTerminalResponse> CriarCamada_xunit(string nomeCamada, string path) =>
        ExecuteDotnetAsync(path, "criar o projeto de testes", "new", "xunit", "-n", nomeCamada);

    public Task<ResultTerminalResponse> CriarCamada_API(string nomeCamada, string path) =>
        ExecuteDotnetAsync(path, "criar o projeto API", "new", "webapi", "-n", nomeCamada);

    public Task<ResultTerminalResponse> CriarCamada_Solucao(string nomeSolucao, string path) =>
        ExecuteDotnetAsync(path, "criar a solution", "new", "sln", "-n", nomeSolucao);

    public Task<ResultTerminalResponse> AddProject_in_the_Solution(string projeto, string workingPath) =>
        ExecuteDotnetAsync(workingPath, "adicionar o projeto à solution", "sln", "add", projeto);

    public Task<ResultTerminalResponse> ReferenceProject_in_the_Solution(string projeto, string referencia, string workingPath) =>
        ExecuteDotnetAsync(workingPath, "adicionar a referência de projeto", "add", projeto, "reference", referencia);

    public Task<ResultTerminalResponse> AddedPackageInTheProject(string nomePacote, string workingPath) =>
        ExecuteDotnetAsync(workingPath, "adicionar o pacote", "add", "package", nomePacote);

    public Task<ResultTerminalResponse> DbReferenceInfra_Migrations(string pathInfra, string pathConsoleOrApi, string pathSolution) =>
        ExecuteDotnetAsync(pathSolution, "criar a migration", "ef", "migrations", "add", "Initial_DB", "--project", pathInfra, "--startup-project", pathConsoleOrApi);

    public Task<ResultTerminalResponse> DbReferenceInfra_Database(string pathInfra, string pathConsoleOrApi, string pathSolution) =>
        ExecuteDotnetAsync(pathSolution, "atualizar o banco de dados", "ef", "database", "update", "--project", pathInfra, "--startup-project", pathConsoleOrApi);

    private static async Task<ResultTerminalResponse> ExecuteDotnetAsync(string workingPath, string operation, params string[] arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            WorkingDirectory = workingPath,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true
        };

        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        using var process = new Process { StartInfo = startInfo };
        process.Start();

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();
        await Task.WhenAll(outputTask, errorTask, process.WaitForExitAsync());

        var output = await outputTask;
        var error = await errorTask;
        if (process.ExitCode != 0)
        {
            var details = string.Join(Environment.NewLine,
                new[] { output, error }.Where(text => !string.IsNullOrWhiteSpace(text)));
            throw new ServiceException($"Falha ao {operation}. Exit code: {process.ExitCode}.{Environment.NewLine}{details}");
        }

        return new ResultTerminalResponse { Saida = output, Error = error };
    }
}
