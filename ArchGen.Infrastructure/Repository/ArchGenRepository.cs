using ArchGen.Domain;
using Microsoft.EntityFrameworkCore;

namespace ArchGen.Infrastructure;

public class ArchGenRepository : IArchGenRepository
{
    private readonly AppDbContext _context;
    public ArchGenRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> CreateNewData(ArchGenEntity data)
    {
        try
        {
            await _context.AddAsync(data);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao criar novo dado: {ex.Message}");
        }
        return true;
    }
    public async Task<string> GetPathDomain() {
        try
        {
            var query = await _context.DataUser.Where(x => x.PathDomain != string.Empty).FirstOrDefaultAsync();
            if(query == null)
            {
                throw new InfrastructureException("Nenhum caminho de domínio encontrado.");
            }
            return query.PathDomain!;
        } catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<string> GetPathApplication() {
        try
        {
            var query = await _context.DataUser.Where(x => x.PathApplication != string.Empty).FirstOrDefaultAsync();
            if(query == null)
            {
                throw new InfrastructureException("Nenhum caminho de aplicação encontrado.");
            }
            return query.PathApplication!;
        } catch (Exception ex)
        {
            throw new Exception($"Erro ao obter o caminho da aplicação: {ex.Message}");
        }
        }
    public async Task<string> GetPathInfrastructure() {
        try
        {
            var query = await _context.DataUser.Where(x => x.PathInfrastructure != string.Empty).FirstOrDefaultAsync();
            if(query == null)
            {
                throw new InfrastructureException("Nenhum caminho de infraestrutura encontrado.");
            }
            return query.PathInfrastructure!;
        } catch (Exception ex)
        {
            throw new Exception($"Erro ao obter o caminho da infraestrutura: {ex.Message}");
        }
    }
    public async Task<string> GetPathTests() {
        try
        {
            var query = await _context.DataUser.Where(x => x.PathTests != string.Empty).FirstOrDefaultAsync();
            if(query == null)
            {
                throw new InfrastructureException("Nenhum caminho de testes encontrado.");
            }
            return query.PathTests!;
        } catch (Exception ex)
        {
            throw new Exception($"Erro ao obter o caminho dos testes: {ex.Message}");
        }
    }
}
