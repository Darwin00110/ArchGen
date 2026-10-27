using ArchGen.Domain.Enums;
using ArchGen.Domain.Exceptions;

namespace ArchGen.Domain.Entities;

public class ProjectHistory
{
    public Guid Id { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string ProjectPath { get; set; } = string.Empty;
    public string ProjectType { get; set; } = string.Empty;
    public string TemplateUsed { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = ProjectStatusEnum.Success.ToString();
    public string? Details { get; set; }

    public ProjectHistory() { }

    public ProjectHistory(string projectName, string projectPath, string projectType, string templateUsed)
    {
        ProjectName = projectName;
        ProjectPath = projectPath;
        ProjectType = projectType;
        TemplateUsed = templateUsed;
        CreatedAt = DateTime.UtcNow;
        Status = ProjectStatusEnum.Success.ToString();
        Validate();
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(ProjectName))
            throw new ValidationException("O nome do projeto não pode ser vazio.");

        if (ProjectName.Contains(' ') || !char.IsLetter(ProjectName[0]) && ProjectName[0] != '_')
            throw new ValidationException("O nome do projeto deve iniciar com letra ou sublinhado e não conter espaços.");

        if (string.IsNullOrWhiteSpace(ProjectPath))
            throw new ValidationException("O caminho de destino do projeto não pode ser vazio.");

        if (ProjectType != ProjectTypeEnum.API.ToString() && ProjectType != ProjectTypeEnum.CONSOLE.ToString())
            throw new ValidationException($"Tipo de projeto inválido: '{ProjectType}'. Use 'API' ou 'CONSOLE'.");

        if (string.IsNullOrWhiteSpace(TemplateUsed))
            throw new ValidationException("O template utilizado deve ser informado.");
    }

    public void MarkAsSuccess(string? details = null)
    {
        Status = ProjectStatusEnum.Success.ToString();
        Details = details;
    }

    public void MarkAsFailed(string errorMessage)
    {
        Status = ProjectStatusEnum.Failed.ToString();
        Details = errorMessage;
    }
}
