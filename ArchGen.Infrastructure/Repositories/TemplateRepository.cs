using ArchGen.Domain.Entities;
using ArchGen.Domain.Interfaces;
using ArchGen.InfraStructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ArchGen.InfraStructure.Repositories;

public class TemplateRepository : ITemplateRepository
{
    private readonly AppDbContext _context;

    public TemplateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TemplateDefinition?> GetByNameAsync(string name, CancellationToken ct = default)
    {
        return await _context.Templates
            .Include(t => t.Files)
            .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower(), ct);
    }

    public async Task<TemplateDefinition?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Templates
            .Include(t => t.Files)
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<List<TemplateDefinition>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Templates
            .Include(t => t.Files)
            .OrderBy(t => t.Name)
            .ToListAsync(ct);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default)
    {
        return await _context.Templates
            .AnyAsync(t => t.Name.ToLower() == name.ToLower(), ct);
    }

    public async Task AddAsync(TemplateDefinition template, CancellationToken ct = default)
    {
        _context.Templates.Add(template);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(TemplateDefinition template, CancellationToken ct = default)
    {
        _context.Templates.Update(template);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(TemplateDefinition template, CancellationToken ct = default)
    {
        _context.Templates.Remove(template);
        await _context.SaveChangesAsync(ct);
    }
}
