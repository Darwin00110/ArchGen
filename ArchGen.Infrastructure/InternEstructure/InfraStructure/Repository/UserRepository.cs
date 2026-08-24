
namespace InfraStructure;
using Domain;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> CreateUser(User user)
    {
        try
        {   
            var query = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        } catch(Exception e)
        {
            throw new InfraStructureException($'Error: {e.Message}');
        }
    }
    public async Task<bool> UpdateUser(User user)
    {
        try
        {   
            var query = await _context.Users.Where(x => x.Email == user.Email).FirstOrDefaultAsync();
            if(query == null)
            {
                throw new InfraStructureException('Usuario não encontrado.');
            }
            query!.Email = user.Email;
            query!.Telefone = user.Telefone;
            query!.Senha = user.Senha;
            query!.Nome = user.Nome;
            await _context.SaveChangesAsync();
            return true;
        } catch(Exception e)
        {
            throw new InfraStructureException($'Error: {e.Message}');
        }
    }
    public async Task<bool> DeleteUser(Guid Id)
    {
        try
        {   
            var query = await _context.Users.Where(x => x.ID == Id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return true;
        } catch(Exception e)
        {
            throw new InfraStructureException($'Error: {e.Message}');
        }
    }
    public async Task<bool> VerifyUserExists_WithEmail(string email)
    {
        try
        {   
            var query = await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            if(query == null)
            {
                return false;
            }
            return true;
        } catch(Exception e)
        {
            throw new InfraStructureException($'Error: {e.Message}');
        }
    }
    public async Task<User> GetDataUser_WithID(Guid ID)
    {
        try
        {   
            var query = await _context.Users.Where(x => x.ID == ID).FirstOrDefaultAsync();
            if(query == null)
            {
                throw new InfraStructureException('Usuario não existe.');
            }
            return new User
            {
                Email = query.Email,
                Nome = query.Nome,
                Senha = query.Senha,
                Telefone = query.Telefone
            };
        } catch(Exception e)
        {
            throw new InfraStructureException($'Error: {e.Message}');
        }
    }
    public async Task<bool> VerifyUserExists_WithID(Guid ID)
    {
        try
        {
            var query = await _context.Users.Where(x => x.ID == ID).FirstOrDefaultAsync();
            if(query == null)
            {   
                return false;
            }
            return true;
        } catch(Exception e)
        {
            throw new InfraStructureException($'Error: {e.Message}');
        }
    } 
}