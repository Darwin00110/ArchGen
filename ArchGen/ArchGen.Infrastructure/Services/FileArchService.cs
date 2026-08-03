using ArchGen.Application;
using ArchGen.Domain;

namespace ArchGen.Infrastructure;

public class FileArchService : IFileArchService
{
    private readonly string _nomeprojeto;

    public FileArchService(string NomeProjeto)
    {
        _nomeprojeto = NomeProjeto;
        Directory.CreateDirectory(PathMain);
    }

    private string PathMain => Path.Combine(Environment.CurrentDirectory, $"{_nomeprojeto}.Infrastructure", "FilesArch");

    private string PathDomain => Path.Combine(PathMain, "Domain");
    private string PathDomain_Entities => Path.Combine(PathDomain, "Entities");
    private string PathDomain_Exceptions => Path.Combine(PathDomain, "Exceptions");
    private string PathDomain_Interfaces => Path.Combine(PathDomain, "Interfaces");
    private string PathDomain_Enums => Path.Combine(PathDomain, "Enums");

    private string PathApplication => Path.Combine(PathMain, "Application");
    private string PathApplication_DTOs => Path.Combine(PathApplication, "DTOs");
    private string PathApplication_UseCases => Path.Combine(PathApplication, "UseCases");
    private string PathApplication_Interfaces => Path.Combine(PathApplication, "Interfaces");

    private string PathInfraStructure => Path.Combine(PathMain, "Infrastructure");
    private string PathInfraStructure_Data => Path.Combine(PathInfraStructure, "Data");
    private string PathInfraStructure_Repository => Path.Combine(PathInfraStructure, "Repository");
    private string PathInfraStructure_Services => Path.Combine(PathInfraStructure, "Services");

    private string PathTests => Path.Combine(PathMain, "Tests");
    private string PathTests_Domain => Path.Combine(PathTests, "Domain");
    private string PathTests_Application => Path.Combine(PathTests, "Application");

    private string PathFileDomain_User => Path.Combine(PathDomain_Entities, "User.cs");
    private string PathFileDomain_DomainException => Path.Combine(PathDomain_Exceptions, "DomainException.cs");
    private string PathFileDomain_ServiceException => Path.Combine(PathDomain_Exceptions, "ServiceException.cs");
    private string PathFileDomain_UseCaseException => Path.Combine(PathDomain_Exceptions, "UseCaseException.cs");
    private string PathFileDomain_InfraStructureException => Path.Combine(PathDomain_Exceptions, "InfraStructureException.cs");
    private string PathFileDomain_OptionsStatusUsuario => Path.Combine(PathDomain_Enums, "OptionsStatusUsuario.cs");
    private string PathFileDomain_Interfaces => Path.Combine(PathDomain_Interfaces, "Interfaces.cs");

    private string PathFileApplication_CreateUserRequest => Path.Combine(PathApplication_DTOs, "CreateUserRequest.cs");
    private string PathFileApplication_ReadUserResponse => Path.Combine(PathApplication_DTOs, "ReadUserResponse.cs");
    private string PathFileApplication_IUserUseCase => Path.Combine(PathApplication_Interfaces, "IUserUseCase.cs");
    private string PathFileApplication_UserUseCase => Path.Combine(PathApplication_UseCases, "UserUseCase.cs");

    private string PathFileInfrastructure_AppDbContext => Path.Combine(PathInfraStructure_Data, "AppDbContext.cs");
    private string PathFileInfrastructure_UserRepository => Path.Combine(PathInfraStructure_Repository, "UserRepository.cs");
    private string PathFileInfrastructure_FileService => Path.Combine(PathInfraStructure_Services, "FileService.cs");
    private string PathFileInfrastructure_TerminalService => Path.Combine(PathInfraStructure_Services, "TerminalService.cs");

    private string PathFileTests_UserTests => Path.Combine(PathTests_Domain, "UserTests.cs");
    private string PathFileTests_UserUseCaseTests => Path.Combine(PathTests_Application, "UserUseCaseTests.cs");

    private static async Task<string> ReadOrDefaultAsync(string path, Func<string> fallback)
    {
        if (File.Exists(path))
        {
            return await File.ReadAllTextAsync(path);
        }

        return fallback();
    }

    private static async Task WriteFileAsync(string path, string content)
    {
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.WriteAllTextAsync(path, content);
    }

    private string DomainUserTemplate() => $@"using {_nomeprojeto}.Domain.Exceptions;

namespace {_nomeprojeto}.Domain.Entities;

public class User
{{
    public Guid ID {{ get; set; }}
    public string Nome {{ get; set; }} = string.Empty;
    public string Email {{ get; set; }} = string.Empty;
    public string Telefone {{ get; set; }} = string.Empty;

    public void ValidateNome()
    {{
        if (string.IsNullOrWhiteSpace(Nome))
        {{
            throw new DomainException(""Nome não pode estar vazio, campo obrigatorio."");
        }}
    }}

    public void ValidateEmail()
    {{
        if (string.IsNullOrWhiteSpace(Email))
        {{
            throw new DomainException(""Email não pode estar vazio, campo obrigatorio."");
        }}

        if (!Email.Contains(""@gmail.com"", StringComparison.OrdinalIgnoreCase))
        {{
            throw new DomainException(""Formato Invalido do Email, ex: (exemplo@gmail.com)"");
        }}
    }}

    public void ValidateTelefone()
    {{
        if (string.IsNullOrWhiteSpace(Telefone))
        {{
            throw new DomainException(""Telefone não pode estar vazio, campo obrigatorio."");
        }}
    }}
}}
";

    private string DomainExceptionTemplate(string exceptionName) => $@"namespace {_nomeprojeto}.Domain.Exceptions;

public class {exceptionName} : Exception
{{
    public {exceptionName}(string message) : base(message)
    {{
    }}
}}
";

    private string DomainEnumTemplate() => $@"namespace {_nomeprojeto}.Domain.Enums;

public enum OptionsStatusUsuario
{{
    Ativo,
    Inativo,
    Bloqueado
}}
";

    private string DomainInterfacesTemplate() => $@"namespace {_nomeprojeto}.Domain.Interfaces;

public interface IDomainMarker
{{
}}
";

    private string ApplicationCreateUserRequestTemplate() => $@"using System.ComponentModel.DataAnnotations;

namespace {_nomeprojeto}.Application.DTOs;

public sealed class CreateUserRequest
{{
    [Required(ErrorMessage = ""Nome do usuário é obrigatório."")]
    public required string Nome {{ get; set; }}

    [Required(ErrorMessage = ""Email do usuário é obrigatório."")]
    public required string Email {{ get; set; }}

    [Required(ErrorMessage = ""Telefone do usuário é obrigatório."")]
    public required string Telefone {{ get; set; }}
}}
";

    private string ApplicationReadUserResponseTemplate() => $@"namespace {_nomeprojeto}.Application.DTOs;

public sealed class ReadUserResponse
{{
    public Guid ID {{ get; set; }}
    public string Nome {{ get; set; }} = string.Empty;
    public string Email {{ get; set; }} = string.Empty;
    public string Telefone {{ get; set; }} = string.Empty;
}}
";

    private string ApplicationIUserUseCaseTemplate() => $@"using {_nomeprojeto}.Application.DTOs;

namespace {_nomeprojeto}.Application.Interfaces;

public interface IUserUseCase
{{
    Task<bool> CreateAsync(CreateUserRequest request);
}}
";

    private string ApplicationUserUseCaseTemplate() => $@"using {_nomeprojeto}.Application.DTOs;
using {_nomeprojeto}.Application.Interfaces;

namespace {_nomeprojeto}.Application.UseCases;

public class UserUseCase : IUserUseCase
{{
    public Task<bool> CreateAsync(CreateUserRequest request)
    {{
        return Task.FromResult(
            !string.IsNullOrWhiteSpace(request.Nome) &&
            !string.IsNullOrWhiteSpace(request.Email) &&
            !string.IsNullOrWhiteSpace(request.Telefone));
    }}
}}
";

    private string InfrastructureAppDbContextTemplate() => $@"using Microsoft.EntityFrameworkCore;
using {_nomeprojeto}.Domain.Entities;

namespace {_nomeprojeto}.Infrastructure.Data;

public class AppDbContext : DbContext
{{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {{
    }}

    public DbSet<User> Users => Set<User>();
}}
";

    private string InfrastructureUserRepositoryTemplate() => $@"using {_nomeprojeto}.Domain.Entities;

namespace {_nomeprojeto}.Infrastructure.Repository;

public class UserRepository
{{
    private readonly List<User> _users = new();

    public Task AddAsync(User user)
    {{
        _users.Add(user);
        return Task.CompletedTask;
    }}
}}
";

    private string InfrastructureFileServiceTemplate() => $@"using System.Security.Cryptography;

namespace {_nomeprojeto}.Infrastructure.Services;

public class FileService
{{
    public async Task<string> ObterHashAsync(string path)
    {{
        using var sha256 = SHA256.Create();
        await using var stream = File.OpenRead(path);
        var hash = await sha256.ComputeHashAsync(stream);
        return Convert.ToHexString(hash);
    }}
}}
";

    private string InfrastructureTerminalServiceTemplate() => $@"using System.Diagnostics;

namespace {_nomeprojeto}.Infrastructure.Services;

public class TerminalService
{{
    public string Executar(string comando, string workingDirectory)
    {{
        var psi = new ProcessStartInfo
        {{
            FileName = ""cmd.exe"",
            Arguments = $""/c {{comando}}"",
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        }};

        using var process = Process.Start(psi);
        if (process is null)
        {{
            throw new InvalidOperationException(""Não foi possível iniciar o processo."");
        }}

        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        return string.IsNullOrWhiteSpace(error) ? output : error;
    }}
}}
";

    private string TestsUserTemplate() => $@"using {_nomeprojeto}.Domain.Entities;

namespace {_nomeprojeto}.Tests.Domain;

public class UserTests
{{
    [Fact]
    public void User_Deve_Permitir_Criacao()
    {{
        var user = new User
        {{
            Nome = ""Teste"",
            Email = ""teste@gmail.com"",
            Telefone = ""11999999999""
        }};

        user.ValidateNome();
        user.ValidateEmail();
        user.ValidateTelefone();

        Assert.Equal(""Teste"", user.Nome);
    }}
}}
";

    private string TestsUserUseCaseTemplate() => $@"using {_nomeprojeto}.Application.DTOs;
using {_nomeprojeto}.Application.UseCases;

namespace {_nomeprojeto}.Tests.Application;

public class UserUseCaseTests
{{
    [Fact]
    public async Task CreateAsync_Deve_Retornar_True_Quando_Dados_Sao_Validos()
    {{
        var useCase = new UserUseCase();

        var result = await useCase.CreateAsync(new CreateUserRequest
        {{
            Nome = ""Teste"",
            Email = ""teste@gmail.com"",
            Telefone = ""11999999999""
        }});

        Assert.True(result);
    }}
}}
";

    public async Task<string> GetDataDomain_Entities()
    {
        return await ReadOrDefaultAsync(PathFileDomain_User, DomainUserTemplate);
    }

    public async Task<string> GetDataDomain_Exceptions()
    {
        return await ReadOrDefaultAsync(PathFileDomain_DomainException, () => DomainExceptionTemplate("DomainException"));
    }

    public async Task<string> GetDataDomain_Enums()
    {
        return await ReadOrDefaultAsync(PathFileDomain_OptionsStatusUsuario, DomainEnumTemplate);
    }

    public async Task<string> GetDataDomain_Interfaces()
    {
        return await ReadOrDefaultAsync(PathFileDomain_Interfaces, DomainInterfacesTemplate);
    }

    public async Task<string> GetDataApplication_DTOs()
    {
        return await ReadOrDefaultAsync(PathFileApplication_CreateUserRequest, ApplicationCreateUserRequestTemplate);
    }

    public async Task<string> GetDataApplication_Usecases()
    {
        return await ReadOrDefaultAsync(PathFileApplication_UserUseCase, ApplicationUserUseCaseTemplate);
    }

    public async Task<string> GetDataApplication_Interfaces()
    {
        return await ReadOrDefaultAsync(PathFileApplication_IUserUseCase, ApplicationIUserUseCaseTemplate);
    }

    public async Task<string> GetDataInfraStructure_Data()
    {
        return await ReadOrDefaultAsync(PathFileInfrastructure_AppDbContext, InfrastructureAppDbContextTemplate);
    }

    public async Task<string> GetDataInfraStructure_Services()
    {
        return await ReadOrDefaultAsync(PathFileInfrastructure_FileService, InfrastructureFileServiceTemplate);
    }

    public async Task<string> GetDataTests_Domain()
    {
        return await ReadOrDefaultAsync(PathFileTests_UserTests, TestsUserTemplate);
    }

    public async Task<string> GetDataTests_Application()
    {
        return await ReadOrDefaultAsync(PathFileTests_UserUseCaseTests, TestsUserUseCaseTemplate);
    }

    public async Task<bool> CreateDataDomain()
    {
        Directory.CreateDirectory(PathDomain_Entities);
        Directory.CreateDirectory(PathDomain_Exceptions);
        Directory.CreateDirectory(PathDomain_Interfaces);
        Directory.CreateDirectory(PathDomain_Enums);

        await WriteFileAsync(PathFileDomain_User, DomainUserTemplate());
        await WriteFileAsync(PathFileDomain_DomainException, DomainExceptionTemplate("DomainException"));
        await WriteFileAsync(PathFileDomain_ServiceException, DomainExceptionTemplate("ServiceException"));
        await WriteFileAsync(PathFileDomain_UseCaseException, DomainExceptionTemplate("UseCaseException"));
        await WriteFileAsync(PathFileDomain_InfraStructureException, DomainExceptionTemplate("InfraStructureException"));
        await WriteFileAsync(PathFileDomain_OptionsStatusUsuario, DomainEnumTemplate());
        await WriteFileAsync(PathFileDomain_Interfaces, DomainInterfacesTemplate());

        return true;
    }

    public async Task<bool> CreateDataApplication()
    {
        Directory.CreateDirectory(PathApplication_DTOs);
        Directory.CreateDirectory(PathApplication_Interfaces);
        Directory.CreateDirectory(PathApplication_UseCases);

        await WriteFileAsync(PathFileApplication_CreateUserRequest, ApplicationCreateUserRequestTemplate());
        await WriteFileAsync(PathFileApplication_ReadUserResponse, ApplicationReadUserResponseTemplate());
        await WriteFileAsync(PathFileApplication_IUserUseCase, ApplicationIUserUseCaseTemplate());
        await WriteFileAsync(PathFileApplication_UserUseCase, ApplicationUserUseCaseTemplate());

        return true;
    }

    public async Task<bool> CreateDataTestes()
    {
        Directory.CreateDirectory(PathTests_Domain);
        Directory.CreateDirectory(PathTests_Application);

        await WriteFileAsync(PathFileTests_UserTests, TestsUserTemplate());
        await WriteFileAsync(PathFileTests_UserUseCaseTests, TestsUserUseCaseTemplate());

        return true;
    }

    public async Task<bool> CreateDataInfrastructure()
    {
        Directory.CreateDirectory(PathInfraStructure_Data);
        Directory.CreateDirectory(PathInfraStructure_Repository);
        Directory.CreateDirectory(PathInfraStructure_Services);

        await WriteFileAsync(PathFileInfrastructure_AppDbContext, InfrastructureAppDbContextTemplate());
        await WriteFileAsync(PathFileInfrastructure_UserRepository, InfrastructureUserRepositoryTemplate());
        await WriteFileAsync(PathFileInfrastructure_FileService, InfrastructureFileServiceTemplate());
        await WriteFileAsync(PathFileInfrastructure_TerminalService, InfrastructureTerminalServiceTemplate());

        return true;
    }
}
