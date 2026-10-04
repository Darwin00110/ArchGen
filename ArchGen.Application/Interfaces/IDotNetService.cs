namespace ArchGen.Application;

public interface IDotNetService
{
    public Task<DotnetServiceResponse> CreateClass(string NomeClasse, string path);
    public Task<DotnetServiceResponse> CreateClassLib(string NomeClasse, string path);
    public Task<DotnetServiceResponse> CreateConsole(string NomeCamada, string path);
    public Task<DotnetServiceResponse> CreateAPI(string NomeCamada, string path);
    public Task<DotnetServiceResponse> CreateTests(string NomeCamada, string path);

}
