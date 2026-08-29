namespace ArchGen.Application;

public interface IInternInfraStructureGenerationService
{
    public Task SetNomeProjeto(string nomeProjeto = "");
    public Task SetPathInternInfraStructure(string path);
    public Task VerifyInfraStructureInternStructure();
    public Task CreateInternStructure();
}
