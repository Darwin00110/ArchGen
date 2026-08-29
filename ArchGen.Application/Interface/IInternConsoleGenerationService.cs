namespace ArchGen.Application;

public interface IInternConsoleGenerationService
{
    public Task SetNomeProjeto(string nomeProjeto = "");
    public Task SetConsoleInternDomain(string path);
    public Task VerifyInternConsoleFiles();
    public Task<bool> CreateInternConsoleFiles();
}
