using ArchGen.Domain;
using Microsoft.EntityFrameworkCore;

namespace ArchGen.InfraStructure;

public class ArchGenRepository : IArchGenRepository
{
    private readonly AppDbContext _context;
    public ArchGenRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> VerifyExists_OnThe_Databank(string NomeProjeto)
    {
        try
        {
            var query = await _context.Template.Where(x => x.NomeProjeto == NomeProjeto).FirstOrDefaultAsync();
            if(query == null)
            {
                return false;
            }
            return true;
        } catch(Exception e)
        {
            throw new InfraStructureException(e.Message);
        }
    }
}
