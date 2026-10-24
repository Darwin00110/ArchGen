using ArchGen.Application.DTOs;
using ArchGen.Application.Interfaces;
using ArchGen.Cli;
using ArchGen.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ArchGen.Tests.Cli;

public class CliRunnerTests
{
    private readonly Mock<IGenerateProjectUseCase> _generateUseCaseMock;
    private readonly Mock<ITemplateUseCase> _templateUseCaseMock;
    private readonly Mock<IProjectHistoryUseCase> _historyUseCaseMock;
    private readonly Mock<IHelpUseCase> _helpUseCaseMock;
    private readonly Mock<IFileService> _fileServiceMock;
    private readonly CliRunner _runner;

    public CliRunnerTests()
    {
        _generateUseCaseMock = new Mock<IGenerateProjectUseCase>();
        _templateUseCaseMock = new Mock<ITemplateUseCase>();
        _historyUseCaseMock = new Mock<IProjectHistoryUseCase>();
        _helpUseCaseMock = new Mock<IHelpUseCase>();
        _fileServiceMock = new Mock<IFileService>();

        _runner = new CliRunner(
            _generateUseCaseMock.Object,
            _templateUseCaseMock.Object,
            _historyUseCaseMock.Object,
            _helpUseCaseMock.Object,
            _fileServiceMock.Object);
    }

    [Fact]
    public async Task RunAsync_Should_Call_HelpUseCase_When_Empty_Args()
    {
        _helpUseCaseMock.Setup(h => h.RenderHelpTable()).Returns("Tabela de Ajuda");

        var exitCode = await _runner.RunAsync(Array.Empty<string>());

        exitCode.Should().Be(0);
        _helpUseCaseMock.Verify(h => h.RenderHelpTable(), Times.Once);
    }

    [Theory]
    [InlineData("--help")]
    [InlineData("-h")]
    public async Task RunAsync_Should_Call_HelpUseCase_When_Help_Flag_Passed(string flag)
    {
        _helpUseCaseMock.Setup(h => h.RenderHelpTable()).Returns("Tabela de Ajuda");

        var exitCode = await _runner.RunAsync(new[] { flag });

        exitCode.Should().Be(0);
        _helpUseCaseMock.Verify(h => h.RenderHelpTable(), Times.Once);
    }

    [Fact]
    public async Task RunAsync_Should_Call_GenerateUseCase_When_New_Command_Executed()
    {
        var args = new[] { "new", "MinhaApi", "API", @"C:\Destino", "--template", "Default" };

        _generateUseCaseMock.Setup(g => g.ExecuteAsync(It.IsAny<GenerateProjectRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GenerateProjectResult
            {
                Success = true,
                ProjectName = "MinhaApi",
                OutputPath = @"C:\Destino\MinhaApi",
                GeneratedFilesCount = 20,
                GeneratedFiles = new List<string> { "MinhaApi.sln" },
                Message = "Criado com sucesso"
            });

        var exitCode = await _runner.RunAsync(args);

        exitCode.Should().Be(0);
        _generateUseCaseMock.Verify(g => g.ExecuteAsync(
            It.Is<GenerateProjectRequest>(r => r.ProjectName == "MinhaApi" && r.ProjectType == "API"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RunAsync_Should_Call_TemplateUseCase_When_Template_List_Command_Executed()
    {
        var args = new[] { "template", "list" };

        _templateUseCaseMock.Setup(t => t.GetAllTemplatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TemplateDto>
            {
                new TemplateDto { Name = "Default", IsDefault = true, FilesCount = 26 }
            });

        var exitCode = await _runner.RunAsync(args);

        exitCode.Should().Be(0);
        _templateUseCaseMock.Verify(t => t.GetAllTemplatesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RunAsync_Should_Call_HistoryUseCase_When_History_Command_Executed()
    {
        var args = new[] { "history" };

        _historyUseCaseMock.Setup(h => h.GetAllHistoryAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProjectHistoryDto>
            {
                new ProjectHistoryDto { Id = 1, ProjectName = "Proj1", ProjectPath = @"C:\Proj1", ProjectType = "API", TemplateUsed = "Default", CreatedAt = DateTime.UtcNow, Status = "Success" }
            });

        var exitCode = await _runner.RunAsync(args);

        exitCode.Should().Be(0);
        _historyUseCaseMock.Verify(h => h.GetAllHistoryAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
