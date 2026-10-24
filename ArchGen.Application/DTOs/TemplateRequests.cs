namespace ArchGen.Application.DTOs;

public class CreateTemplateRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? BaseTemplateName { get; set; }
}

public class UpdateTemplateFileRequest
{
    public string TemplateName { get; set; } = string.Empty;
    public string RelativePath { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Layer { get; set; }
    public bool? IsForApi { get; set; }
    public bool? IsForConsole { get; set; }
}
