using ArchGen.Application.DTOs;

namespace ArchGen.Application.Interfaces;

public interface ITemplateUseCase
{
    Task<List<TemplateDto>> GetAllTemplatesAsync(CancellationToken ct = default);
    Task<TemplateDetailDto?> GetTemplateDetailsAsync(string name, CancellationToken ct = default);
    Task<TemplateDto> CreateTemplateAsync(CreateTemplateRequest request, CancellationToken ct = default);
    Task<TemplateFileDto> UpdateTemplateFileAsync(UpdateTemplateFileRequest request, CancellationToken ct = default);
    Task<bool> DeleteTemplateAsync(string name, CancellationToken ct = default);
    Task<string?> GetFileContentAsync(string templateName, string relativePath, CancellationToken ct = default);
}
