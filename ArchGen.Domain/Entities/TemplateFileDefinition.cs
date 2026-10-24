using ArchGen.Domain.Exceptions;

namespace ArchGen.Domain.Entities;

public class TemplateFileDefinition
{
    public int Id { get; set; }
    public int TemplateDefinitionId { get; set; }
    public TemplateDefinition? Template { get; set; }

    public string RelativePath { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Layer { get; set; } = string.Empty;
    public bool IsForApi { get; set; } = true;
    public bool IsForConsole { get; set; } = true;

    public TemplateFileDefinition() { }

    public TemplateFileDefinition(string relativePath, string content, string layer, bool isForApi = true, bool isForConsole = true)
    {
        RelativePath = NormalizePath(relativePath);
        Content = content;
        Layer = layer;
        IsForApi = isForApi;
        IsForConsole = isForConsole;
        Validate();
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(RelativePath))
            throw new ValidationException("O caminho relativo do arquivo do template não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(Layer))
            throw new ValidationException("A camada do arquivo do template deve ser especificada.");

        if (!IsForApi && !IsForConsole)
            throw new ValidationException("O arquivo do template deve ser aplicável para pelo menos uma modalidade (API ou CONSOLE).");
    }

    public void UpdateContent(string newContent)
    {
        Content = newContent ?? string.Empty;
    }

    public static string NormalizePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return string.Empty;
        return path.Replace('\\', '/').Trim().TrimStart('/');
    }
}
