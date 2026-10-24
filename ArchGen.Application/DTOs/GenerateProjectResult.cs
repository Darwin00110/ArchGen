namespace ArchGen.Application.DTOs;

public class GenerateProjectResult
{
    public bool Success { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string OutputPath { get; set; } = string.Empty;
    public string ProjectType { get; set; } = string.Empty;
    public string TemplateUsed { get; set; } = string.Empty;
    public int GeneratedFilesCount { get; set; }
    public List<string> GeneratedFiles { get; set; } = new();
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
}
