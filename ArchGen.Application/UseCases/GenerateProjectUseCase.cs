using ArchGen.Application.DTOs;
using ArchGen.Application.Interfaces;
using ArchGen.Domain.Entities;
using ArchGen.Domain.Enums;
using ArchGen.Domain.Exceptions;
using ArchGen.Domain.Interfaces;

namespace ArchGen.Application.UseCases;

public class GenerateProjectUseCase : IGenerateProjectUseCase
{
    private readonly ITemplateRepository _templateRepository;
    private readonly IDefaultTemplateProvider _defaultTemplateProvider;
    private readonly IProjectHistoryRepository _historyRepository;
    private readonly IFileService _fileService;

    public GenerateProjectUseCase(
        ITemplateRepository templateRepository,
        IDefaultTemplateProvider defaultTemplateProvider,
        IProjectHistoryRepository historyRepository,
        IFileService _fileService)
    {
        _templateRepository = templateRepository;
        _defaultTemplateProvider = defaultTemplateProvider;
        _historyRepository = historyRepository;
        this._fileService = _fileService;
    }

    public async Task<GenerateProjectResult> ExecuteAsync(GenerateProjectRequest request, CancellationToken ct = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var projectName = request.ProjectName?.Trim() ?? string.Empty;
        var projectType = string.IsNullOrWhiteSpace(request.ProjectType) ? "API" : request.ProjectType.Trim().ToUpperInvariant();
        var templateName = string.IsNullOrWhiteSpace(request.TemplateName) ? "Default" : request.TemplateName.Trim();

        var basePath = string.IsNullOrWhiteSpace(request.TargetPath) ? _fileService.GetCurrentDirectory() : request.TargetPath.Trim();
        var destinationDir = _fileService.Combine(basePath, projectName);

        // Domain history entity to record the attempt
        var history = new ProjectHistory
        {
            ProjectName = projectName,
            ProjectPath = destinationDir,
            ProjectType = projectType,
            TemplateUsed = templateName,
            CreatedAt = DateTime.UtcNow,
            Status = ProjectStatusEnum.Failed.ToString()
        };

        try
        {
            // Validation
            if (string.IsNullOrWhiteSpace(projectName))
                throw new ValidationException("O nome do projeto não pode ser vazio.");

            if (projectName.Contains(' ') || (!char.IsLetter(projectName[0]) && projectName[0] != '_'))
                throw new ValidationException($"Nome de projeto inválido: '{projectName}'. O nome deve iniciar com letra ou sublinhado e não conter espaços.");

            if (projectType != ProjectTypeEnum.API.ToString() && projectType != ProjectTypeEnum.CONSOLE.ToString())
                throw new ValidationException($"Tipo de projeto inválido: '{projectType}'. Os tipos válidos são 'API' ou 'CONSOLE'.");

            if (_fileService.DirectoryExists(destinationDir) && _fileService.GetFiles(destinationDir, "*.*").Any())
            {
                throw new ValidationException($"O diretório de destino '{destinationDir}' já existe e contém arquivos. Escolha outro nome ou diretório.");
            }

            // Resolve Template
            TemplateDefinition? template = null;
            var defaultTemplate = _defaultTemplateProvider.GetDefaultTemplate();

            if (templateName.Equals(defaultTemplate.Name, StringComparison.OrdinalIgnoreCase))
            {
                // Check if there is an override in database
                template = await _templateRepository.GetByNameAsync(templateName, ct) ?? defaultTemplate;
            }
            else
            {
                template = await _templateRepository.GetByNameAsync(templateName, ct);
                if (template == null)
                    throw new TemplateNotFoundException(templateName);
            }

            // Filter files for API or CONSOLE
            var isApi = projectType == ProjectTypeEnum.API.ToString();
            var targetFiles = template.Files
                .Where(f => isApi ? f.IsForApi : f.IsForConsole)
                .ToList();

            if (targetFiles.Count == 0)
                throw new ProjectGenerationException($"O template '{template.Name}' não possui arquivos compatíveis com o tipo de projeto '{projectType}'.");

            // Ensure destination directory
            _fileService.CreateDirectory(destinationDir);

            // Tokens dictionary
            var domainGuid = Guid.NewGuid().ToString("B").ToUpperInvariant();
            var appGuid = Guid.NewGuid().ToString("B").ToUpperInvariant();
            var infraGuid = Guid.NewGuid().ToString("B").ToUpperInvariant();
            var presentationGuid = Guid.NewGuid().ToString("B").ToUpperInvariant();
            var testsGuid = Guid.NewGuid().ToString("B").ToUpperInvariant();

            var replacements = new Dictionary<string, string>
            {
                { "{{PROJECT_NAME}}", projectName },
                { "{{SAFE_PROJECT_NAME}}", projectName.Replace("-", "_") },
                { "{{PROJECT_TYPE}}", projectType },
                { "{{YEAR}}", DateTime.UtcNow.Year.ToString() },
                { "{{DATE}}", DateTime.UtcNow.ToString("yyyy-MM-dd") },
                { "{{DOMAIN_GUID}}", domainGuid },
                { "{{APPLICATION_GUID}}", appGuid },
                { "{{INFRASTRUCTURE_GUID}}", infraGuid },
                { "{{PRESENTATION_GUID}}", presentationGuid },
                { "{{API_GUID}}", presentationGuid },
                { "{{CONSOLE_GUID}}", presentationGuid },
                { "{{TESTS_GUID}}", testsGuid },
            };

            var generatedFiles = new List<string>();

            foreach (var templateFile in targetFiles)
            {
                var relativePath = ReplaceTokens(templateFile.RelativePath, replacements);
                var content = ReplaceTokens(templateFile.Content, replacements);

                var fullFilePath = _fileService.Combine(destinationDir, relativePath);
                var dir = _fileService.GetDirectoryName(fullFilePath);

                if (!string.IsNullOrWhiteSpace(dir) && !_fileService.DirectoryExists(dir))
                {
                    _fileService.CreateDirectory(dir);
                }

                await _fileService.WriteAllTextAsync(fullFilePath, content, ct);
                generatedFiles.Add(relativePath);
            }

            // Save history success
            history.MarkAsSuccess($"Solução gerada com sucesso. {generatedFiles.Count} arquivos criados.");
            await _historyRepository.AddAsync(history, ct);

            return new GenerateProjectResult
            {
                Success = true,
                ProjectName = projectName,
                OutputPath = destinationDir,
                ProjectType = projectType,
                TemplateUsed = template.Name,
                GeneratedFilesCount = generatedFiles.Count,
                GeneratedFiles = generatedFiles,
                Message = $"Projeto '{projectName}' em Clean Architecture ({projectType}) gerado com sucesso em '{destinationDir}'!"
            };
        }
        catch (Exception ex)
        {
            history.MarkAsFailed(ex.Message);
            try
            {
                await _historyRepository.AddAsync(history, ct);
            }
            catch
            {
                // Ignore history recording errors if db fails
            }

            return new GenerateProjectResult
            {
                Success = false,
                ProjectName = projectName,
                OutputPath = destinationDir,
                ProjectType = projectType,
                TemplateUsed = templateName,
                Message = $"Erro ao gerar projeto: {ex.Message}",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    private static string ReplaceTokens(string source, Dictionary<string, string> replacements)
    {
        if (string.IsNullOrEmpty(source)) return string.Empty;

        var result = source;
        foreach (var pair in replacements)
        {
            result = result.Replace(pair.Key, pair.Value);
        }

        return result;
    }
}
