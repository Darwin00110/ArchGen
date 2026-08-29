namespace ArchGen.Application;

public interface IArchGenService
{
    public Task<bool> VerifyInternStructure_Diretory(string path);
    
}
