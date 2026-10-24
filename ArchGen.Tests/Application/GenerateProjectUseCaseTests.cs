using ArchGen.Application.DTOs;
using ArchGen.Application.UseCases;
using ArchGen.Domain.Entities;
using ArchGen.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ArchGen.Tests.Application;

public class GenerateProjectUseCaseTests
{
    private readonly Mock<ITemplateRepository> _templateRepoMock;
    private readonly Mock<IDefaultTemplateProvider> _defaultTemplateProviderMock;
    private readonly Mock<IProjectHistoryRepository> _historyRepoMock;
    private readonly Mock<IFileService> _fileServiceMock;
    private readonly GenerateProjectUseCase _useCase;

    private readonly TemplateDefinition _sampleDefaultTemplate;

    public GenerateProjectUseCaseTests()
    {
        _templateRepoMock = new Mock<ITemplateRepository>();
        _defaultTemplateProviderMock = new Mock<IDefaultTemplateProvider>();
        _historyRepoMock = new Mock<IProjectHistoryRepository>();
        _fileServiceMock = new Mock<IFileService>();

        _sampleDefaultTemplate = new TemplateDefinition("Default", "Default desc", isDefault: true);
        _sampleDefaultTemplate.AddOrUpdateFile("{{PROJECT_NAME}}.sln", "Solution {{PROJECT_NAME}}", "Solution", true, true);
        _sampleDefaultTemplate.AddOrUpdateFile("src/{{PROJECT_NAME}}.Domain/User.cs", "namespace {{PROJECT_NAME}}.Domain;", "Domain", true, true);
        _sampleDefaultTemplate.AddOrUpdateFile("src/{{PROJECT_NAME}}.API/Program.cs", "API Program for {{PROJECT_NAME}}", "API", isForApi: true, isForConsole: false);
        _sampleDefaultTemplate.AddOrUpdateFile("src/{{PROJECT_NAME}}.Console/Program.cs", "Console Program for {{PROJECT_NAME}}", "Console", isForApi: false, isForConsole: true);

        _defaultTemplateProviderMock.Setup(p => p.GetDefaultTemplate()).Returns(_sampleDefaultTemplate);
        _fileServiceMock.Setup(f => f.GetCurrentDirectory()).Returns(@"C:\Work");
        _fileServiceMock.Setup(f => f.Combine(It.IsAny<string[]>())).Returns((string[] parts) => string.Join(@"\", parts));
        _fileServiceMock.Setup(f => f.GetDirectoryName(It.IsAny<string>())).Returns(@"C:\Work\MyProject\src");

        _useCase = new GenerateProjectUseCase(
            _templateRepoMock.Object,
            _defaultTemplateProviderMock.Object,
            _historyRepoMock.Object,
            _fileServiceMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Generate_Project_With_Default_Template_API()
    {
        var request = new GenerateProjectRequest
        {
            ProjectName = "MeuApp",
            ProjectType = "API",
            TargetPath = @"C:\Projetos",
            TemplateName = "Default"
        };

        _fileServiceMock.Setup(f => f.DirectoryExists(@"C:\Projetos\MeuApp")).Returns(false);

        var result = await _useCase.ExecuteAsync(request);

        result.Success.Should().BeTrue();
        result.ProjectName.Should().Be("MeuApp");
        result.ProjectType.Should().Be("API");
        result.TemplateUsed.Should().Be("Default");
        result.GeneratedFilesCount.Should().Be(3); // sln, domain, api (console is excluded)

        // Verify files written
        _fileServiceMock.Verify(f => f.WriteAllTextAsync(
            It.Is<string>(p => p.Contains("User.cs")),
            It.Is<string>(c => c.Contains("MeuApp.Domain")),
            It.IsAny<CancellationToken>()), Times.Once);

        // Verify history saved
        _historyRepoMock.Verify(h => h.AddAsync(
            It.Is<ProjectHistory>(p => p.ProjectName == "MeuApp" && p.Status == "Success"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Generate_Project_With_Console_Type_Filtering_Files()
    {
        var request = new GenerateProjectRequest
        {
            ProjectName = "MeuConsoleApp",
            ProjectType = "CONSOLE",
            TargetPath = @"C:\Projetos",
            TemplateName = "Default"
        };

        _fileServiceMock.Setup(f => f.DirectoryExists(@"C:\Projetos\MeuConsoleApp")).Returns(false);

        var result = await _useCase.ExecuteAsync(request);

        result.Success.Should().BeTrue();
        result.ProjectType.Should().Be("CONSOLE");
        result.GeneratedFilesCount.Should().Be(3); // sln, domain, console (api is excluded)

        _fileServiceMock.Verify(f => f.WriteAllTextAsync(
            It.Is<string>(p => p.EndsWith("Program.cs") && p.Contains("Console")),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);

        _fileServiceMock.Verify(f => f.WriteAllTextAsync(
            It.Is<string>(p => p.Contains("API")),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Use_Custom_Template_From_Repository()
    {
        var customTemplate = new TemplateDefinition("MeuTemplateCustom", "Custom", isDefault: false);
        customTemplate.AddOrUpdateFile("src/Custom.cs", "// Custom File for {{PROJECT_NAME}}", "Custom", true, true);

        _templateRepoMock.Setup(r => r.GetByNameAsync("MeuTemplateCustom", It.IsAny<CancellationToken>()))
            .ReturnsAsync(customTemplate);

        var request = new GenerateProjectRequest
        {
            ProjectName = "CustomProject",
            ProjectType = "API",
            TemplateName = "MeuTemplateCustom"
        };

        var result = await _useCase.ExecuteAsync(request);

        result.Success.Should().BeTrue();
        result.TemplateUsed.Should().Be("MeuTemplateCustom");
        result.GeneratedFilesCount.Should().Be(1);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Fail_When_ProjectName_Is_Invalid()
    {
        var request = new GenerateProjectRequest
        {
            ProjectName = "Nome Invalido Com Espaco",
            ProjectType = "API"
        };

        var result = await _useCase.ExecuteAsync(request);

        result.Success.Should().BeFalse();
        result.Message.Should().Contain("inválido");

        _historyRepoMock.Verify(h => h.AddAsync(
            It.Is<ProjectHistory>(p => p.Status == "Failed"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Fail_When_Target_Directory_Already_Has_Files()
    {
        var request = new GenerateProjectRequest
        {
            ProjectName = "AppExistente",
            ProjectType = "API",
            TargetPath = @"C:\Projetos"
        };

        _fileServiceMock.Setup(f => f.DirectoryExists(@"C:\Projetos\AppExistente")).Returns(true);
        _fileServiceMock.Setup(f => f.GetFiles(@"C:\Projetos\AppExistente", "*.*", It.IsAny<SearchOption>()))
            .Returns(new[] { @"C:\Projetos\AppExistente\file.txt" });

        var result = await _useCase.ExecuteAsync(request);

        result.Success.Should().BeFalse();
        result.Message.Should().Contain("já existe e contém arquivos");
    }

    [Fact]
    public async Task ExecuteAsync_Should_Fail_When_Template_Is_Not_Found()
    {
        var request = new GenerateProjectRequest
        {
            ProjectName = "AppInexistente",
            ProjectType = "API",
            TemplateName = "NaoExiste"
        };

        _templateRepoMock.Setup(r => r.GetByNameAsync("NaoExiste", It.IsAny<CancellationToken>()))
            .ReturnsAsync((TemplateDefinition?)null);

        var result = await _useCase.ExecuteAsync(request);

        result.Success.Should().BeFalse();
        result.Message.Should().Contain("não foi encontrado");
    }
}
