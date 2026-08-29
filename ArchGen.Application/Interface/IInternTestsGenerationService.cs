namespace ArchGen.Application;

public interface IInternTestsGenerationService
{
    public Task SetNomeProjeto(string nomeProjeto = "");
    public Task SetPathInternTests(string path);
    public Task<bool> CreateStructureInternTests(); 
}
