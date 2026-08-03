namespace ArchGen.Application;

public interface IFileArchService
{
    public Task<string> GetDataDomain_Entities();
    public Task<string> GetDataDomain_Exceptions();
    public Task<string> GetDataDomain_Enums();
    public Task<string> GetDataDomain_Interfaces();

    public Task<string> GetDataApplication_DTOs();
    public Task<string> GetDataApplication_Usecases();
    public Task<string> GetDataApplication_Interfaces();

    public Task<string> GetDataInfraStructure_Data();
    public Task<string> GetDataInfraStructure_Services();

    public Task<string> GetDataTests_Domain();
    public Task<string> GetDataTests_Application();

    public Task<bool> CreateDataDomain();
    public Task<bool> CreateDataApplication();
    public Task<bool> CreateDataTestes();
    public Task<bool> CreateDataInfrastructure();
}
