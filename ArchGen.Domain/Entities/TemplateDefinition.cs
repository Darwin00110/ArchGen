using ArchGen.Domain.Exceptions;

namespace ArchGen.Domain.Entities;

public class TemplateDefinition
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public List<TemplateFileDefinition> Files { get; set; } = new();

    public TemplateDefinition() { }

    public TemplateDefinition(string name, string description, bool isDefault = false)
    {
        Name = name;
        Description = description;
        IsDefault = isDefault;
        CreatedAt = DateTime.UtcNow;
        Validate();
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ValidationException("O nome do template não pode ser vazio.");

        if (Name.Contains(' ') || (!char.IsLetterOrDigit(Name[0]) && Name[0] != '_'))
            throw new ValidationException("O nome do template deve ser alfanumérico e não conter espaços.");
    }

    public TemplateDefinition Clone(string newName, string? newDescription = null)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ValidationException("O novo nome do template clonado não pode ser vazio.");

        var cloned = new TemplateDefinition
        {
            Name = newName.Trim(),
            Description = newDescription ?? $"Clonado a partir de {Name}",
            IsDefault = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Files = new List<TemplateFileDefinition>()
        };

        cloned.Validate();

        foreach (var file in Files)
        {
            cloned.Files.Add(new TemplateFileDefinition
            {
                RelativePath = file.RelativePath,
                Content = file.Content,
                Layer = file.Layer,
                IsForApi = file.IsForApi,
                IsForConsole = file.IsForConsole
            });
        }

        return cloned;
    }

    public TemplateFileDefinition? GetFile(string relativePath)
    {
        var normalized = TemplateFileDefinition.NormalizePath(relativePath);
        return Files.FirstOrDefault(f => string.Equals(f.RelativePath, normalized, StringComparison.OrdinalIgnoreCase));
    }

    public TemplateFileDefinition AddOrUpdateFile(string relativePath, string content, string layer = "Custom", bool isForApi = true, bool isForConsole = true)
    {
        var existing = GetFile(relativePath);
        if (existing != null)
        {
            existing.UpdateContent(content);
            if (!string.IsNullOrWhiteSpace(layer) && layer != "Custom")
                existing.Layer = layer;
            existing.IsForApi = isForApi;
            existing.IsForConsole = isForConsole;
            UpdatedAt = DateTime.UtcNow;
            return existing;
        }

        var newFile = new TemplateFileDefinition(relativePath, content, layer, isForApi, isForConsole);
        Files.Add(newFile);
        UpdatedAt = DateTime.UtcNow;
        return newFile;
    }

    public bool RemoveFile(string relativePath)
    {
        var existing = GetFile(relativePath);
        if (existing == null) return false;

        Files.Remove(existing);
        UpdatedAt = DateTime.UtcNow;
        return true;
    }
}
