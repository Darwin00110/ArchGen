import subprocess
import pyautogui

import os
import subprocess
import sys
import shutil
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
        with open(PathArquivo, 'w', encoding="UTF-8") as file:
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
            self.ModificandoArquivos(os.path.join(PathPadraoDomain, "Entities", "User.cs"), f"""
namespace {self.NomePrograma}.Domain;

public class User
{{
    public Guid ID {{get; set;}}
    public string Nome {{get; set; }} = string.Empty;
    public string Email {{get; set;}} = string.Empty;
    public string Telefone {{get; set;}} = string.Empty;
    public void Validate_Nome()
    {{
        if (string.IsNullOrEmpty(Nome))
        {{
            throw new DomainException("Nome não pode estar vazio, campo obrigatorio.");
        }}
    }}
    public void Validate_Email()
    {{
        if (string.IsNullOrEmpty(Email))
        {{
            throw new DomainException("Email não pode estar vazio, campo obrigatorio.");
        }}
        if (!Email.Contains("@gmail.com"))
        {{
            throw new DomainException("Formato Invalido do Email, ex: (exemplo@gmail.com)");
        }}
    }}
    public void Validate_Telefone()
    {{
        if (string.IsNullOrEmpty(Telefone))
        {{
            throw new DomainException("Telefone não pode estar vazio, campo obrigatorio.");
        }}
    }}
}}

""")
        if not os.path.exists(os.path.join(PathPadraoDomain, "Exceptions")):
            os.mkdir(os.path.join(PathPadraoDomain, "Exceptions"))
            self.CriandoClassesCS("DomainException", os.path.join(PathPadraoDomain, "Exceptions"))
            self.ModificandoArquivos(os.path.join(PathPadraoDomain, "Exceptions", "DomainException.cs"), f"""
namespace {self.NomePrograma}.Domain;

public class DomainException : Exception
{{
    public DomainException(string message) : base(message) {{}}
}}
""")
            self.CriandoClassesCS("UseCaseException", os.path.join(PathPadraoDomain, "Exceptions"))
            self.ModificandoArquivos(os.path.join(PathPadraoDomain, "Exceptions", "UseCaseException.cs"), f"""
namespace {self.NomePrograma}.Domain;

public class UseCaseException : Exception
{{
    public UseCaseException(string message) : base(message) {{}}
}}
""")
            
            self.CriandoClassesCS("InfraStructureException", os.path.join(PathPadraoDomain, "Exceptions"))
            self.ModificandoArquivos(os.path.join(PathPadraoDomain, "Exceptions", "InfraStructureException.cs"), f"""
namespace {self.NomePrograma}.Domain;

public class InfraStructureException : Exception
{{
    public InfraStructureException(string message) : base(message) {{}}
}}
""")
        if not os.path.exists(os.path.join(PathPadraoDomain, "Interfaces")):
            os.mkdir(os.path.join(PathPadraoDomain, "Interfaces"))
            self.CriandoClassesCS("IUserRepository", os.path.join(PathPadraoDomain, "Interfaces"), "interface")
            self.ModificandoArquivos(os.path.join(PathPadraoDomain, "Interfaces", "IUserRepository.cs"), f"""
namespace {self.NomePrograma}.Domain;
public interface IUserRepository
{{
    public Task<bool> VerifyUserExists(Guid id);
    public Task<bool> VerifyUserExists_WithEmail(string Email);
    public Task Create(User data);
    public Task<User> Read(Guid id);
    public Task Patch(Guid id, User data);
    public Task Update(Guid id, User data);
    public Task Delete(Guid id);
}}
""")
        if not os.path.exists(os.path.join(PathPadraoDomain, "Enums")):
            os.mkdir(os.path.join(PathPadraoDomain, "Enums"))
            self.CriandoClassesCS("OptionsStatusUsuario", os.path.join(PathPadraoDomain, "Enums"), "enum")

    def Criando_Solucao(self):
        caminho_sln = os.path.join(self.PathProjeto, f"{self.NomePrograma}.sln")
        if not os.path.exists(caminho_sln):
            print("O arquivo de solução esta sendo criado")
            subprocess.run(["dotnet", "new", "sln", "-n", self.NomePrograma], cwd=self.PathProjeto)
        if self.TipoPrograma == "API":
            pathPadraoSlnUso = self.PathPadraoAPI
        else:
            pathPadraoSlnUso = self.PathPadraoConsole        
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
            subprocess.run(["dotnet", "sln", "add", f"{self.NomePrograma}.Console"], cwd=self.PathProjeto)
            subprocess.run(
                ["dotnet", "add", f"{self.NomePrograma}.Console", "reference", f"{self.NomePrograma}.Application"],
                cwd=self.PathProjeto
            )
            subprocess.run(
                ["dotnet", "add", f"{self.NomePrograma}.Console", "reference", f"{self.NomePrograma}.Domain"],
                cwd=self.PathProjeto
            )
            subprocess.run(
                ["dotnet", "add", f"{self.NomePrograma}.Console", "reference", f"{self.NomePrograma}.InfraStructure"],
                cwd=self.PathProjeto
            )

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
        if self.TipoPrograma == "API":
            pathPadraoSlnUso = self.PathPadraoAPI
        elif self.TipoPrograma == "CONSOLE":
            pathPadraoSlnUso = self.PathPadraoConsole
        
        migrations = subprocess.run(["dotnet", "ef", "migrations", "add", "Initial_Project", "--startup-project", pathPadraoSlnUso], cwd=self.PathPadraoInfraStructure)
        if migrations.returncode != 0:
            print("Erro na criação das 'migrations', pulando processo.")

        database = subprocess.run(["dotnet", "ef", "database", "update", "--startup-project", pathPadraoSlnUso], cwd=self.PathPadraoInfraStructure)
        if database.returncode != 0:
            print("Erro na criação das 'database', pulando processo.")

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
            self.ModificandoArquivos(os.path.join(PathPadraoApplication, "UseCase", "UserUseCase.cs"), f"""
using {self.NomePrograma}.Domain;

namespace {self.NomePrograma}.Application;

public class UserUseCase : IUserUseCase
{{
    private readonly IUserRepository _repo;
    public UserUseCase(IUserRepository repo)
    {{
        _repo = repo;
    }}
    public async Task<bool> Create(CreateUserRequest request)
    {{

        return false;
    }}
    public async Task<User> ReadUser(Guid id)
    {{
        return new User
        {{

        }};
    }}
    public async Task<bool> UpdateUser(Guid id, UpdateUserRequest request)
    {{
        return false;
    }}
    public async Task<bool> PatchUser(Guid id, PatchUserRequest request)
    {{
        return false;

    }}
    public async Task<bool> DeleteUser(Guid id)
    {{
        return false;
    }}
}}
""")
        if not os.path.exists(os.path.join(PathPadraoApplication, "Interfaces")):
            os.mkdir(os.path.join(PathPadraoApplication, "Interfaces"))
            self.CriandoClassesCS("IUserUseCase", os.path.join(PathPadraoApplication, "Interfaces"), "interface")
            self.ModificandoArquivos(os.path.join(PathPadraoApplication, "Interfaces", "IUserUseCase.cs"), f"""
using {self.NomePrograma}.Domain;

namespace {self.NomePrograma}.Application;

public interface IUserUseCase
{{
    public Task<bool> Create(CreateUserRequest request);
    public Task<User> ReadUser(Guid id);
    public Task<bool> UpdateUser(Guid id, UpdateUserRequest request);
    public Task<bool> PatchUser(Guid id, PatchUserRequest request);
    public Task<bool> DeleteUser(Guid id);
}}

""")

    def Criando_InfraStrutura(self):
        self.PathPadraoInfraStructure = os.path.join(self.PathProjeto, f"{self.NomePrograma}.InfraStructure")
        if os.path.exists(self.PathPadraoInfraStructure):
            print("A pasta InfraStructure ja existe")
            return

        resultado = subprocess.run(
            ["dotnet", "new", "classlib", "-n", f"{self.NomePrograma}.InfraStructure"],
            cwd=self.PathProjeto
        )
        if resultado.returncode != 0:
            print("Erro ao criar o projeto InfraStructure")
            return

        arquivo_padrao = os.path.join(self.PathPadraoInfraStructure, "Class1.cs")
        if os.path.exists(arquivo_padrao):
            os.remove(arquivo_padrao)

        if not os.path.exists(os.path.join(self.PathPadraoInfraStructure, "Data")):
            os.mkdir(os.path.join(self.PathPadraoInfraStructure, "Data"))
            self.CriandoClassesCS("AppDbContext", os.path.join(self.PathPadraoInfraStructure, "Data"))
            self.ModificandoArquivos(os.path.join(self.PathPadraoInfraStructure, "Data", "AppDbContext.cs"), f"""
using Microsoft.EntityFrameworkCore;
using {self.NomePrograma}.Domain;

namespace {self.NomePrograma}.InfraStructure;

public class AppDbContext : DbContext
{{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){{}}
    public DbSet<User> User {{get; set;}}
}}

""")
        if not os.path.exists(os.path.join(self.PathPadraoInfraStructure, "Repository")):
            os.mkdir(os.path.join(self.PathPadraoInfraStructure, "Repository"))
            self.CriandoClassesCS("UserRepository", os.path.join(self.PathPadraoInfraStructure, "Repository"))
            self.ModificandoArquivos(os.path.join(self.PathPadraoInfraStructure, "Repository", "UserRepository.cs"), f"""
using Microsoft.EntityFrameworkCore;
using {self.NomePrograma}.Application;
using {self.NomePrograma}.Domain;
using System.Diagnostics;
using {self.NomePrograma}.InfraStructure;

namespace {self.NomePrograma}.InfraStructure;

public class UserRepository : IUserRepository
{{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db)
    {{
        _db = db;
    }}
    public async Task Create(User data)
    {{
        try
        {{
            var query = await _db.User.AddAsync(data);
            await _db.SaveChangesAsync();
        }}
        catch (Exception e)
        {{
            throw new InfraStructureException($"Repository Error: {{e.Message}}");
        }}
    }}
    public async Task<bool> VerifyUserExists_WithEmail(string Email)
    {{
        try
        {{
            var query = await _db.User.Where(x => x.Email == Email).FirstOrDefaultAsync();
            if (query == null)
            {{
                return false;
            }}
            return true;
        }}
        catch (Exception e)
        {{
            throw new InfraStructureException($"Repository Error: {{e.Message}}");
        }}
    }}
    public async Task<User> Read(Guid id)
    {{
        try
        {{
            var query = await _db.User.Where(x => x.ID == id).FirstOrDefaultAsync();
            if (query == null)
            {{
                throw new InfraStructureException("Usuario n�o existe, ou n�o encontrado");
            }}
            return query;
        }}
        catch (Exception e)
        {{
            throw new InfraStructureException($"Repository Error: {{e.Message}}");
        }}
    }}
    public async Task Patch(Guid id, User data)
    {{
        try
        {{

            var usuario = await _db.User.FindAsync(id);
            if (usuario is null)
                throw new InfraStructureException("Repository Error: Usu�rio n�o encontrado.");

            if (!string.IsNullOrWhiteSpace(data.Nome))
                usuario.Nome = data.Nome;

            if (!string.IsNullOrWhiteSpace(data.Email))
                usuario.Email = data.Email;
            if (!string.IsNullOrWhiteSpace(data.Telefone))
                usuario.Telefone = data.Telefone;

            // Status � um enum (OptionsStatusUsuario), n�o string � n�o d� pra checar
            // "vazio" do mesmo jeito. Se 0 for um valor v�lido do enum, isso sempre
            // vai atualizar. Ajuste conforme o que faz sentido no seu dom�nio.

            await _db.SaveChangesAsync();
        }}
        catch (Exception e)
        {{
            throw new InfraStructureException($"Repository Error: {{e.Message}}");
        }}
    }}

    public async Task Update(Guid id, User data)
    {{
        try
        {{

            var usuario = await _db.User.FindAsync(id);
            if (usuario is null)
                throw new InfraStructureException("Repository Error: Usu�rio n�o encontrado.");

            // Update = substitui��o completa, diferente do Patch
            usuario.Nome = data.Nome;
            usuario.Email = data.Email;
            usuario.Telefone = data.Telefone;
            await _db.SaveChangesAsync();
        }}
        catch (Exception e)
        {{
            throw new InfraStructureException($"Repository Error: {{e.Message}}");
        }}
    }}

    public async Task Delete(Guid id)
    {{
        try
        {{
            var usuario = await _db.User.FindAsync(id);
            if (usuario is null)
                throw new InfraStructureException("Repository Error: Usu�rio n�o encontrado.");
            _db.User.Remove(usuario);
            await _db.SaveChangesAsync();
        }}
        catch (Exception e)
        {{
            throw new InfraStructureException($"Repository Error: {{e.Message}}");
        }}
    }}
    public async Task<bool> VerifyUserExists(Guid id)
    {{
        try
        {{
            var query = await _db.User.Where(x => x.ID == id).FirstOrDefaultAsync();
            if (query == null)
            {{
                return false;
            }}
            return true;
        }}
        catch (Exception e)
        {{
            throw new InfraStructureException($"Repository Error: {{e.Message}}");
        }}
    }}
}}
""")
        subprocess.run(["dotnet", "add", "package", "Microsoft.EntityFrameworkCore", "--version", "8.0.0"], cwd=self.PathPadraoInfraStructure)
        subprocess.run(["dotnet", "add", "package", "Microsoft.EntityFrameworkCore.Sqlite", "--version", "8.0.0"], cwd=self.PathPadraoInfraStructure)
        subprocess.run(["dotnet", "add", "package", "Microsoft.EntityFrameworkCore.Design", "--version", "8.0.0"], cwd=self.PathPadraoInfraStructure)
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
        self.PathPadraoAPI = os.path.join(self.PathProjeto, f"{self.NomePrograma}.API")
        if os.path.exists(self.PathPadraoAPI):
            print("A pasta API ja existe")
            return

        resultado = subprocess.run(
            ["dotnet", "new", "webapi", "-n", f"{self.NomePrograma}.API"],
            cwd=self.PathProjeto
        )
        if resultado.returncode != 0:
            print("Erro ao criar o projeto API")
            return
        
        self.ModificandoArquivos(os.path.join(self.PathPadraoAPI, "Program.cs"), f"""
using Microsoft.EntityFrameworkCore;
using {self.NomePrograma}.Application;
using {self.NomePrograma}.Domain;
using {self.NomePrograma}.InfraStructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserUseCase, UserUseCase>();

var NomeProjeto = Environment.CurrentDirectory;
Console.WriteLine(NomeProjeto);

var PathBanco = Path.Combine(@"{self.PathPadraoInfraStructure}", "Data", "database.db");
builder.Services.AddDbContext<AppDbContext>(options =>
{{
    options.UseSqlite($"Data Source={{PathBanco}}");
}});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{{
    app.UseSwagger();
    app.UseSwaggerUI();
}}

app.MapControllers();
app.UseHttpsRedirection();
app.Run();
""")
        subprocess.run(["dotnet", "add", "package", "Microsoft.EntityFrameworkCore", "--version", "8.0.0"], cwd=self.PathPadraoAPI)
        subprocess.run(["dotnet", "add", "package", "Microsoft.EntityFrameworkCore.Design", "--version", "8.0.0"], cwd=self.PathPadraoAPI)
        subprocess.run(["dotnet", "add", "package", "Microsoft.EntityFrameworkCore.Sqlite", "--version", "8.0.0"], cwd=self.PathPadraoAPI)
        subprocess.run(["dotnet", "add", "package", "Microsoft.AspNetCore.Mvc"], cwd=self.PathPadraoAPI)
        if not os.path.exists(os.path.join(self.PathPadraoAPI, "Controller")):
            os.mkdir(os.path.join(self.PathPadraoAPI, "Controller"))
            self.CriandoClassesCS("UserController", os.path.join(self.PathPadraoAPI, "Controller"))
            self.ModificandoArquivos(os.path.join(self.PathPadraoAPI, "Controller","UserController.cs"), f"""
using Microsoft.AspNetCore.Mvc;
using {self.NomePrograma}.Application;

namespace {self.NomePrograma}.API;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{{
    private readonly IUserUseCase _usecase;
    public UserController(IUserUseCase usecase)
    {{
        _usecase = usecase;
    }}
    [HttpPost("register")]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {{
        try
        {{
            var result = await _usecase.Create(request);
            return Ok(new
            {{
                Sucess = result
            }});
        }} catch(Exception e)
        {{
            return BadRequest(new {{Error = e.Message}});
        }}
    }}
    [HttpGet("me")]
    public async Task<IActionResult> Read(Guid id)
    {{
        try
        {{
            var result = await _usecase.ReadUser(id);
            return Ok(new
            {{
                Data = result
            }});
        }} catch(Exception e)
        {{
            return BadRequest(new {{Error = e.Message}});
        }}        
    }}
    [HttpPut("me")]
    public async Task<IActionResult> Update(Guid id, UpdateUserRequest request)
    {{
        try
        {{
            var result = await _usecase.UpdateUser(id, request);
            return Ok(new
            {{
                Sucess = result
            }});
        }} catch(Exception e)
        {{
            return BadRequest(new {{Error = e.Message}});
        }}       
    }}
    [HttpPatch("me")]
    public async Task<IActionResult> Patch(Guid id, PatchUserRequest request)
    {{
        try
        {{
            var result = await _usecase.PatchUser(id, request);
            return Ok(new
            {{
                Sucess = result
            }});
        }} catch(Exception e)
        {{
            return BadRequest(new {{Error = e.Message}});
        }}
    }}
    [HttpDelete("me")]
    public async Task<IActionResult> Delete(Guid id)
    {{
        try
        {{
            var result = await _usecase.DeleteUser(id);
            return Ok(new
            {{
                Sucess = result
            }});
        }} catch(Exception e)
        {{
            return BadRequest(new {{Error = e.Message}});
        }}
    }}
}}
            
""")
    @staticmethod
    def Instalacao_Sistema(Path = os.getcwd()):
        ProgramaExecutavel = os.path.join(Path, "ArchGen.exe")
        if not os.path.exists(ProgramaExecutavel):
            print("Arquivo executavel não existe, favor instalar o programa novamente, ou execute (python ArchGen.py --install all)")
        else:
            PastaProgramFiles = os.path.join(os.environ["ProgramFiles"], "ArchGen")
            if not os.path.exists(PastaProgramFiles):
                print("Criando pasta do ArchGen")
                os.mkdir(PastaProgramFiles)
            if not os.path.exists(os.path.join(PastaProgramFiles, "ArchGen.exe")):
                print("Movendo executavel do ArchGen")
                shutil.move(ProgramaExecutavel, PastaProgramFiles)
            
    def Criando_CONSOLE(self):
        self.PathPadraoConsole = os.path.join(self.PathProjeto, f"{self.NomePrograma}.Console")
        if os.path.exists(self.PathPadraoConsole):
            print("A pasta Console ja existe")
            return

        resultado = subprocess.run(
            ["dotnet", "new", "console", "-n", f"{self.NomePrograma}.Console"],
            cwd=self.PathProjeto
        )
        if resultado.returncode != 0:
            print("Erro ao criar o projeto Console")
            return  
        biblioteca_EntityFrameCoreSqlite = subprocess.run(["dotnet", "add", "package", "Microsoft.EntityFrameworkCore.Sqlite", "--version", "8.0.0"], cwd=self.PathPadraoConsole)
        if biblioteca_EntityFrameCoreSqlite.returncode != 0:
            print("Erro na instalação da biblioteca Microsoft.EntityFrameworkCore.Sqlite, prosseguindo para as proximas etapas.")

        biblioteca_EntityFrameCoreSqliteDesing = subprocess.run(["dotnet", "add", "package", "Microsoft.EntityFrameworkCore.Design", "--version", "8.0.0"], cwd=self.PathPadraoConsole)
        if biblioteca_EntityFrameCoreSqliteDesing.returncode != 0:
            print("Erro na instalação da biblioteca Microsoft.EntityFrameworkCore.Design, prosseguindo para as proximas etapas.")
        biblioteca_EntityFrameworkCore = subprocess.run(["dotnet", "add", "package", "Microsoft.EntityFrameworkCore", "--version", "8.0.0"], cwd=self.PathPadraoConsole)
        if biblioteca_EntityFrameworkCore.returncode != 0:
            print("Erro na instalação da biblioteca Microsoft.EntityFrameworkCore, prosseguindo para as proximas etapas.")
        
        biblioteca_InjecaoDependencia = subprocess.run(["dotnet", "add", "package", "Microsoft.Extensions.DependencyInjection"], cwd=self.PathPadraoConsole)
        if biblioteca_InjecaoDependencia.returncode != 0:
            print("Erro na instalação da biblioteca Microsoft.Extensions.DependencyInjection, prosseguindo para as proximas etapas.")
        
        biblioteca_InjecaoDependenciaHost = subprocess.run(["dotnet", "add", "package", "Microsoft.Extensions.Hosting", "--version", "8.0.0"], cwd=self.PathPadraoConsole)
        if biblioteca_InjecaoDependenciaHost.returncode != 0:
            print("Erro na instalação da biblioteca  Microsoft.Extensions.Hosting, prosseguindo para as proximas etapas.")
        pathProgram = os.path.join(self.PathPadraoConsole, "Program.cs")
        self.ModificandoArquivos(pathProgram, """
using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=aquiles.db"));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserUseCase, UserUseCase>();

var app = builder.Build();

var rootCommand = new RootCommand("Aquiles CLI");

// ---- comando: aquiles user create --nome "Darwin" --email x@gmail.com ----
var userCommand = new Command("user", "Gerencia usuários");
var createCommand = new Command("create", "Cria um novo usuário");

var nomeOption = new Option<string>("--nome") { IsRequired = true };
var emailOption = new Option<string>("--email") { IsRequired = true };

createCommand.AddOption(nomeOption);
createCommand.AddOption(emailOption);

createCommand.SetHandler(async (nome, email) =>
{
    using var scope = app.Services.CreateScope();
    var useCase = scope.ServiceProvider.GetRequiredService<IUserUseCase>();

    try
    {
        var result = await useCase.Create(new CreateUserRequest { Nome = nome, Email = email });
        Console.WriteLine(result ? "Usuário criado com sucesso." : "Falha ao criar usuário.");
    }
    catch (DomainException ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}, nomeOption, emailOption);

userCommand.AddCommand(createCommand);
rootCommand.AddCommand(userCommand);

return await rootCommand.InvokeAsync(args);
""")
if __name__ == "__main__":
    if len(args) == 3:
        programa = Program(args[1], args[2])
        programa.Main()
        if len(args) >= 4:
            programa = Program(args[1], args[2], args[3])
            programa.Main()
        else:
            print("Path não foi passado, usando o path atual da execução do programa.")
    else:
        print("INFO")