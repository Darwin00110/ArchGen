using ArchGen.Application.DTOs;
using ArchGen.Application.UseCases;
using ArchGen.Domain.Entities;
using ArchGen.Domain.Exceptions;
using ArchGen.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ArchGen.Tests.Application;

public class TemplateUseCaseTests
{
    private readonly Mock<ITemplateRepository> _templateRepoMock;
    private readonly Mock<IDefaultTemplateProvider> _defaultTemplateProviderMock;
    private readonly TemplateUseCase _useCase;
    private readonly TemplateDefinition _sampleDefaultTemplate;

    public TemplateUseCaseTests()
    {
        _templateRepoMock = new Mock<ITemplateRepository>();
        _defaultTemplateProviderMock = new Mock<IDefaultTemplateProvider>();

        _sampleDefaultTemplate = new TemplateDefinition("Default", "Template padrão", isDefault: true);
        _sampleDefaultTemplate.AddOrUpdateFile("src/Domain/User.cs", "// User code", "Domain");

        _defaultTemplateProviderMock.Setup(p => p.GetDefaultTemplate()).Returns(_sampleDefaultTemplate);

        _useCase = new TemplateUseCase(_templateRepoMock.Object, _defaultTemplateProviderMock.Object);
    }

    [Fact]
    public async Task GetAllTemplatesAsync_Should_Return_Default_And_Db_Templates()
    {
        var dbTemplate = new TemplateDefinition("CustomDb", "Custom", isDefault: false);
        dbTemplate.AddOrUpdateFile("file.cs", "//", "Custom");

        _templateRepoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TemplateDefinition> { dbTemplate });

        var list = await _useCase.GetAllTemplatesAsync();

        list.Should().HaveCount(2);
        list.Should().Contain(t => t.Name == "Default" && t.IsDefault);
        list.Should().Contain(t => t.Name == "CustomDb" && !t.IsDefault);
    }

    [Fact]
    public async Task GetTemplateDetailsAsync_Should_Return_Default_Template_When_Requested()
    {
        var details = await _useCase.GetTemplateDetailsAsync("Default");

        details.Should().NotBeNull();
        details!.Name.Should().Be("Default");
        details.Files.Should().ContainSingle(f => f.RelativePath == "src/Domain/User.cs");
    }

    [Fact]
    public async Task GetTemplateDetailsAsync_Should_Return_Custom_Template_From_Repo()
    {
        var custom = new TemplateDefinition("MinhaCustom", "Desc", false);
        custom.AddOrUpdateFile("src/App.cs", "// code", "App");

        _templateRepoMock.Setup(r => r.GetByNameAsync("MinhaCustom", It.IsAny<CancellationToken>()))
            .ReturnsAsync(custom);

        var details = await _useCase.GetTemplateDetailsAsync("MinhaCustom");

        details.Should().NotBeNull();
        details!.Name.Should().Be("MinhaCustom");
        details.Files.Should().ContainSingle(f => f.RelativePath == "src/App.cs");
    }

    [Fact]
    public async Task CreateTemplateAsync_Should_Clone_Default_And_Save_To_Repository()
    {
        _templateRepoMock.Setup(r => r.ExistsByNameAsync("NovoTemplate", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var request = new CreateTemplateRequest
        {
            Name = "NovoTemplate",
            BaseTemplateName = "Default",
            Description = "Novo template clonado"
        };

        var result = await _useCase.CreateTemplateAsync(request);

        result.Should().NotBeNull();
        result.Name.Should().Be("NovoTemplate");
        result.FilesCount.Should().Be(1);

        _templateRepoMock.Verify(r => r.AddAsync(
            It.Is<TemplateDefinition>(t => t.Name == "NovoTemplate" && t.Files.Count == 1),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateTemplateAsync_Should_Throw_When_Template_Name_Is_Reserved_Default()
    {
        var request = new CreateTemplateRequest { Name = "Default" };

        Func<Task> act = async () => await _useCase.CreateTemplateAsync(request);
        await act.Should().ThrowAsync<ValidationException>().WithMessage("*reservado*");
    }

    [Fact]
    public async Task CreateTemplateAsync_Should_Throw_When_Template_Already_Exists()
    {
        _templateRepoMock.Setup(r => r.ExistsByNameAsync("Existente", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var request = new CreateTemplateRequest { Name = "Existente" };

        Func<Task> act = async () => await _useCase.CreateTemplateAsync(request);
        await act.Should().ThrowAsync<ValidationException>().WithMessage("*Já existe um template*");
    }

    [Fact]
    public async Task UpdateTemplateFileAsync_Should_Throw_When_Modifying_Default_Template()
    {
        var request = new UpdateTemplateFileRequest
        {
            TemplateName = "Default",
            RelativePath = "src/Domain/User.cs",
            Content = "// modified"
        };

        Func<Task> act = async () => await _useCase.UpdateTemplateFileAsync(request);
        await act.Should().ThrowAsync<ValidationException>().WithMessage("*somente leitura*");
    }

    [Fact]
    public async Task UpdateTemplateFileAsync_Should_Update_File_In_Custom_Template_And_Save()
    {
        var custom = new TemplateDefinition("MeuTemplate", "Desc", false);
        custom.AddOrUpdateFile("src/Domain/User.cs", "// old code", "Domain");

        _templateRepoMock.Setup(r => r.GetByNameAsync("MeuTemplate", It.IsAny<CancellationToken>()))
            .ReturnsAsync(custom);

        var request = new UpdateTemplateFileRequest
        {
            TemplateName = "MeuTemplate",
            RelativePath = "src/Domain/User.cs",
            Content = "// new custom code"
        };

        var result = await _useCase.UpdateTemplateFileAsync(request);

        result.RelativePath.Should().Be("src/Domain/User.cs");
        result.Content.Should().Be("// new custom code");

        _templateRepoMock.Verify(r => r.UpdateAsync(
            It.Is<TemplateDefinition>(t => t.Name == "MeuTemplate"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTemplateAsync_Should_Throw_When_Attempting_To_Delete_Default()
    {
        Func<Task> act = async () => await _useCase.DeleteTemplateAsync("Default");
        await act.Should().ThrowAsync<ValidationException>().WithMessage("*não pode ser excluído*");
    }

    [Fact]
    public async Task DeleteTemplateAsync_Should_Delete_Custom_Template()
    {
        var custom = new TemplateDefinition("ParaExcluir", "Desc", false);

        _templateRepoMock.Setup(r => r.GetByNameAsync("ParaExcluir", It.IsAny<CancellationToken>()))
            .ReturnsAsync(custom);

        var result = await _useCase.DeleteTemplateAsync("ParaExcluir");

        result.Should().BeTrue();
        _templateRepoMock.Verify(r => r.DeleteAsync(custom, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetFileContentAsync_Should_Return_Content_When_Found()
    {
        var content = await _useCase.GetFileContentAsync("Default", "src/Domain/User.cs");
        content.Should().Be("// User code");
    }
}
