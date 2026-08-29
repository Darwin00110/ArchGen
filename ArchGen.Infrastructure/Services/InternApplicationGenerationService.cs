using ArchGen.Application;
using ArchGen.Domain;

namespace ArchGen.Infrastructure;

public class InternApplicationGenerationService : IInternApplicationGenerationService
{
    private string PathApplicationFromInternEstructure = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure" ,"Application"));
    private string PathInternSolution = Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure");

    private readonly IDotnetService _dotnet;
    public InternApplicationGenerationService(IDotnetService dotnet)
    {
        _dotnet = dotnet;
    }
    private string NomeProjeto = string.Empty;
    public async Task SetNomeProjeto(string nomeProjeto = "")
    {
        NomeProjeto = nomeProjeto;
    }
    public async Task SetPathInternApplication(string path)
    {
        if (Path.Exists(path))
        {
            PathApplicationFromInternEstructure = Path.Combine(path, $"{NomeProjeto}.Application");
            PathInternSolution = path;
        } else
        {
            throw new ServiceException("O path Não existe");
        }
    }
    public async Task VerifyApplicationInternStructure()
    {
        var PathApplicationDTOs = Path.Combine(PathApplicationFromInternEstructure!, "DTOs");
        string PathApplicationInterface = Path.Combine(PathApplicationFromInternEstructure!, "Interface");
        string PathApplicationUsecase = Path.Combine(PathApplicationFromInternEstructure!, "UseCase");
        if (Directory.Exists(PathApplicationFromInternEstructure))
        {
            throw new ServiceException("Impossivel continuar, a Pasta(Application) ja existe.");
        }
        if(Directory.Exists(PathApplicationDTOs))
        {
            throw new ServiceException("Impossivel continuar, a Pasta(DTOs) ja existe.");
        }
        if(Directory.Exists(PathApplicationInterface))
        {
            throw new ServiceException("Impossivel continuar, a Pasta(Interface) ja existe.");
        }
        if(Directory.Exists(PathApplicationUsecase))
        {
            throw new ServiceException("Impossivel continuar, a Pasta(UseCase) ja existe.");
        }
        

        if(File.Exists(Path.Combine(PathApplicationDTOs, "CreateUserRequest.cs")))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(CreateUserRequest.cs) ja existe.");           
        }
        if(File.Exists(Path.Combine(PathApplicationDTOs, "ReadUserResponse.cs")))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(ReadUserResponse.cs) ja existe.");           
        }
        if(File.Exists(Path.Combine(PathApplicationDTOs, "UpdateUserRequest.cs")))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(UpdateUserRequest.cs) ja existe.");           
        }

        if(File.Exists(Path.Combine(PathApplicationInterface, "IUserUseCase.cs")))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(IUserUseCase.cs) ja existe.");           
        }

        if(File.Exists(Path.Combine(PathApplicationUsecase, "UserUseCase.cs")))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(UserUseCase.cs) ja existe.");           
        }
    }
    public async Task<bool> CreateInternApplicationFiles() {
        var Domain = (NomeProjeto == string.Empty) ? "Domain" : $"{NomeProjeto}.Domain";
        var Application = (NomeProjeto == string.Empty) ? "Application" : $"{NomeProjeto}.Application"; 
        if (!Directory.Exists(PathApplicationFromInternEstructure))
        {
            await _dotnet.CriarCamada_Classlib($"{NomeProjeto}.Application", PathInternSolution);
            var PathApplicationUseCases = Path.Combine(PathApplicationFromInternEstructure!, "UseCases");
            Directory.CreateDirectory(PathApplicationUseCases);
            await File.WriteAllTextAsync(Path.Combine(PathApplicationUseCases, "UserUseCase.cs"), @$"
namespace {Application};
using {Domain};
public class UserUseCase : IUserUseCase
{{
    private readonly IUserRepository _repo;
    public UserUseCase(IUserRepository repo)
    {{
        _repo = repo;
    }}
    public async Task<bool> CreateUser(CreateUserRequest request)
    {{
        var verifyUser = await _repo.VerifyUserExists_WithEmail(request.Email);
        if (verifyUser)
        {{
            throw new UseCaseException(""Usuario ja existe."");
        }}
        var DataUser = new User
        {{
            ID = Guid.NewGuid(),
            Email = request.Email,
            Nome = request.Nome,
            Telefone = request.Telefone ?? throw new UseCaseException(""Telefone é obrigatório""),
            Senha = request.Senha
        }};
        DataUser.Validate_Nome();
        DataUser.Validate_Telefone();
        DataUser.Validate_Email();
        var result = await _repo.CreateUser(DataUser);
        return result;
    }}
    public async Task<ReadUserResponse> ReadUser(Guid ID)
    {{
        var verifyUser = await _repo.VerifyUserExists_WithID(ID);
        if (!verifyUser)
        {{
            throw new UseCaseException(""Usuario não existe."");
        }}
        var GetData = await _repo.GetDataUser_WithID(ID);
        return new ReadUserResponse
        {{
          Email = GetData.Email,
          Nome = GetData.Nome,
          Telefone = GetData.Telefone  
        }};
    }}
    public async Task<bool> UpdateUser(Guid ID, UpdateUserRequest request)
    {{
        var verifyUser = await _repo.VerifyUserExists_WithID(ID);
        if (!verifyUser)
        {{
            throw new UseCaseException(""Usuario não existe."");
        }}
        var DataUser = new User
        {{
          Email = request.Email,
          Nome = request.Nome,
          Telefone = request.Telefone,
          Senha = request.Senha  
        }};
        DataUser.Validate_Email();
        DataUser.Validate_Nome();
        DataUser.Validate_Telefone();
        var result = await _repo.UpdateUser(DataUser);
        return result;
    }}
    public async Task<bool> DeleteUser(Guid ID)
    {{
        var verifyUser = await _repo.VerifyUserExists_WithID(ID);
        if (!verifyUser)
        {{
            throw new UseCaseException(""Usuario não existe."");
        }}
        var result = await _repo.DeleteUser(ID);
        return result;
    }}
    public async Task<string> Exec()
    {{
        return ""Ta funcionando."";
    }}
    
}}
");
    var PathInterfaces = Path.Combine(PathApplicationFromInternEstructure!, "Interfaces");
    if(!Directory.Exists(PathInterfaces))
    {
        Directory.CreateDirectory(PathInterfaces);
        await File.WriteAllTextAsync(Path.Combine(PathInterfaces, "IUserUseCase.cs"), @$"
namespace {Application};

public interface IUserUseCase
{{
    public Task<bool> CreateUser(CreateUserRequest request);
    public Task<ReadUserResponse> ReadUser(Guid ID);
    public Task<bool> UpdateUser(Guid ID, UpdateUserRequest request);
    public Task<bool> DeleteUser(Guid ID);
    public Task<string> Exec();
}}");
    }
    var PathDTOs = Path.Combine(PathApplicationFromInternEstructure!, "DTOs");
    if(!Directory.Exists(PathDTOs))
            {
                Directory.CreateDirectory(PathDTOs);
                await File.WriteAllTextAsync(Path.Combine(PathDTOs, "CreateUserRequest.cs"), $@"
using System.ComponentModel.DataAnnotations;

namespace {Application};

public class CreateUserRequest
{{
    [Required(ErrorMessage = ""O nome é obrigatório."")]
    public required string Nome {{ get; set; }}
    [Required(ErrorMessage = ""O email é obrigatório."")]
    [EmailAddress(ErrorMessage = ""O email fornecido não é válido."")]
    public required string Email {{ get; set; }}
    public string? Telefone {{ get; set; }}
    [Required(ErrorMessage = ""A senha é obrigatória."")]
    public required string Senha {{get; set;}}
}}");
        await File.WriteAllTextAsync(Path.Combine(PathDTOs, "ReadUserResponse.cs"), $@"
namespace {Application};

public class ReadUserResponse
{{
    public required string Nome {{ get; set; }}
    public required string Email {{ get; set; }}
    public required string Telefone {{ get; set; }}
}}");
        await File.WriteAllTextAsync(Path.Combine(PathDTOs, "UpdateUserRequest.cs"), @$"
using System.ComponentModel.DataAnnotations;

namespace {Application};

public class UpdateUserRequest
{{
    public Guid? ID {{get; set;}}
    [Required(ErrorMessage = ""O email é obrigatório."")]
    [EmailAddress(ErrorMessage = ""O email fornecido não é válido."")]
    public required string Email {{ get; set; }}
    [Required(ErrorMessage = ""A senha é obrigatória."")]
    public required string Senha {{get; set;}}
    public required string Telefone {{get; set;}}
    public required string Nome {{get; set;}}
}}");   
            }
        }
        if(File.Exists(Path.Combine(PathApplicationFromInternEstructure, "Class1.cs"))){
            File.Delete(Path.Combine(PathApplicationFromInternEstructure, "Class1.cs"));
        }
        await _dotnet.ReferenceProject_in_the_Solution($"{NomeProjeto}.Application", $"{NomeProjeto}.Domain", PathInternSolution);
        return true;
    }
}
