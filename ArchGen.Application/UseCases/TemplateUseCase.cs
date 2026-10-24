using ArchGen.Application.DTOs;
using ArchGen.Application.Interfaces;
using ArchGen.Domain.Entities;
using ArchGen.Domain.Exceptions;
using ArchGen.Domain.Interfaces;

namespace ArchGen.Application.UseCases;

public class TemplateUseCase : ITemplateUseCase
{
    private readonly ITemplateRepository _templateRepository;
    private readonly IDefaultTemplateProvider _defaultTemplateProvider;

    public TemplateUseCase(
        ITemplateRepository templateRepository,
        IDefaultTemplateProvider defaultTemplateProvider)
    {
        _templateRepository = templateRepository;
        _defaultTemplateProvider = defaultTemplateProvider;
    }

    public async Task<List<TemplateDto>> GetAllTemplatesAsync(CancellationToken ct = default)
    {
        var result = new List<TemplateDto>();

        // Default template
        var defaultTemplate = _defaultTemplateProvider.GetDefaultTemplate();
        result.Add(new TemplateDto
        {
            Id = defaultTemplate.Id,
            Name = defaultTemplate.Name,
            Description = defaultTemplate.Description,
            IsDefault = true,
            CreatedAt = defaultTemplate.CreatedAt,
            UpdatedAt = defaultTemplate.UpdatedAt,
            FilesCount = defaultTemplate.Files.Count
        });

        // Templates from database
        var dbTemplates = await _templateRepository.GetAllAsync(ct);
        foreach (var t in dbTemplates)
        {
            if (t.Name.Equals(defaultTemplate.Name, StringComparison.OrdinalIgnoreCase))
                continue;

            result.Add(new TemplateDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                IsDefault = false,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                FilesCount = t.Files.Count
            });
        }

        return result;
    }

    public async Task<TemplateDetailDto?> GetTemplateDetailsAsync(string name, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("O nome do template não pode ser vazio.");

        TemplateDefinition? template = null;
        var defaultTemplate = _defaultTemplateProvider.GetDefaultTemplate();

        if (name.Equals(defaultTemplate.Name, StringComparison.OrdinalIgnoreCase))
        {
            template = defaultTemplate;
        }
        else
        {
            template = await _templateRepository.GetByNameAsync(name, ct);
        }

        if (template == null) return null;

        return new TemplateDetailDto
        {
            Id = template.Id,
            Name = template.Name,
            Description = template.Description,
            IsDefault = template.IsDefault,
            CreatedAt = template.CreatedAt,
            UpdatedAt = template.UpdatedAt,
            Files = template.Files.Select(f => new TemplateFileDto
            {
                Id = f.Id,
                RelativePath = f.RelativePath,
                Content = f.Content,
                Layer = f.Layer,
                IsForApi = f.IsForApi,
                IsForConsole = f.IsForConsole
            }).OrderBy(f => f.RelativePath).ToList()
        };
    }

    public async Task<TemplateDto> CreateTemplateAsync(CreateTemplateRequest request, CancellationToken ct = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException("O nome do novo template deve ser informado.");

        var cleanName = request.Name.Trim();
        var defaultTemplate = _defaultTemplateProvider.GetDefaultTemplate();

        if (cleanName.Equals(defaultTemplate.Name, StringComparison.OrdinalIgnoreCase))
            throw new ValidationException($"Não é permitido criar um template com o nome reservado '{defaultTemplate.Name}'.");

        var exists = await _templateRepository.ExistsByNameAsync(cleanName, ct);
        if (exists)
            throw new ValidationException($"Já existe um template cadastrado com o nome '{cleanName}'.");

        // Base template
        var baseName = string.IsNullOrWhiteSpace(request.BaseTemplateName) ? defaultTemplate.Name : request.BaseTemplateName.Trim();
        TemplateDefinition? baseTemplate;

        if (baseName.Equals(defaultTemplate.Name, StringComparison.OrdinalIgnoreCase))
        {
            baseTemplate = defaultTemplate;
        }
        else
        {
            baseTemplate = await _templateRepository.GetByNameAsync(baseName, ct);
            if (baseTemplate == null)
                throw new TemplateNotFoundException(baseName);
        }

        var newTemplate = baseTemplate.Clone(cleanName, request.Description);
        await _templateRepository.AddAsync(newTemplate, ct);

        return new TemplateDto
        {
            Id = newTemplate.Id,
            Name = newTemplate.Name,
            Description = newTemplate.Description,
            IsDefault = false,
            CreatedAt = newTemplate.CreatedAt,
            UpdatedAt = newTemplate.UpdatedAt,
            FilesCount = newTemplate.Files.Count
        };
    }

    public async Task<TemplateFileDto> UpdateTemplateFileAsync(UpdateTemplateFileRequest request, CancellationToken ct = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrWhiteSpace(request.TemplateName))
            throw new ValidationException("O nome do template deve ser informado.");

        if (string.IsNullOrWhiteSpace(request.RelativePath))
            throw new ValidationException("O caminho relativo do arquivo deve ser informado.");

        var defaultTemplate = _defaultTemplateProvider.GetDefaultTemplate();
        if (request.TemplateName.Equals(defaultTemplate.Name, StringComparison.OrdinalIgnoreCase))
        {
            throw new ValidationException("O template padrão ('Default') é somente leitura. Para modificá-lo, crie um novo template clonando o padrão usando 'template create' e edite o novo template.");
        }

        var template = await _templateRepository.GetByNameAsync(request.TemplateName, ct);
        if (template == null)
            throw new TemplateNotFoundException(request.TemplateName);

        var updatedFile = template.AddOrUpdateFile(
            request.RelativePath,
            request.Content,
            request.Layer ?? "Custom",
            request.IsForApi ?? true,
            request.IsForConsole ?? true
        );

        await _templateRepository.UpdateAsync(template, ct);

        return new TemplateFileDto
        {
            Id = updatedFile.Id,
            RelativePath = updatedFile.RelativePath,
            Content = updatedFile.Content,
            Layer = updatedFile.Layer,
            IsForApi = updatedFile.IsForApi,
            IsForConsole = updatedFile.IsForConsole
        };
    }

    public async Task<bool> DeleteTemplateAsync(string name, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("O nome do template deve ser informado.");

        var defaultTemplate = _defaultTemplateProvider.GetDefaultTemplate();
        if (name.Equals(defaultTemplate.Name, StringComparison.OrdinalIgnoreCase))
            throw new ValidationException("O template padrão ('Default') não pode ser excluído.");

        var template = await _templateRepository.GetByNameAsync(name, ct);
        if (template == null)
            throw new TemplateNotFoundException(name);

        await _templateRepository.DeleteAsync(template, ct);
        return true;
    }

    public async Task<string?> GetFileContentAsync(string templateName, string relativePath, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(templateName) || string.IsNullOrWhiteSpace(relativePath))
            return null;

        var defaultTemplate = _defaultTemplateProvider.GetDefaultTemplate();
        TemplateDefinition? template = null;

        if (templateName.Equals(defaultTemplate.Name, StringComparison.OrdinalIgnoreCase))
        {
            template = defaultTemplate;
        }
        else
        {
            template = await _templateRepository.GetByNameAsync(templateName, ct);
        }

        if (template == null) return null;

        var file = template.GetFile(relativePath);
        return file?.Content;
    }
}
