using ArchGen.Domain.Entities;
using ArchGen.Domain.Interfaces;
using ArchGen.InfraStructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ArchGen.InfraStructure.Repositories;

public class ProjectHistoryRepository : IProjectHistoryRepository
{
    private readonly AppDbContext _context;

    public ProjectHistoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ProjectHistory history, CancellationToken ct = default)
    {
        _context.ProjectHistories.Add(history);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<List<ProjectHistory>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.ProjectHistories
            .OrderByDescending(h => h.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<ProjectHistory?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.ProjectHistories.FirstOrDefaultAsync(h => h.Id == id, ct);
    }
}
