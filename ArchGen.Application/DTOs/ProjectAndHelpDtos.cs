namespace ArchGen.Application.DTOs;

public class ProjectHistoryDto
{
    public int Id { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string ProjectPath { get; set; } = string.Empty;
    public string ProjectType { get; set; } = string.Empty;
    public string TemplateUsed { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Details { get; set; }
}

public class HelpItemDto
{
    public string Command { get; set; } = string.Empty;
    public string Syntax { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Options { get; set; } = string.Empty;
    public string Example { get; set; } = string.Empty;
}
