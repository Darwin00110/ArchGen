using ArchGen.Application.DTOs;
using ArchGen.Application.Interfaces;
using ArchGen.Domain.Interfaces;

namespace ArchGen.Application.UseCases;

public class ProjectHistoryUseCase : IProjectHistoryUseCase
{
    private readonly IProjectHistoryRepository _historyRepository;

    public ProjectHistoryUseCase(IProjectHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }

    public async Task<List<ProjectHistoryDto>> GetAllHistoryAsync(CancellationToken ct = default)
    {
        var list = await _historyRepository.GetAllAsync(ct);
        return list.Select(h => new ProjectHistoryDto
        {
            Id = h.Id,
            ProjectName = h.ProjectName,
            ProjectPath = h.ProjectPath,
            ProjectType = h.ProjectType,
            TemplateUsed = h.TemplateUsed,
            CreatedAt = h.CreatedAt,
            Status = h.Status,
            Details = h.Details
        }).OrderByDescending(h => h.CreatedAt).ToList();
    }

    public async Task<ProjectHistoryDto?> GetHistoryByIdAsync(int id, CancellationToken ct = default)
    {
        var h = await _historyRepository.GetByIdAsync(id, ct);
        if (h == null) return null;

        return new ProjectHistoryDto
        {
            Id = h.Id,
            ProjectName = h.ProjectName,
            ProjectPath = h.ProjectPath,
            ProjectType = h.ProjectType,
            TemplateUsed = h.TemplateUsed,
            CreatedAt = h.CreatedAt,
            Status = h.Status,
            Details = h.Details
        };
    }
}
