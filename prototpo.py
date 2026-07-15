import os
import subprocess
import sys

args = sys.argv


class Program:
    def __init__(self, NomePrograma: str, TipoPrograma="API", PathPrograma=""):
        self.NomePrograma = NomePrograma
        self.TipoPrograma = TipoPrograma.upper()
        self.PathProjeto = PathPrograma if PathPrograma != "" else os.getcwd()

    def Main(self):
        self.Criando_Domain()
        self.Criando_Application()
        self.Criando_InfraStrutura()
        if self.TipoPrograma == "API":
            self.Criando_API()
        elif self.TipoPrograma == "CONSOLE":
            self.Criando_CONSOLE()
        self.Criando_Tests()
        self.Criando_Solucao()

    def ModificandoArquivos(self, PathArquivo: str, Content: str):
        with open(PathArquivo, 'w') as file:
            file.write(Content)
        return

    def CriandoClassesCS(self, Nome: str, PathProjeto: str, TipoArquivo="class"):
        resultado = subprocess.run(
            ["dotnet", "new", TipoArquivo, "-n", Nome],
            cwd=PathProjeto
        )
        if resultado.returncode != 0:
            print(f"Erro ao criar '{Nome}' ({TipoArquivo}) em {PathProjeto}")
        return resultado.returncode == 0

    def Criando_Domain(self):
        PathPadraoDomain = os.path.join(self.PathProjeto, f"{self.NomePrograma}.Domain")
        if os.path.exists(PathPadraoDomain):
            print("A pasta Domain ja existe")
            return

        resultado = subprocess.run(
            ["dotnet", "new", "classlib", "-n", f"{self.NomePrograma}.Domain"],
            cwd=self.PathProjeto
        )
        if resultado.returncode != 0:
            print("Erro ao criar o projeto Domain")
            return

        arquivo_padrao = os.path.join(PathPadraoDomain, "Class1.cs")
        if os.path.exists(arquivo_padrao):
            os.remove(arquivo_padrao)

        if not os.path.exists(os.path.join(PathPadraoDomain, "Entities")):
            os.mkdir(os.path.join(PathPadraoDomain, "Entities"))
            self.CriandoClassesCS("User", os.path.join(PathPadraoDomain, "Entities"))
            self.ModificandoArquivos(os.path.join(PathPadraoDomain, "Entities", "User.cs"), """
namespace SwaggerLoader.Domain;

public class User
{
    public Guid ID {get; set;}
    public string Nome {get; set; } = string.Empty;
    public string Email {get; set;} = string.Empty;
    public string Telefone {get; set;} = string.Empty;
    public void Validate_Nome()
    {
        if (string.IsNullOrEmpty(Nome))
        {
            throw new DomainException("Nome não pode estar vazio, campo obrigatorio.");
        }
    }
    public void Validate_Email()
    {
        if (string.IsNullOrEmpty(Email))
        {
            throw new DomainException("Email não pode estar vazio, campo obrigatorio.");
        }
        if (!Email.Contains("@gmail.com"))
        {
            throw new DomainException("Formato Invalido do Email, ex: (exemplo@gmail.com)");
        }
    }
    public void Validate_Telefone()
    {
        if (string.IsNullOrEmpty(Telefone))
        {
            throw new DomainException("Telefone não pode estar vazio, campo obrigatorio.");
        }
    }
}

""")
        if not os.path.exists(os.path.join(PathPadraoDomain, "Exceptions")):
            os.mkdir(os.path.join(PathPadraoDomain, "Exceptions"))
            self.CriandoClassesCS("DomainException", os.path.join(PathPadraoDomain, "Exceptions"))
            self.ModificandoArquivos(os.path.join(PathPadraoDomain, "Exceptions", "DomainException.cs"), """
namespace SwaggerLoader.Domain;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) {}
}
""")
            self.CriandoClassesCS("ApplicationException", os.path.join(PathPadraoDomain, "Exceptions"))
            self.ModificandoArquivos(os.path.join(PathPadraoDomain, "Exceptions", "ApplicationException.cs"), """
namespace SwaggerLoader.Domain;

public class ApplicationException : Exception
{
    public ApplicationException(string message) : base(message) {}
}
""")
            
            self.CriandoClassesCS("InfraStructureException", os.path.join(PathPadraoDomain, "Exceptions"))
            self.ModificandoArquivos(os.path.join(PathPadraoDomain, "Exceptions", "InfraStructureException.cs"), """
namespace SwaggerLoader.Domain;

public class InfraStructureException : Exception
{
    public InfraStructureException(string message) : base(message) {}
}
""")
        if not os.path.exists(os.path.join(PathPadraoDomain, "Interfaces")):
            os.mkdir(os.path.join(PathPadraoDomain, "Interfaces"))
            self.CriandoClassesCS("IUserRepository", os.path.join(PathPadraoDomain, "Interfaces"), "interface")
        if not os.path.exists(os.path.join(PathPadraoDomain, "Enums")):
            os.mkdir(os.path.join(PathPadraoDomain, "Enums"))
            self.CriandoClassesCS("OptionsStatusUsuario", os.path.join(PathPadraoDomain, "Enums"), "enum")

    def Criando_Solucao(self):
        caminho_sln = os.path.join(self.PathProjeto, f"{self.NomePrograma}.sln")
        if os.path.exists(caminho_sln):
            print("O arquivo de solução ja existe")
            return

        subprocess.run(["dotnet", "new", "sln", "-n", self.NomePrograma], cwd=self.PathProjeto)

        # dotnet sln add <projeto> -- a ordem certa é "sln add", não "add sln"
        subprocess.run(["dotnet", "sln", "add", f"{self.NomePrograma}.Domain"], cwd=self.PathProjeto)
        subprocess.run(["dotnet", "sln", "add", f"{self.NomePrograma}.Application"], cwd=self.PathProjeto)
        subprocess.run(["dotnet", "sln", "add", f"{self.NomePrograma}.InfraStructure"], cwd=self.PathProjeto)
        subprocess.run(["dotnet", "sln", "add", f"{self.NomePrograma}.Tests"], cwd=self.PathProjeto)

        if self.TipoPrograma == "API":
            subprocess.run(["dotnet", "sln", "add", f"{self.NomePrograma}.API"], cwd=self.PathProjeto)
            subprocess.run(
                ["dotnet", "add", f"{self.NomePrograma}.API", "reference", f"{self.NomePrograma}.Application"],
                cwd=self.PathProjeto
            )
            subprocess.run(
                ["dotnet", "add", f"{self.NomePrograma}.API", "reference", f"{self.NomePrograma}.Domain"],
                cwd=self.PathProjeto
            )
            subprocess.run(
                ["dotnet", "add", f"{self.NomePrograma}.API", "reference", f"{self.NomePrograma}.InfraStructure"],
                cwd=self.PathProjeto
            )
        elif self.TipoPrograma == "CONSOLE":
            print("em producao")

        subprocess.run(
            ["dotnet", "add", f"{self.NomePrograma}.Application", "reference", f"{self.NomePrograma}.Domain"],
            cwd=self.PathProjeto
        )

        subprocess.run(
            ["dotnet", "add", f"{self.NomePrograma}.InfraStructure", "reference", f"{self.NomePrograma}.Domain"],
            cwd=self.PathProjeto
        )
        subprocess.run(
            ["dotnet", "add", f"{self.NomePrograma}.InfraStructure", "reference", f"{self.NomePrograma}.Application"],
            cwd=self.PathProjeto
        )

        subprocess.run(
            ["dotnet", "add", f"{self.NomePrograma}.Tests", "reference", f"{self.NomePrograma}.Application"],
            cwd=self.PathProjeto
        )
        subprocess.run(
            ["dotnet", "add", f"{self.NomePrograma}.Tests", "reference", f"{self.NomePrograma}.Domain"],
            cwd=self.PathProjeto
        )

    def Criando_Application(self):
        PathPadraoApplication = os.path.join(self.PathProjeto, f"{self.NomePrograma}.Application")
        if os.path.exists(PathPadraoApplication):
            print("A pasta Application ja existe")
            return

        resultado = subprocess.run(
            ["dotnet", "new", "classlib", "-n", f"{self.NomePrograma}.Application"],
            cwd=self.PathProjeto
        )
        if resultado.returncode != 0:
            print("Erro ao criar o projeto Application")
            return

        arquivo_padrao = os.path.join(PathPadraoApplication, "Class1.cs")
        if os.path.exists(arquivo_padrao):
            os.remove(arquivo_padrao)

        if not os.path.exists(os.path.join(PathPadraoApplication, "DTOs")):
            os.mkdir(os.path.join(PathPadraoApplication, "DTOs"))
            self.CriandoClassesCS("CreateUserRequest", os.path.join(PathPadraoApplication, "DTOs"))
            self.CriandoClassesCS("UpdateUserRequest", os.path.join(PathPadraoApplication, "DTOs"))
            self.CriandoClassesCS("PatchUserRequest", os.path.join(PathPadraoApplication, "DTOs"))
        if not os.path.exists(os.path.join(PathPadraoApplication, "UseCase")):
            os.mkdir(os.path.join(PathPadraoApplication, "UseCase"))
            self.CriandoClassesCS("UserUseCase", os.path.join(PathPadraoApplication, "UseCase"))
            self.ModificandoArquivos(os.path.join(PathPadraoApplication, "UseCase", "UserUseCase.cs"), """
using SwaggerLoader.Domain;

namespace SwaggerLoader.Application;

public class UserUseCase : IUserUseCase
{
    public async Task<bool> Create(CreateUserRequest request)
    {

        return false;
    }
    public async Task<User> ReadUser(Guid id)
    {
        return new User
        {

        };
    }
    public async Task<bool> UpdateUser(Guid id, UpdateUserRequest request)
    {
        return false;
    }
    public async Task<bool> PatchUser(Guid id, PatchUserRequest request)
    {
        return false;

    }
    public async Task<bool> DeleteUser(Guid id)
    {
        return false;
    }
}""")
        if not os.path.exists(os.path.join(PathPadraoApplication, "Interfaces")):
            os.mkdir(os.path.join(PathPadraoApplication, "Interfaces"))
            self.CriandoClassesCS("IUserUseCase", os.path.join(PathPadraoApplication, "Interfaces"), "interface")
            self.ModificandoArquivos(os.path.join(PathPadraoApplication, "Interfaces", "IUserUseCase.cs"), """
using SwaggerLoader.Domain;

namespace SwaggerLoader.Application;

public interface IUserUseCase
{
    public Task<bool> Create(CreateUserRequest request);
    public Task<User> ReadUser(Guid id);
    public Task<bool> UpdateUser(Guid id, UpdateUserRequest request);
    public Task<bool> PatchUser(Guid id, PatchUserRequest request);
    public Task<bool> DeleteUser(Guid id);
}
""")

    def Criando_InfraStrutura(self):
        PathPadraoInfraStructure = os.path.join(self.PathProjeto, f"{self.NomePrograma}.InfraStructure")
        if os.path.exists(PathPadraoInfraStructure):
            print("A pasta InfraStructure ja existe")
            return

        resultado = subprocess.run(
            ["dotnet", "new", "classlib", "-n", f"{self.NomePrograma}.InfraStructure"],
            cwd=self.PathProjeto
        )
        if resultado.returncode != 0:
            print("Erro ao criar o projeto InfraStructure")
            return

        arquivo_padrao = os.path.join(PathPadraoInfraStructure, "Class1.cs")
        if os.path.exists(arquivo_padrao):
            os.remove(arquivo_padrao)

        if not os.path.exists(os.path.join(PathPadraoInfraStructure, "Data")):
            os.mkdir(os.path.join(PathPadraoInfraStructure, "Data"))
            self.CriandoClassesCS("AppDbContext", os.path.join(PathPadraoInfraStructure, "Data"))
            self.ModificandoArquivos(os.path.join(PathPadraoInfraStructure, "Data", "AppDbContext.cs"), """
using Microsoft.EntityFrameworkCore;
using SwaggerLoader.Domain;

namespace SwaggerLoader.InfraStructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    public DbSet<User> User {get; set;}
}

""")
        if not os.path.exists(os.path.join(PathPadraoInfraStructure, "Repository")):
            os.mkdir(os.path.join(PathPadraoInfraStructure, "Repository"))
            self.CriandoClassesCS("UserRepository", os.path.join(PathPadraoInfraStructure, "Repository"))
        subprocess.run(["dotnet", "add", "package", "Microsoft.EntityFrameworkCore", "--version", "8.0.0"], cwd=PathPadraoInfraStructure)
    def Criando_Tests(self):
        PathPadraoTestes = os.path.join(self.PathProjeto, f"{self.NomePrograma}.Tests")
        if os.path.exists(PathPadraoTestes):
            print("A pasta Tests ja existe")
            return

        resultado = subprocess.run(
            ["dotnet", "new", "xunit", "-n", f"{self.NomePrograma}.Tests"],
            cwd=self.PathProjeto
        )
        if resultado.returncode != 0:
            print("Erro ao criar o projeto Tests")
            return

        arquivo_padrao = os.path.join(PathPadraoTestes, "UnitTest1.cs")
        if os.path.exists(arquivo_padrao):
            os.remove(arquivo_padrao)

        if not os.path.exists(os.path.join(PathPadraoTestes, "Domain")):
            os.mkdir(os.path.join(PathPadraoTestes, "Domain"))
            self.CriandoClassesCS("UserTests", os.path.join(PathPadraoTestes, "Domain"))
        if not os.path.exists(os.path.join(PathPadraoTestes, "Application")):
            os.mkdir(os.path.join(PathPadraoTestes, "Application"))
            self.CriandoClassesCS("UserUseCaseTests", os.path.join(PathPadraoTestes, "Application"))

    def Criando_API(self):
        PathPadraoAPI = os.path.join(self.PathProjeto, f"{self.NomePrograma}.API")
        if os.path.exists(PathPadraoAPI):
            print("A pasta API ja existe")
            return

        resultado = subprocess.run(
            ["dotnet", "new", "webapi", "-n", f"{self.NomePrograma}.API"],
            cwd=self.PathProjeto
        )
        if resultado.returncode != 0:
            print("Erro ao criar o projeto API")
            return
        
        self.ModificandoArquivos(os.path.join(PathPadraoAPI, "Program.cs"), """
using Microsoft.EntityFrameworkCore;
using SwaggerLoader.InfraStructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var NomeProjeto = Environment.CurrentDirectory;
Console.WriteLine(NomeProjeto);

var PathBanco = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "SwaggerLoader.InfraStructure", "Data", "database.db"));
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite($"Data Source={PathBanco}");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.Run();
""")
        subprocess.run(["dotnet", "add", "package", "Microsoft.EntityFrameworkCore", "--version", "8.0.0"], cwd=PathPadraoAPI)
        subprocess.run(["dotnet", "add", "package", "Microsoft.EntityFrameworkCore.Sqlite", "--version", "8.0.0"], cwd=PathPadraoAPI)
        subprocess.run(["dotnet", "add", "package", "Microsoft.AspNetCore.Mvc"], cwd=PathPadraoAPI)
        if not os.path.exists(os.path.join(PathPadraoAPI, "Controller")):
            os.mkdir(os.path.join(PathPadraoAPI, "Controller"))
            self.CriandoClassesCS("UserController", os.path.join(PathPadraoAPI, "Controller"))
            self.ModificandoArquivos(os.path.join(PathPadraoAPI, "Program.cs"), """
using Microsoft.AspNetCore.Mvc;
using SwaggerLoader.Application;

namespace SwaggerLoader.API;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserUseCase _usecase;
    public UserController(IUserUseCase usecase)
    {
        _usecase = usecase;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {
        try
        {
            var result = await _usecase.Create(request);
            return Ok(new
            {
                Sucess = result
            });
        } catch(Exception e)
        {
            return BadRequest(new {Error = e.Message});
        }
    }
    [HttpGet("me")]
    public async Task<IActionResult> Read(Guid id)
    {
        try
        {
            var result = await _usecase.ReadUser(id);
            return Ok(new
            {
                Data = result
            });
        } catch(Exception e)
        {
            return BadRequest(new {Error = e.Message});
        }        
    }
    [HttpPut("me")]
    public async Task<IActionResult> Update(Guid id, UpdateUserRequest request)
    {
        try
        {
            var result = await _usecase.UpdateUser(id, request);
            return Ok(new
            {
                Sucess = result
            });
        } catch(Exception e)
        {
            return BadRequest(new {Error = e.Message});
        }        
    }
    [HttpPatch("me")]
    public async Task<IActionResult> Patch(Guid id, PatchUserRequest request)
    {
        try
        {
            var result = await _usecase.PatchUser(id, request);
            return Ok(new
            {
                Sucess = result
            });
        } catch(Exception e)
        {
            return BadRequest(new {Error = e.Message});
        }
    }
    [HttpDelete("me")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await _usecase.DeleteUser(id);
            return Ok(new
            {
                Sucess = result
            });
        } catch(Exception e)
        {
            return BadRequest(new {Error = e.Message});
        }
    }
}
            
""")

    def Criando_CONSOLE(self):
        PathPadraoConsole = os.path.join(self.PathProjeto, f"{self.NomePrograma}.API")
        if os.path.exists(PathPadraoConsole):
            print("A pasta API ja existe")
            return

        resultado = subprocess.run(
            ["dotnet", "new", "console", "-n", f"{self.NomePrograma}.CONSOLE"],
            cwd=self.PathProjeto
        )
        if resultado.returncode != 0:
            print("Erro ao criar o projeto API")
            return  
        AdicionarBiblioteca = subprocess.run(["dotnet", "add", "package", "Microsoft.EntityFrameworkCore.Sqlite"])


def Controle_args(args: list[str]):
    args[1] = args[1].upper()
    if args[1] in ("-N", "--N", "--NAME", "-NAME"):
        print("Executando função especial")
    return

def CodigoProducao():
    if len(args) == 3:
        programa = Program(args[1], args[2])
        programa.Main()
    elif len(args) >= 4:
        programa = Program(args[1], args[2], args[3])
        programa.Main()
    else:
        print("Mais um dia normal")
def Teste():
    programa = Program("SwaggerLoader", "API")
    programa.Main()
if __name__ == "__main__":
    Teste()