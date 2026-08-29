using ArchGen.Application;
using ArchGen.Domain;

namespace ArchGen.Infrastructure;

public class InternDomainGenerationService : IInternDomainGenerationService
{
    private readonly IDotnetService _dotnet;
    private readonly IArchGenRepository _repo;
    public InternDomainGenerationService(IDotnetService dotnet, IArchGenRepository repo)
    {
        _dotnet = dotnet;
        _repo = repo;
    }
    private string? PathDomain;

    private string PathInternSolution = Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure");
    private string PathDomainFromInternEstructure = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure" ,"Domain"));
    private string NomeProjeto = string.Empty;
    public async Task SetPathInternDomain(string path)
    {
        if (Path.Exists(path))
        {
            PathDomainFromInternEstructure = Path.Combine(path, $"{NomeProjeto}.Domain");
            PathInternSolution = path;
        } else
        {
            throw new ServiceException("O path Não existe");
        }
    }
    public async Task SetNomeProjeto(string nomeProjeto = "")
    {
        NomeProjeto = nomeProjeto;
    }
    public async Task<string> SetPathDomain(string path_domain)
    {
        if (Path.Exists(path_domain))
        {
            PathDomain = path_domain;
            return "O path foi definido com sucesso";
        }
        else
        {
            return "O path fornecido não existe";
        }
    }
    public async Task VerifyDomainInternStructure()
    {
        var PathDomainExceptions = Path.Combine(PathDomainFromInternEstructure!, "Exceptions");
        string PathDomainEntities = Path.Combine(PathDomainFromInternEstructure!, "Entities");
        if (Directory.Exists(PathDomainFromInternEstructure))
        {
            throw new ServiceException("Impossivel continuar, a Pasta(Domain) ja existe.");
        }
        if(Directory.Exists(Path.Combine(PathDomainFromInternEstructure, "Entities")))
        {
            throw new ServiceException("Impossivel continuar, a Pasta(Entities) ja existe.");
        }
        if(Directory.Exists(Path.Combine(PathDomainFromInternEstructure, "Exceptions")))
        {
            throw new ServiceException("Impossivel continuar, a Pasta(Exceptions) ja existe.");
        }
        if(Directory.Exists(Path.Combine(PathDomainFromInternEstructure, "Interfaces")))
        {
            throw new ServiceException("Impossivel continuar, a Pasta(Interfaces) ja existe.");
        }
        if(Directory.Exists(Path.Combine(PathDomainFromInternEstructure, "Enums")))
        {
            throw new ServiceException("Impossivel continuar, a Pasta(Enums) ja existe.");
        }

        if(File.Exists(Path.Combine(PathDomainEntities, "User.cs")))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(User.cs) ja existe.");           
        }

        if(File.Exists(Path.Combine(PathDomainEntities, "User.cs")))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(User.cs) ja existe.");           
        }

        if(File.Exists(Path.Combine(PathDomainExceptions, "DomainException.cs")))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(DomainException.cs) ja existe.");           
        }

        if(File.Exists(Path.Combine(PathDomainExceptions, "UseCaseException.cs")))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(DomainException.cs) ja existe.");           
        }

        if(File.Exists(Path.Combine(PathDomainExceptions, "InfraStructureException.cs")))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(DomainException.cs) ja existe.");           
        }

        if(File.Exists(Path.Combine(PathDomainExceptions, "ServiceException.cs")))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(DomainException.cs) ja existe.");           
        }
        if(File.Exists(Path.Combine(PathDomainFromInternEstructure!, "Interfaces", "IUserRepository.cs")))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(IUserRepository.cs) ja existe.");           
        }
    }
    public async Task CreateStructure()
    {
        try
        {
            var Domain = (NomeProjeto == string.Empty) ? "Domain" : $"{NomeProjeto}.Domain";
            if (!Directory.Exists(PathDomainFromInternEstructure))
            {
                await _dotnet.CriarCamada_Classlib($"{NomeProjeto}.Domain", PathInternSolution);
            }
            
            if(!Directory.Exists(Path.Combine(PathDomainFromInternEstructure, "Entities")))
            {
                string PathDomainEntities = Path.Combine(PathDomainFromInternEstructure!, "Entities");
                Directory.CreateDirectory(Path.Combine(PathDomainEntities));
                await File.WriteAllTextAsync(Path.Combine(PathDomainEntities, "User.cs"), @$"
namespace {Domain};

public class User
{{
    public Guid ID {{get; set;}}
    public required string Nome {{get; set;}}
    public required string Telefone {{get; set;}}
    public required string Senha {{get; set;}}
    public required string Email {{get; set;}}
    public void Validate_Nome ()
    {{
        if(string.IsNullOrEmpty(Nome))
        {{
            throw new DomainException(""O nome não pode ser nulo ou vazio."");
        }}
    }}
    public void Validate_Telefone ()
    {{
        if(string.IsNullOrEmpty(Telefone))
        {{
            throw new DomainException(""O telefone não pode ser nulo ou vazio."");
        }}
    }}
    public void Validate_Email ()
    {{
        if(string.IsNullOrEmpty(Email))
        {{
            throw new DomainException(""O email não pode ser nulo ou vazio."");
        }}
        if (!Email.Contains(""@gmail.com""))
        {{
            throw new DomainException(""Formato invalido de email, ex: (exemplo@gmail.com)"");
        }}
    }}
}}");
            }

            if(!Directory.Exists(Path.Combine(PathDomainFromInternEstructure!, "Enums")))
            {
                Directory.CreateDirectory(Path.Combine(PathDomainFromInternEstructure!, "Enums"));
            }

            if(!Directory.Exists(Path.Combine(PathDomainFromInternEstructure!, "Exceptions")))
            {
                var PathDomainExceptions = Path.Combine(PathDomainFromInternEstructure!, "Exceptions");
                Directory.CreateDirectory(PathDomainExceptions);
                if(!File.Exists(Path.Combine(PathDomainExceptions, "DomainException.cs")))
                {
                await File.WriteAllTextAsync(Path.Combine(PathDomainExceptions, "DomainException.cs"), @$"
namespace {Domain};

public class DomainException : Exception
{{
    public DomainException(string message) : base(message)
    {{
    }}
}}
");                 
                }
                if(!File.Exists(Path.Combine(PathDomainExceptions, "ServiceException.cs")))
                {
                await File.WriteAllTextAsync(Path.Combine(PathDomainExceptions, "ServiceException.cs"), @$"
namespace {Domain};

public class ServiceException : Exception
{{
    public ServiceException(string message) : base(message)
    {{
    }}
}}
");
                }
                if(!File.Exists(Path.Combine(PathDomainExceptions, "UseCaseException.cs")))
                {
                    
        await File.WriteAllTextAsync(Path.Combine(PathDomainExceptions, "UseCaseException.cs"), @$"
namespace {Domain};

public class UseCaseException : Exception
{{
    public UseCaseException(string message) : base(message)
    {{
    }}
}}
");
                }
                if(!File.Exists(Path.Combine(PathDomainExceptions, "InfraStructureException.cs")))
                {
        await File.WriteAllTextAsync(Path.Combine(PathDomainExceptions, "InfraStructureException.cs"), @$"
namespace {Domain};

public class InfraStructureException : Exception
{{
    public InfraStructureException(string message) : base(message)
    {{
    }}
}}
");        
        }
            }

            if(!Directory.Exists(Path.Combine(PathDomainFromInternEstructure!, "Interfaces")))
            {
                Directory.CreateDirectory(Path.Combine(PathDomainFromInternEstructure!, "Interfaces"));
                await File.WriteAllTextAsync(Path.Combine(PathDomainFromInternEstructure!, "Interfaces", "IUserRepository.cs"), $@"
namespace {Domain};

public interface IUserRepository
{{
    public Task<bool> CreateUser(User user);
    public Task<bool> DeleteUser(Guid Id);
    public Task<bool> VerifyUserExists_WithEmail(string email);
    public Task<bool> VerifyUserExists_WithID(Guid ID);
    public Task<User> GetDataUser_WithID(Guid ID);
    public Task<bool> UpdateUser(User user);
}}");
            }

        } catch(Exception ex)
        {
            throw new ServiceException($"Erro ao criar os arquivos de domínio internos: {ex.Message}");
        }
        if (File.Exists(Path.Combine(PathDomainFromInternEstructure, "Class1.cs"))){
            File.Delete(Path.Combine(PathDomainFromInternEstructure, "Class1.cs"));
        }
    }
    public async Task VerifyDomainFiles()
    {
        try
        {
            var PathDomainFromDb = await _repo.GetPathDomain();
            PathDomain = PathDomainFromDb;
        } catch (Exception ex)
        {
            if(ex.Message.Contains("Nenhum caminho de domínio encontrado."))
            {
                PathDomain = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Domain"));
            }
            else
            {
                throw new ServiceException(ex.Message);
            }
        }
    }
}
