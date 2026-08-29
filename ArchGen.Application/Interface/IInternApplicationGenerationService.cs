namespace ArchGen.Application;

public interface IInternApplicationGenerationService
{
    public Task SetNomeProjeto(string nomeProjeto = "");
    public Task SetPathInternApplication(string path);
    public Task VerifyApplicationInternStructure();
    public Task<bool> CreateInternApplicationFiles(); 
}
