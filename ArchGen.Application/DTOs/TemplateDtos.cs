namespace ArchGen.Application.DTOs;

public class TemplateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int FilesCount { get; set; }
}

public class TemplateDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<TemplateFileDto> Files { get; set; } = new();
}

public class TemplateFileDto
{
    public int Id { get; set; }
    public string RelativePath { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Layer { get; set; } = string.Empty;
    public bool IsForApi { get; set; }
    public bool IsForConsole { get; set; }
}
