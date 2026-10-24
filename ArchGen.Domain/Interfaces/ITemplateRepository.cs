using ArchGen.Domain.Entities;

namespace ArchGen.Domain.Interfaces;

public interface ITemplateRepository
{
    Task<TemplateDefinition?> GetByNameAsync(string name, CancellationToken ct = default);
    Task<TemplateDefinition?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<TemplateDefinition>> GetAllAsync(CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
    Task AddAsync(TemplateDefinition template, CancellationToken ct = default);
    Task UpdateAsync(TemplateDefinition template, CancellationToken ct = default);
    Task DeleteAsync(TemplateDefinition template, CancellationToken ct = default);
}
