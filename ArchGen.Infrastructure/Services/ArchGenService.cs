using System.Diagnostics;
using System.Reflection;
using ArchGen.Application;
using ArchGen.Domain;

namespace ArchGen.Infrastructure;

public class ArchGenService : IArchGenService
{
    private string? NomeDoProjeto;
    private string? TipoDoProjeto;
    private string? PathDomain;
    private string? PathApplication;
    private string? PathInfrastructure;
    private string? PathTests;

    private string? PathDomainFromInternEstructure = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure" ,"Domain"));
    private string? PathApplicationFromInternEstructure = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure" ,"Application"));
    private string? PathInfrastructureFromInternEstructure = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure" ,"InfraStructure"));
    private string? PathTestsFromInternEstructure = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure" ,"Tests"));
    private string? PathAPIFromInternEstructure = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure" ,"API"));
    private string? PathConsoleFromInternEstructure = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure" ,"Console"));
    
    private string PathInternSolution = Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure");
    private readonly IArchGenRepository _repo;
    public ArchGenService(IArchGenRepository repo)
    {
        _repo = repo;
    }
    public void SetConfiguracaoDoProjeto(string nomeDoProjeto, string TipoProjeto)
    {
        NomeDoProjeto = nomeDoProjeto;
        TipoDoProjeto = TipoProjeto;
        TipoDoProjeto = TipoDoProjeto.ToUpper();
    }
    public async Task CreateInternEstructure()
    {
        if(!Directory.Exists(PathDomainFromInternEstructure))
        {
            Console.WriteLine($"Criando estrutura de domínio em: {PathDomainFromInternEstructure}");
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
    public async Task VerifyApplicationFiles() 
    {
        try
        {
            var PathApplicationFromDb = await _repo.GetPathApplication();
            PathApplication = PathApplicationFromDb;
        } catch (Exception ex)
        {
            if(ex.Message.Equals("Nenhum caminho de aplicação encontrado."))
            {
                PathApplication = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Application"));
            }
            throw new Exception($"Erro ao verificar os arquivos de aplicação: {ex.Message}");
        }
    }
    public async Task VerifyInfrastructureFiles() 
    {
        try
        {
            var PathInfrastructureFromDb = await _repo.GetPathInfrastructure();
            PathInfrastructure = PathInfrastructureFromDb;
        } catch (Exception ex)
        {
            if(ex.Message.Equals("Nenhum caminho de infraestrutura encontrado."))
            {
                PathInfrastructure = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure"));
            }
            throw new Exception($"Erro ao verificar os arquivos de infraestrutura: {ex.Message}");
        }
    }
    public async Task VerifyTestsFiles() 
    {
        try
        {
            var PathTestsFromDb = await _repo.GetPathTests();
            PathTests = PathTestsFromDb;
        } catch (Exception ex)
        {
            if(ex.Message.Equals("Nenhum caminho de testes encontrado."))
            {
                PathTests = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Tests"));
            }
            throw new Exception($"Erro ao verificar os arquivos de testes: {ex.Message}");
        }
    }
    public async Task VerifyInternDomainFiles() {
        try
        {
            if (!Directory.Exists(PathDomainFromInternEstructure))
            {
                Directory.CreateDirectory(PathDomainFromInternEstructure!);
            }   
            if(!Directory.Exists(Path.Combine(PathDomainFromInternEstructure!, "Entities")))
            {
                string PathDomainEntities = Path.Combine(PathDomainFromInternEstructure!, "Entities");
                Directory.CreateDirectory(Path.Combine(PathDomainEntities));
                await File.WriteAllTextAsync(Path.Combine(PathDomainEntities, "User.cs"), @$"
namespace Domain;

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
            throw new DomainException('O nome não pode ser nulo ou vazio.');
        }}
    }}
    public void Validate_Telefone ()
    {{
        if(string.IsNullOrEmpty(Telefone))
        {{
            throw new DomainException('O telefone não pode ser nulo ou vazio.');
        }}
    }}
    public void Validate_Email ()
    {{
        if(string.IsNullOrEmpty(Email))
        {{
            throw new DomainException('O email não pode ser nulo ou vazio.');
        }}
        if (!Email.Contains('@gmail.com'))
        {{
            throw new DomainException('Formato invalido de email, ex: (exemplo@gmail.com)');
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
namespace Domain;

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
namespace Domain;

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
namespace Domain;

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
namespace Domain;

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
namespace Domain;

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
            throw new ServiceException($"Erro ao verificar os arquivos de domínio internos: {ex.Message}");
        }
    }
    public async Task VerifyInternApplicationFiles() {
        if (!Directory.Exists(PathApplicationFromInternEstructure))
        {
            Directory.CreateDirectory(PathApplicationFromInternEstructure!);
            var PathApplicationUseCases = Path.Combine(PathApplicationFromInternEstructure!, "UseCases");
            Directory.CreateDirectory(PathApplicationUseCases);
            await File.WriteAllTextAsync(Path.Combine(PathApplicationUseCases, "UserUseCase.cs"), @$"
namespace Application;
using Domain;
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
            throw new UseCaseException('Usuario ja existe.');
        }}
        var DataUser = new User
        {{
            ID = Guid.NewGuid(),
            Email = request.Email,
            Nome = request.Nome,
            Telefone = request.Telefone ?? throw new UseCaseException('Telefone é obrigatório'),
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
            throw new UseCaseException('Usuario não existe.');
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
            throw new UseCaseException('Usuario não existe.');
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
            throw new UseCaseException('Usuario não existe.');
        }}
        var result = await _repo.DeleteUser(ID);
        return result;
    }}
}}");
    var PathInterfaces = Path.Combine(PathApplicationFromInternEstructure!, "Interfaces");
    if(!Directory.Exists(PathInterfaces))
    {
        Directory.CreateDirectory(PathInterfaces);
        await File.WriteAllTextAsync(Path.Combine(PathInterfaces, "IUserUseCase.cs"), @$"
namespace Application;

public interface IUserUseCase
{{
    public Task<bool> CreateUser(CreateUserRequest request);
    public Task<ReadUserResponse> ReadUser(Guid ID);
    public Task<bool> UpdateUser(Guid ID, UpdateUserRequest request);
    public Task<bool> DeleteUser(Guid ID);
}}");
    }
    var PathDTOs = Path.Combine(PathApplicationFromInternEstructure!, "DTOs");
    if(!Directory.Exists(PathDTOs))
            {
                Directory.CreateDirectory(PathDTOs);
                await File.WriteAllTextAsync(Path.Combine(PathDTOs, "CreateUserRequest.cs"), $@"
using System.ComponentModel.DataAnnotations;

namespace Application;

public class CreateUserRequest
{{
    [Required(ErrorMessage = 'O nome é obrigatório.')]
    public required string Nome {{ get; set; }}
    [Required(ErrorMessage = 'O email é obrigatório.')]
    [EmailAddress(ErrorMessage = 'O email fornecido não é válido.')]
    public required string Email {{ get; set; }}
    public string? Telefone {{ get; set; }}
    [Required(ErrorMessage = 'A senha é obrigatória.')]
    public required string Senha {{get; set;}}
}}");
        await File.WriteAllTextAsync(Path.Combine(PathDTOs, "ReadUserResponse.cs"), $@"
namespace Application;

public class ReadUserResponse
{{
    public required string Nome {{ get; set; }}
    public required string Email {{ get; set; }}
    public required string Telefone {{ get; set; }}
}}");
        await File.WriteAllTextAsync(Path.Combine(PathDTOs, "UpdateUserRequest.cs"), @$"
using System.ComponentModel.DataAnnotations;

namespace Application;

public class UpdateUserRequest
{{
    public Guid? ID {{get; set;}}
    [Required(ErrorMessage = 'O email é obrigatório.')]
    [EmailAddress(ErrorMessage = 'O email fornecido não é válido.')]
    public required string Email {{ get; set; }}
    [Required(ErrorMessage = 'A senha é obrigatória.')]
    public required string Senha {{get; set;}}
    public required string Telefone {{get; set;}}
    public required string Nome {{get; set;}}
}}");   
            }
        }

    }
    public async Task VerifyInternInfrastructureFiles() {
        if (!Directory.Exists(PathInfrastructureFromInternEstructure))
        {
            var PathData = Path.Combine(PathInfrastructureFromInternEstructure!, "Data");
            var PathRepository = Path.Combine(PathInfrastructureFromInternEstructure!, "Repository");
            
            var PathFile_AppDbContext = Path.Combine(PathData, "AppDbContext.cs");
            var PathFile_UserRepository = Path.Combine(PathRepository, "UserRepository.cs");
            Directory.CreateDirectory(PathInfrastructureFromInternEstructure!);
            Directory.CreateDirectory(PathData);
            Directory.CreateDirectory(PathRepository);
            await File.WriteAllTextAsync(PathFile_AppDbContext, $@"
using Domain;
using Microsoft.EntityFrameworkCore;
namespace InfraStructure;
public class AppDbContext : DbContext       
{{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {{
    }}
    public DbSet<User> Users {{ get; set; }}
}}");
        await File.WriteAllTextAsync(PathFile_UserRepository, $@"
namespace InfraStructure;
using Domain;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {{
        _context = context;
    }}
    public async Task<bool> CreateUser(User user)
    {{
        try
        {{   
            var query = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }} catch(Exception e)
        {{
            throw new InfraStructureException($'Error: {{e.Message}}');
        }}
    }}
    public async Task<bool> UpdateUser(User user)
    {{
        try
        {{   
            var query = await _context.Users.Where(x => x.Email == user.Email).FirstOrDefaultAsync();
            if(query == null)
            {{
                throw new InfraStructureException('Usuario não encontrado.');
            }}
            query!.Email = user.Email;
            query!.Telefone = user.Telefone;
            query!.Senha = user.Senha;
            query!.Nome = user.Nome;
            await _context.SaveChangesAsync();
            return true;
        }} catch(Exception e)
        {{
            throw new InfraStructureException($'Error: {{e.Message}}');
        }}
    }}
    public async Task<bool> DeleteUser(Guid Id)
    {{
        try
        {{   
            var query = await _context.Users.Where(x => x.ID == Id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return true;
        }} catch(Exception e)
        {{
            throw new InfraStructureException($'Error: {{e.Message}}');
        }}
    }}
    public async Task<bool> VerifyUserExists_WithEmail(string email)
    {{
        try
        {{   
            var query = await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            if(query == null)
            {{
                return false;
            }}
            return true;
        }} catch(Exception e)
        {{
            throw new InfraStructureException($'Error: {{e.Message}}');
        }}
    }}
    public async Task<User> GetDataUser_WithID(Guid ID)
    {{
        try
        {{   
            var query = await _context.Users.Where(x => x.ID == ID).FirstOrDefaultAsync();
            if(query == null)
            {{
                throw new InfraStructureException('Usuario não existe.');
            }}
            return new User
            {{
                Email = query.Email,
                Nome = query.Nome,
                Senha = query.Senha,
                Telefone = query.Telefone
            }};
        }} catch(Exception e)
        {{
            throw new InfraStructureException($'Error: {{e.Message}}');
        }}
    }}
    public async Task<bool> VerifyUserExists_WithID(Guid ID)
    {{
        try
        {{
            var query = await _context.Users.Where(x => x.ID == ID).FirstOrDefaultAsync();
            if(query == null)
            {{   
                return false;
            }}
            return true;
        }} catch(Exception e)
        {{
            throw new InfraStructureException($'Error: {{e.Message}}');
        }}
    }} 
}}");
        }
    }
    private class ProcessResponse
    {
        public string Saida {get; set;} = string.Empty;
        public string Error {get; set;} = string.Empty;
    }
    private async Task<ProcessResponse> StartProcesso(string Command, string Arguments, bool UseShell = false, string WorkingDiretory = "")
    {
        WorkingDiretory = (WorkingDiretory == string.Empty) ? Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure") : WorkingDiretory;
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = Command,
            Arguments = Arguments,
            WorkingDirectory = WorkingDiretory,
            UseShellExecute = UseShell,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        Process process = new Process
        {
            StartInfo = psi
        };
        process.Start();
        string saida = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        return new ProcessResponse
        {
            Error = error,
            Saida = saida
        };
    }
    public async Task VerifyInternTestsFiles() {
        if (!Directory.Exists(PathTestsFromInternEstructure))
        {
            await StartProcesso("dotnet", "new xunit -n Tests", false, PathTestsFromInternEstructure!);
        }
    }
    public async Task VerifySoluctionFiles()
    {
        if (!Directory.Exists(PathInternSolution))
        {
            Directory.CreateDirectory(PathInternSolution);
        }
        if (!Directory.Exists(PathDomainFromInternEstructure))
        {
            await VerifyInternDomainFiles();
        }
        if (!Directory.Exists(PathApplicationFromInternEstructure))
        {
            await VerifyInternApplicationFiles();
        }
        if (!Directory.Exists(PathInfrastructureFromInternEstructure))
        {
            await VerifyInternInfrastructureFiles();
        }
        if (!Directory.Exists(PathTestsFromInternEstructure))
        {
            await VerifyInternTestsFiles();
        }

        if(!File.Exists(Path.Combine(PathInternSolution, "Solution.sln")))
        {
           await StartProcesso("dotnet", "new sln -n Solution", false, PathInternSolution);
           await StartProcesso("dotnet", "sln add Domain", false, PathInternSolution);
           await StartProcesso("dotnet", "sln add Application", false, PathInternSolution);
           await StartProcesso("dotnet", "sln add InfraStructure", false, PathInternSolution);
           await StartProcesso("dotnet", "sln add Tests", false, PathInternSolution);
            if (TipoDoProjeto!.Equals("API"))
            {
                await StartProcesso("dotnet", "sln add API", false, PathInternSolution);
                await StartProcesso("dotnet", "add API reference Domain", false, PathInternSolution);
                await StartProcesso("dotnet", "add API reference Application", false, PathInternSolution);
                await StartProcesso("dotnet", "add API reference InfraStructure", false, PathInternSolution);
            }
            if (TipoDoProjeto!.Equals("CONSOLE"))
            {
                await StartProcesso("dotnet", "sln add CONSOLE", false, PathInternSolution);
                await StartProcesso("dotnet", "add CONSOLE reference Domain", false, PathInternSolution);
                await StartProcesso("dotnet", "add CONSOLE reference Application", false, PathInternSolution);
                await StartProcesso("dotnet", "add CONSOLE reference InfraStructure", false, PathInternSolution);
            }
            await StartProcesso("dotnet", "add Application reference Domain", false, PathInternSolution);
            await StartProcesso("dotnet", "add InfraStructure reference Domain", false, PathInternSolution);
            await StartProcesso("dotnet", "add InfraStructure reference Application", false, PathInternSolution);
            await StartProcesso("dotnet", "add Tests reference Domain", false, PathInternSolution);
            await StartProcesso("dotnet", "add Tests reference Application", false, PathInternSolution);
        }
    }
}
