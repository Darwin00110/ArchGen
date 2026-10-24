using ArchGen.Application.DTOs;

namespace ArchGen.Application.Interfaces;

public interface IGenerateProjectUseCase
{
    Task<GenerateProjectResult> ExecuteAsync(GenerateProjectRequest request, CancellationToken ct = default);
}
