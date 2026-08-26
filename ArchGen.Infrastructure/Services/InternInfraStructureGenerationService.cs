using ArchGen.Application;
using ArchGen.Domain;

namespace ArchGen.Infrastructure;

public class InternInfraStructureGenerationService
{
    private readonly IDotnetService _dotnet;
    private string PathInfrastructureFromInternEstructure = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure" ,"InfraStructure"));
    private string PathInternSolution = Path.Combine(Environment.CurrentDirectory, "..", "ArchGen.Infrastructure", "InternEstructure");

    public InternInfraStructureGenerationService(IDotnetService dotnet)
    {
        _dotnet = dotnet;
    }
    public async Task SetPathInternInfraStructure(string path)
    {
        if (Path.Exists(path))
        {
            PathInfrastructureFromInternEstructure = Path.Combine(path, "InfraStructure");
            PathInternSolution = path;
        } else
        {
            throw new ServiceException("O path Não existe");
        }
    }
    public async Task VerifyInfraStructureInternStructure()
    {
        var PathInfraStructureData = Path.Combine(PathInfrastructureFromInternEstructure!, "Data");
        string PathInfraStructureServices = Path.Combine(PathInfrastructureFromInternEstructure!, "Services");
        string PathInfraStructureRepository = Path.Combine(PathInfrastructureFromInternEstructure!, "Repository");
        if (Directory.Exists(PathInfrastructureFromInternEstructure))
        {
            throw new ServiceException("Impossivel continuar, a Pasta(Application) ja existe.");
        }
        if(Directory.Exists(PathInfraStructureData))
        {
            throw new ServiceException("Impossivel continuar, a Pasta(Data) ja existe.");
        }
        if(Directory.Exists(PathInfraStructureRepository))
        {
            throw new ServiceException("Impossivel continuar, a Pasta(Repository) ja existe.");
        }
        if(Directory.Exists(PathInfraStructureServices))
        {
            throw new ServiceException("Impossivel continuar, a Pasta(Services) ja existe.");
        }
        

        if(File.Exists(Path.Combine(PathInfraStructureData, "AppDbContext.cs")))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(AppDbContext.cs) ja existe.");           
        }
        if(File.Exists(Path.Combine(PathInfraStructureData, "database.db")))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(database.db) ja existe.");           
        }
        
        if(File.Exists(Path.Combine(PathInfraStructureRepository, "UserRepository.cs")))
        {
            throw new ServiceException("Impossivel continuar, o Arquivo(UserRepository.cs.cs) ja existe.");           
        }
    }
    public async Task CreateInternStructure()
    {
        if (!Directory.Exists(PathInfrastructureFromInternEstructure))
        {
            await _dotnet.CriarCamada_Classlib("InfraStructure", PathInternSolution);
            var PathData = Path.Combine(PathInfrastructureFromInternEstructure!, "Data");
            var PathRepository = Path.Combine(PathInfrastructureFromInternEstructure!, "Repository");
            
            var PathFile_AppDbContext = Path.Combine(PathData, "AppDbContext.cs");
            var PathFile_AppDbContextFactory = Path.Combine(PathData, "AppDbContextFactory.cs");
            var PathFile_UserRepository = Path.Combine(PathRepository, "UserRepository.cs");
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
            throw new InfraStructureException($""Error: {{e.Message}}"");
        }}
    }}
    public async Task<bool> UpdateUser(User user)
    {{
        try
        {{   
            var query = await _context.Users.Where(x => x.Email == user.Email).FirstOrDefaultAsync();
            if(query == null)
            {{
                throw new InfraStructureException(""Usuario não encontrado."");
            }}
            query!.Email = user.Email;
            query!.Telefone = user.Telefone;
            query!.Senha = user.Senha;
            query!.Nome = user.Nome;
            await _context.SaveChangesAsync();
            return true;
        }} catch(Exception e)
        {{
            throw new InfraStructureException($""Error: {{e.Message}}"");
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
            throw new InfraStructureException($""Error: {{e.Message}}"");
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
            throw new InfraStructureException($""Error: {{e.Message}}"");
        }}
    }}
    public async Task<User> GetDataUser_WithID(Guid ID)
    {{
        try
        {{   
            var query = await _context.Users.Where(x => x.ID == ID).FirstOrDefaultAsync();
            if(query == null)
            {{
                throw new InfraStructureException(""Usuario não existe."");
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
            throw new InfraStructureException($""Error: {{e.Message}}"");
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
            throw new InfraStructureException($""Error: {{e.Message}}"");
        }}
    }} 
}}");
        }
        if(File.Exists(Path.Combine(PathInfrastructureFromInternEstructure, "Class1.cs")))
        {
            File.Delete(Path.Combine(PathInfrastructureFromInternEstructure, "Class1.cs"));
        }
        await _dotnet.AddedPackageInTheProject("Microsoft.EntityFrameworkCore", PathInfrastructureFromInternEstructure!);
        await _dotnet.AddedPackageInTheProject("Microsoft.EntityFrameworkCore.Design", PathInfrastructureFromInternEstructure!);
        await _dotnet.AddedPackageInTheProject("Microsoft.EntityFrameworkCore.Sqlite", PathInfrastructureFromInternEstructure!);
    }
    
}
