namespace ArchGen.Application;

public interface IInternDomainGenerationService
{
    public  Task SetPathInternDomain(string path);
    public  Task SetNomeProjeto(string nomeProjeto = "");
    public  Task VerifyDomainInternStructure();
    public  Task CreateStructure();
}
