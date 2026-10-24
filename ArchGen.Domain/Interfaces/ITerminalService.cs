namespace ArchGen.Domain.Interfaces;

public record ProcessResult(int ExitCode, string StandardOutput, string StandardError);

public interface ITerminalService
{
    Task<ProcessResult> ExecuteAsync(string command, string arguments, string workingDirectory = "", CancellationToken ct = default);
}
