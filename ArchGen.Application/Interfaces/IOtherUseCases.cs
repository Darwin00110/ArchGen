using ArchGen.Application.DTOs;

namespace ArchGen.Application.Interfaces;

public interface IProjectHistoryUseCase
{
    Task<List<ProjectHistoryDto>> GetAllHistoryAsync(CancellationToken ct = default);
    Task<ProjectHistoryDto?> GetHistoryByIdAsync(int id, CancellationToken ct = default);
}

public interface IHelpUseCase
{
    List<HelpItemDto> GetHelpItems();
    string RenderHelpTable();
    string RenderHelpTable(int tableWidth, int rowHeight);
}
