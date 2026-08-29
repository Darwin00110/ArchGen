namespace ArchGen.Application;

public interface IInternAPIGenerationService
{
    public Task SetNomeProjeto(string nomeProjeto = "");
    public Task SetPathInternAPI(string path);
    public Task Validate_InternAPI();
    public Task<bool> CreateInternAPI();
}
