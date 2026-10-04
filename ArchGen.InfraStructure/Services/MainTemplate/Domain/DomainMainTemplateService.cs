using ArchGen.Application;
using ArchGen.Domain;

namespace ArchGen.InfraStructure;

public class DomainMainTemplateService : IDomainMainTemplateService
{
    private readonly IDotNetService _dotnet;
    public DomainMainTemplateService(IDotNetService dotnet)
    {
        _dotnet = dotnet;
    }
    private async void VerificarPathDomain(TemplateStructure Data)
    {
        if(Path.Exists(Data.paths.PathDomain))
            throw new ServiceException("Impossivel criar a camada Domain, Camada ja existente.");
        else
            await _dotnet.CreateClassLib($"{Data.NomeProjeto}.Domain", Data.PathSolucao);
        if(Path.Exists(Data.paths.Domain.PathEntities))
            throw new ServiceException("Impossivel criar o arquivo Entities, Arquivo ja existente.");
        else
            await _dotnet.CreateClass("Entity", Data.paths.PathDomain);
        if(Path.Exists(Data.paths.Domain.PathEnums))
            throw new ServiceException("Impossivel criar o arquivo Enums, Arquivo ja existente.");
        else
            await _dotnet.CreateClass("Enum", Data.paths.PathDomain);
        if(Path.Exists(Data.paths.Domain.PathExceptions))
            throw new ServiceException("Impossivel criar o arquivo Exceptions, Arquivo ja existente.");
        else
            await _dotnet.CreateClass("Exception", Data.paths.PathDomain);
        if(Path.Exists(Data.paths.Domain.PathInterfaces))
            throw new ServiceException("Impossivel criar o arquivo Interfaces, Arquivo ja existente.");
        else
            await _dotnet.CreateClass("Interface", Data.paths.PathDomain);
    }
    private async Task<string> GetContentEntities(string path)
    {
        if (!File.Exists(path))
            throw new ServiceException("Impossivel pegar o conteudo do arquivo Entities, Arquivo não encontrado.");
        return await File.ReadAllTextAsync(Path.Combine(path));
    }
    public async Task Create(TemplateStructure Data)
    {
        VerificarPathDomain(Data);
        var contentEntities = await GetContentEntities(Data.paths.Domain.PathEntities);
        
    }

}
