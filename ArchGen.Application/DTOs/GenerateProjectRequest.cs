namespace ArchGen.Application.DTOs;

public class GenerateProjectRequest
{
    public string ProjectName { get; set; } = string.Empty;
    public string? TargetPath { get; set; }
    public string ProjectType { get; set; } = "API";
    public string? TemplateName { get; set; }
}
