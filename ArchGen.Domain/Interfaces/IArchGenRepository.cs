namespace ArchGen.Domain;

public interface IArchGenRepository
{
    public Task<bool> CreateNewData(ArchGenEntity data);
    public Task<string> GetPathDomain();
    public Task<string> GetPathApplication();
    public Task<string> GetPathInfrastructure();
    public Task<string> GetPathTests();
}
