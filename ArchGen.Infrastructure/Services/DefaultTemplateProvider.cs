using ArchGen.Domain.Entities;
using ArchGen.Domain.Enums;
using ArchGen.Domain.Interfaces;

namespace ArchGen.InfraStructure.Services;

public class DefaultTemplateProvider : IDefaultTemplateProvider
{
    public TemplateDefinition GetDefaultTemplate()
    {
        var template = new TemplateDefinition(
            name: "Default",
            description: "Template padrão de Clean Architecture em C# (.NET 8.0) com Domain, Application, InfraStructure, Apresentação e Tests.",
            isDefault: true
        );

        // ==========================================
        // 1. Solution Files
        // ==========================================
        template.Files.Add(new TemplateFileDefinition(
            relativePath: "{{PROJECT_NAME}}.sln",
            content: GetSolutionTemplate(),
            layer: ProjectLayerEnum.Solution.ToString(),
            isForApi: true,
            isForConsole: true
        ));

        // ==========================================
        // 2. Domain Layer
        // ==========================================
        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.Domain/{{PROJECT_NAME}}.Domain.csproj",
            content: GetDomainCsprojTemplate(),
            layer: ProjectLayerEnum.Domain.ToString(),
            isForApi: true,
            isForConsole: true
        ));

        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.Domain/Common/BaseEntity.cs",
            content: GetDomainBaseEntityTemplate(),
            layer: ProjectLayerEnum.Domain.ToString(),
            isForApi: true,
            isForConsole: true
        ));

        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.Domain/Enums/UserStatus.cs",
            content: GetDomainUserStatusTemplate(),
            layer: ProjectLayerEnum.Domain.ToString(),
            isForApi: true,
            isForConsole: true
        ));

        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.Domain/Entities/User.cs",
            content: GetDomainUserTemplate(),
            layer: ProjectLayerEnum.Domain.ToString(),
            isForApi: true,
            isForConsole: true
        ));

        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.Domain/Interfaces/IUserRepository.cs",
            content: GetDomainIUserRepositoryTemplate(),
            layer: ProjectLayerEnum.Domain.ToString(),
            isForApi: true,
            isForConsole: true
        ));

        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.Domain/Exceptions/DomainException.cs",
            content: GetDomainExceptionTemplate(),
            layer: ProjectLayerEnum.Domain.ToString(),
            isForApi: true,
            isForConsole: true
        ));

        // ==========================================
        // 3. Application Layer
        // ==========================================
        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.Application/{{PROJECT_NAME}}.Application.csproj",
            content: GetApplicationCsprojTemplate(),
            layer: ProjectLayerEnum.Application.ToString(),
            isForApi: true,
            isForConsole: true
        ));

        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.Application/DTOs/UserDtos.cs",
            content: GetApplicationUserDtosTemplate(),
            layer: ProjectLayerEnum.Application.ToString(),
            isForApi: true,
            isForConsole: true
        ));

        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.Application/Interfaces/IUserUseCase.cs",
            content: GetApplicationIUserUseCaseTemplate(),
            layer: ProjectLayerEnum.Application.ToString(),
            isForApi: true,
            isForConsole: true
        ));

        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.Application/UseCases/UserUseCase.cs",
            content: GetApplicationUserUseCaseTemplate(),
            layer: ProjectLayerEnum.Application.ToString(),
            isForApi: true,
            isForConsole: true
        ));

        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.Application/DependencyInjection.cs",
            content: GetApplicationDependencyInjectionTemplate(),
            layer: ProjectLayerEnum.Application.ToString(),
            isForApi: true,
            isForConsole: true
        ));

        // ==========================================
        // 4. InfraStructure Layer
        // ==========================================
        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.InfraStructure/{{PROJECT_NAME}}.InfraStructure.csproj",
            content: GetInfrastructureCsprojTemplate(),
            layer: ProjectLayerEnum.InfraStructure.ToString(),
            isForApi: true,
            isForConsole: true
        ));

        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.InfraStructure/Data/AppDbContext.cs",
            content: GetInfrastructureAppDbContextTemplate(),
            layer: ProjectLayerEnum.InfraStructure.ToString(),
            isForApi: true,
            isForConsole: true
        ));

        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.InfraStructure/Repositories/UserRepository.cs",
            content: GetInfrastructureUserRepositoryTemplate(),
            layer: ProjectLayerEnum.InfraStructure.ToString(),
            isForApi: true,
            isForConsole: true
        ));

        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.InfraStructure/DependencyInjection.cs",
            content: GetInfrastructureDependencyInjectionTemplate(),
            layer: ProjectLayerEnum.InfraStructure.ToString(),
            isForApi: true,
            isForConsole: true
        ));

        // ==========================================
        // 5. API Layer (API project mode)
        // ==========================================
        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.API/{{PROJECT_NAME}}.API.csproj",
            content: GetApiCsprojTemplate(),
            layer: ProjectLayerEnum.API.ToString(),
            isForApi: true,
            isForConsole: false
        ));

        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.API/Program.cs",
            content: GetApiProgramTemplate(),
            layer: ProjectLayerEnum.API.ToString(),
            isForApi: true,
            isForConsole: false
        ));

        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.API/Controllers/UsersController.cs",
            content: GetApiUsersControllerTemplate(),
            layer: ProjectLayerEnum.API.ToString(),
            isForApi: true,
            isForConsole: false
        ));

        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.API/appsettings.json",
            content: GetAppSettingsTemplate(),
            layer: ProjectLayerEnum.API.ToString(),
            isForApi: true,
            isForConsole: false
        ));

        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.API/appsettings.Development.json",
            content: GetAppSettingsDevelopmentTemplate(),
            layer: ProjectLayerEnum.API.ToString(),
            isForApi: true,
            isForConsole: false
        ));

        // ==========================================
        // 6. Console Layer (CONSOLE project mode)
        // ==========================================
        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.Console/{{PROJECT_NAME}}.Console.csproj",
            content: GetConsoleCsprojTemplate(),
            layer: ProjectLayerEnum.Console.ToString(),
            isForApi: false,
            isForConsole: true
        ));

        template.Files.Add(new TemplateFileDefinition(
            relativePath: "src/{{PROJECT_NAME}}.Console/Program.cs",
            content: GetConsoleProgramTemplate(),
            layer: ProjectLayerEnum.Console.ToString(),
            isForApi: false,
            isForConsole: true
        ));

        // ==========================================
        // 7. Tests Layer
        // ==========================================
        template.Files.Add(new TemplateFileDefinition(
            relativePath: "tests/{{PROJECT_NAME}}.Tests/{{PROJECT_NAME}}.Tests.csproj",
            content: GetTestsCsprojTemplate(),
            layer: ProjectLayerEnum.Tests.ToString(),
            isForApi: true,
            isForConsole: true
        ));

        template.Files.Add(new TemplateFileDefinition(
            relativePath: "tests/{{PROJECT_NAME}}.Tests/Domain/UserTests.cs",
            content: GetTestsUserDomainTemplate(),
            layer: ProjectLayerEnum.Tests.ToString(),
            isForApi: true,
            isForConsole: true
        ));

        template.Files.Add(new TemplateFileDefinition(
            relativePath: "tests/{{PROJECT_NAME}}.Tests/Application/UserUseCaseTests.cs",
            content: GetTestsUserUseCaseTemplate(),
            layer: ProjectLayerEnum.Tests.ToString(),
            isForApi: true,
            isForConsole: true
        ));

        return template;
    }

    private static string GetSolutionTemplate()
    {
        return @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.0.31903.59
MinimumVisualStudioVersion = 10.0.40219.1
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""{{PROJECT_NAME}}.Domain"", ""src\{{PROJECT_NAME}}.Domain\{{PROJECT_NAME}}.Domain.csproj"", ""{{DOMAIN_GUID}}""
EndProject
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""{{PROJECT_NAME}}.Application"", ""src\{{PROJECT_NAME}}.Application\{{PROJECT_NAME}}.Application.csproj"", ""{{APPLICATION_GUID}}""
EndProject
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""{{PROJECT_NAME}}.InfraStructure"", ""src\{{PROJECT_NAME}}.InfraStructure\{{PROJECT_NAME}}.InfraStructure.csproj"", ""{{INFRASTRUCTURE_GUID}}""
EndProject
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""{{PROJECT_NAME}}.{{PROJECT_TYPE}}"", ""src\{{PROJECT_NAME}}.{{PROJECT_TYPE}}\{{PROJECT_NAME}}.{{PROJECT_TYPE}}.csproj"", ""{{PRESENTATION_GUID}}""
EndProject
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""{{PROJECT_NAME}}.Tests"", ""tests\{{PROJECT_NAME}}.Tests\{{PROJECT_NAME}}.Tests.csproj"", ""{{TESTS_GUID}}""
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{{DOMAIN_GUID}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{{DOMAIN_GUID}}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{{APPLICATION_GUID}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{{APPLICATION_GUID}}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{{INFRASTRUCTURE_GUID}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{{INFRASTRUCTURE_GUID}}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{{PRESENTATION_GUID}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{{PRESENTATION_GUID}}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{{TESTS_GUID}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{{TESTS_GUID}}.Debug|Any CPU.Build.0 = Debug|Any CPU
	EndGlobalSection
EndGlobal
".TrimStart();
    }

    private static string GetDomainCsprojTemplate()
    {
        return @"<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>";
    }

    private static string GetDomainBaseEntityTemplate()
    {
        return @"namespace {{PROJECT_NAME}}.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}";
    }

    private static string GetDomainUserStatusTemplate()
    {
        return @"namespace {{PROJECT_NAME}}.Domain.Enums;

public enum UserStatus
{
    Active = 1,
    Inactive = 2,
    Blocked = 3
}";
    }

    private static string GetDomainUserTemplate()
    {
        return @"using {{PROJECT_NAME}}.Domain.Common;
using {{PROJECT_NAME}}.Domain.Enums;
using {{PROJECT_NAME}}.Domain.Exceptions;

namespace {{PROJECT_NAME}}.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserStatus Status { get; set; } = UserStatus.Active;

    public User() { }

    public User(string name, string email, UserStatus status = UserStatus.Active)
    {
        Name = name;
        Email = email;
        Status = status;
        Validate();
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new DomainException(""O nome do usuário não pode ser vazio."");

        if (string.IsNullOrWhiteSpace(Email) || !Email.Contains('@'))
            throw new DomainException(""Email do usuário inválido."");
    }

    public void Update(string name, string email, UserStatus status)
    {
        Name = name;
        Email = email;
        Status = status;
        UpdatedAt = DateTime.UtcNow;
        Validate();
    }
}";
    }

    private static string GetDomainIUserRepositoryTemplate()
    {
        return @"using {{PROJECT_NAME}}.Domain.Entities;

namespace {{PROJECT_NAME}}.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<User>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(User user, CancellationToken ct = default);
    Task UpdateAsync(User user, CancellationToken ct = default);
    Task DeleteAsync(User user, CancellationToken ct = default);
}";
    }

    private static string GetDomainExceptionTemplate()
    {
        return @"namespace {{PROJECT_NAME}}.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}";
    }

    private static string GetApplicationCsprojTemplate()
    {
        return @"<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include=""..\..\src\{{PROJECT_NAME}}.Domain\{{PROJECT_NAME}}.Domain.csproj"" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include=""Microsoft.Extensions.DependencyInjection.Abstractions"" Version=""8.0.0"" />
  </ItemGroup>

</Project>";
    }

    private static string GetApplicationUserDtosTemplate()
    {
        return @"using {{PROJECT_NAME}}.Domain.Enums;

namespace {{PROJECT_NAME}}.Application.DTOs;

public record UserDto(int Id, string Name, string Email, UserStatus Status, DateTime CreatedAt, DateTime? UpdatedAt);

public record CreateUserRequest(string Name, string Email);

public record UpdateUserRequest(string Name, string Email, UserStatus Status);
";
    }

    private static string GetApplicationIUserUseCaseTemplate()
    {
        return @"using {{PROJECT_NAME}}.Application.DTOs;

namespace {{PROJECT_NAME}}.Application.Interfaces;

public interface IUserUseCase
{
    Task<UserDto> CreateAsync(CreateUserRequest request, CancellationToken ct = default);
    Task<List<UserDto>> GetAllAsync(CancellationToken ct = default);
    Task<UserDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<UserDto> UpdateAsync(int id, UpdateUserRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}";
    }

    private static string GetApplicationUserUseCaseTemplate()
    {
        return @"using {{PROJECT_NAME}}.Application.DTOs;
using {{PROJECT_NAME}}.Application.Interfaces;
using {{PROJECT_NAME}}.Domain.Entities;
using {{PROJECT_NAME}}.Domain.Exceptions;
using {{PROJECT_NAME}}.Domain.Interfaces;

namespace {{PROJECT_NAME}}.Application.UseCases;

public class UserUseCase : IUserUseCase
{
    private readonly IUserRepository _userRepository;

    public UserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var user = new User(request.Name, request.Email);
        await _userRepository.AddAsync(user, ct);

        return MapToDto(user);
    }

    public async Task<List<UserDto>> GetAllAsync(CancellationToken ct = default)
    {
        var users = await _userRepository.GetAllAsync(ct);
        return users.Select(MapToDto).ToList();
    }

    public async Task<UserDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var user = await _userRepository.GetByIdAsync(id, ct);
        return user == null ? null : MapToDto(user);
    }

    public async Task<UserDto> UpdateAsync(int id, UpdateUserRequest request, CancellationToken ct = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var user = await _userRepository.GetByIdAsync(id, ct);
        if (user == null)
            throw new DomainException($""Usuário {id} não encontrado."");

        user.Update(request.Name, request.Email, request.Status);
        await _userRepository.UpdateAsync(user, ct);

        return MapToDto(user);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var user = await _userRepository.GetByIdAsync(id, ct);
        if (user == null) return false;

        await _userRepository.DeleteAsync(user, ct);
        return true;
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto(user.Id, user.Name, user.Email, user.Status, user.CreatedAt, user.UpdatedAt);
    }
}";
    }

    private static string GetApplicationDependencyInjectionTemplate()
    {
        return @"using {{PROJECT_NAME}}.Application.Interfaces;
using {{PROJECT_NAME}}.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace {{PROJECT_NAME}}.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserUseCase, UserUseCase>();
        return services;
    }
}";
    }

    private static string GetInfrastructureCsprojTemplate()
    {
        return @"<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include=""..\..\src\{{PROJECT_NAME}}.Domain\{{PROJECT_NAME}}.Domain.csproj"" />
    <ProjectReference Include=""..\..\src\{{PROJECT_NAME}}.Application\{{PROJECT_NAME}}.Application.csproj"" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include=""Microsoft.EntityFrameworkCore.Sqlite"" Version=""8.0.0"" />
    <PackageReference Include=""Microsoft.Extensions.DependencyInjection.Abstractions"" Version=""8.0.0"" />
  </ItemGroup>

</Project>";
    }

    private static string GetInfrastructureAppDbContextTemplate()
    {
        return @"using {{PROJECT_NAME}}.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace {{PROJECT_NAME}}.InfraStructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });
    }
}";
    }

    private static string GetInfrastructureUserRepositoryTemplate()
    {
        return @"using {{PROJECT_NAME}}.Domain.Entities;
using {{PROJECT_NAME}}.Domain.Interfaces;
using {{PROJECT_NAME}}.InfraStructure.Data;
using Microsoft.EntityFrameworkCore;

namespace {{PROJECT_NAME}}.InfraStructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<List<User>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Users.ToListAsync(ct);
    }

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(User user, CancellationToken ct = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(User user, CancellationToken ct = default)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(ct);
    }
}";
    }

    private static string GetInfrastructureDependencyInjectionTemplate()
    {
        return @"using {{PROJECT_NAME}}.Domain.Interfaces;
using {{PROJECT_NAME}}.InfraStructure.Data;
using {{PROJECT_NAME}}.InfraStructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace {{PROJECT_NAME}}.InfraStructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraStructure(this IServiceCollection services, string connectionString = ""Data Source=database.db"")
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}";
    }

    private static string GetApiCsprojTemplate()
    {
        return @"<Project Sdk=""Microsoft.NET.Sdk.Web"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""Swashbuckle.AspNetCore"" Version=""6.5.0"" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include=""..\..\src\{{PROJECT_NAME}}.Domain\{{PROJECT_NAME}}.Domain.csproj"" />
    <ProjectReference Include=""..\..\src\{{PROJECT_NAME}}.Application\{{PROJECT_NAME}}.Application.csproj"" />
    <ProjectReference Include=""..\..\src\{{PROJECT_NAME}}.InfraStructure\{{PROJECT_NAME}}.InfraStructure.csproj"" />
  </ItemGroup>

</Project>";
    }

    private static string GetApiProgramTemplate()
    {
        return @"using {{PROJECT_NAME}}.Application;
using {{PROJECT_NAME}}.InfraStructure;
using {{PROJECT_NAME}}.InfraStructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Clean Architecture Layers DI
builder.Services.AddApplication();
builder.Services.AddInfraStructure(builder.Configuration.GetConnectionString(""DefaultConnection"") ?? ""Data Source=app.db"");

var app = builder.Build();

// Ensure Database created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
";
    }

    private static string GetApiUsersControllerTemplate()
    {
        return @"using {{PROJECT_NAME}}.Application.DTOs;
using {{PROJECT_NAME}}.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace {{PROJECT_NAME}}.API.Controllers;

[ApiController]
[Route(""api/[controller]"")]
public class UsersController : ControllerBase
{
    private readonly IUserUseCase _userUseCase;

    public UsersController(IUserUseCase userUseCase)
    {
        _userUseCase = userUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAll(CancellationToken ct)
    {
        var users = await _userUseCase.GetAllAsync(ct);
        return Ok(users);
    }

    [HttpGet(""{id}"")]
    public async Task<ActionResult<UserDto>> GetById(int id, CancellationToken ct)
    {
        var user = await _userUseCase.GetByIdAsync(id, ct);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserRequest request, CancellationToken ct)
    {
        var created = await _userUseCase.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut(""{id}"")]
    public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UpdateUserRequest request, CancellationToken ct)
    {
        var updated = await _userUseCase.UpdateAsync(id, request, ct);
        return Ok(updated);
    }

    [HttpDelete(""{id}"")]
    public async Task<ActionResult> Delete(int id, CancellationToken ct)
    {
        var success = await _userUseCase.DeleteAsync(id, ct);
        if (!success) return NotFound();
        return NoContent();
    }
}
";
    }

    private static string GetAppSettingsTemplate()
    {
        return @"{
  ""Logging"": {
    ""LogLevel"": {
      ""Default"": ""Information"",
      ""Microsoft.AspNetCore"": ""Warning""
    }
  },
  ""AllowedHosts"": ""*"",
  ""ConnectionStrings"": {
    ""DefaultConnection"": ""Data Source=app.db""
  }
}";
    }

    private static string GetAppSettingsDevelopmentTemplate()
    {
        return @"{
  ""Logging"": {
    ""LogLevel"": {
      ""Default"": ""Debug"",
      ""Microsoft.AspNetCore"": ""Information""
    }
  }
}";
    }

    private static string GetConsoleCsprojTemplate()
    {
        return @"<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include=""..\..\src\{{PROJECT_NAME}}.Domain\{{PROJECT_NAME}}.Domain.csproj"" />
    <ProjectReference Include=""..\..\src\{{PROJECT_NAME}}.Application\{{PROJECT_NAME}}.Application.csproj"" />
    <ProjectReference Include=""..\..\src\{{PROJECT_NAME}}.InfraStructure\{{PROJECT_NAME}}.InfraStructure.csproj"" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include=""Microsoft.Extensions.Hosting"" Version=""8.0.0"" />
    <PackageReference Include=""Microsoft.Extensions.DependencyInjection"" Version=""8.0.0"" />
  </ItemGroup>

</Project>";
    }

    private static string GetConsoleProgramTemplate()
    {
        return @"using {{PROJECT_NAME}}.Application;
using {{PROJECT_NAME}}.Application.DTOs;
using {{PROJECT_NAME}}.Application.Interfaces;
using {{PROJECT_NAME}}.InfraStructure;
using {{PROJECT_NAME}}.InfraStructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine(""🚀 Iniciando aplicação {{PROJECT_NAME}} em Clean Architecture (Console)..."");

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddApplication();
        services.AddInfraStructure(""Data Source=console_app.db"");
    })
    .Build();

// Garantir banco criado e executar demonstração
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();

    var userUseCase = scope.ServiceProvider.GetRequiredService<IUserUseCase>();

    Console.WriteLine(""Criando usuário de teste..."");
    var user = await userUseCase.CreateAsync(new CreateUserRequest(""Usuário Teste"", ""teste@example.com""));
    Console.WriteLine($""Usuário criado: #{user.Id} - {user.Name} ({user.Email})"");

    var list = await userUseCase.GetAllAsync();
    Console.WriteLine($""Total de usuários no banco: {list.Count}"");
}

Console.WriteLine(""✅ Execução concluída com sucesso!"");
";
    }

    private static string GetTestsCsprojTemplate()
    {
        return @"<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""coverlet.collector"" Version=""6.0.0"" />
    <PackageReference Include=""Microsoft.NET.Test.Sdk"" Version=""17.8.0"" />
    <PackageReference Include=""xunit"" Version=""2.5.3"" />
    <PackageReference Include=""xunit.runner.visualstudio"" Version=""2.5.3"" />
    <PackageReference Include=""Moq"" Version=""4.20.72"" />
    <PackageReference Include=""FluentAssertions"" Version=""6.12.0"" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include=""..\..\src\{{PROJECT_NAME}}.Domain\{{PROJECT_NAME}}.Domain.csproj"" />
    <ProjectReference Include=""..\..\src\{{PROJECT_NAME}}.Application\{{PROJECT_NAME}}.Application.csproj"" />
    <ProjectReference Include=""..\..\src\{{PROJECT_NAME}}.InfraStructure\{{PROJECT_NAME}}.InfraStructure.csproj"" />
  </ItemGroup>

</Project>";
    }

    private static string GetTestsUserDomainTemplate()
    {
        return @"using {{PROJECT_NAME}}.Domain.Entities;
using {{PROJECT_NAME}}.Domain.Enums;
using {{PROJECT_NAME}}.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace {{PROJECT_NAME}}.Tests.Domain;

public class UserTests
{
    [Fact]
    public void User_Should_Be_Created_With_Valid_Parameters()
    {
        var user = new User(""João Silva"", ""joao@email.com"");

        user.Name.Should().Be(""João Silva"");
        user.Email.Should().Be(""joao@email.com"");
        user.Status.Should().Be(UserStatus.Active);
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Theory]
    [InlineData("""")]
    [InlineData(""   "")]
    [InlineData(null)]
    public void User_Should_Throw_When_Name_Is_Empty(string? invalidName)
    {
        Action act = () => new User(invalidName!, ""joao@email.com"");
        act.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData("""")]
    [InlineData(""invalido"")]
    [InlineData(null)]
    public void User_Should_Throw_When_Email_Is_Invalid(string? invalidEmail)
    {
        Action act = () => new User(""João"", invalidEmail!);
        act.Should().Throw<DomainException>();
    }
}";
    }

    private static string GetTestsUserUseCaseTemplate()
    {
        return @"using {{PROJECT_NAME}}.Application.DTOs;
using {{PROJECT_NAME}}.Application.UseCases;
using {{PROJECT_NAME}}.Domain.Entities;
using {{PROJECT_NAME}}.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace {{PROJECT_NAME}}.Tests.Application;

public class UserUseCaseTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserUseCase _useCase;

    public UserUseCaseTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _useCase = new UserUseCase(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_Add_User_And_Return_Dto()
    {
        var request = new CreateUserRequest(""Maria"", ""maria@email.com"");

        _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _useCase.CreateAsync(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(""Maria"");
        result.Email.Should().Be(""maria@email.com"");
        _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Mapped_Dtos()
    {
        var users = new List<User>
        {
            new User(""User 1"", ""u1@email.com""),
            new User(""User 2"", ""u2@email.com"")
        };

        _userRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        var result = await _useCase.GetAllAsync();

        result.Should().HaveCount(2);
        result.Select(u => u.Name).Should().Contain(new[] { ""User 1"", ""User 2"" });
    }
}";
    }
}
