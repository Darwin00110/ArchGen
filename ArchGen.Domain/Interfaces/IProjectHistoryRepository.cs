using ArchGen.Domain.Entities;

namespace ArchGen.Domain.Interfaces;

public interface IProjectHistoryRepository
{
    Task AddAsync(ProjectHistory history, CancellationToken ct = default);
    Task<List<ProjectHistory>> GetAllAsync(CancellationToken ct = default);
    Task<ProjectHistory?> GetByIdAsync(Guid id, CancellationToken ct = default);
    public Task<bool> Verify_ProjectExists(string NameProject);
}
