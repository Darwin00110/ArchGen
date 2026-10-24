using ArchGen.Domain.Entities;
using ArchGen.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace ArchGen.Tests.Domain;

public class TemplateFileDefinitionTests
{
    [Fact]
    public void TemplateFileDefinition_Should_Be_Created_With_Valid_Parameters()
    {
        var file = new TemplateFileDefinition("src/Domain/User.cs", "// content", "Domain", true, true);

        file.RelativePath.Should().Be("src/Domain/User.cs");
        file.Content.Should().Be("// content");
        file.Layer.Should().Be("Domain");
        file.IsForApi.Should().BeTrue();
        file.IsForConsole.Should().BeTrue();
    }

    [Fact]
    public void NormalizePath_Should_Convert_Backslashes_And_Trim_Leading_Slash()
    {
        var normalized = TemplateFileDefinition.NormalizePath(@"\src\Domain\User.cs");
        normalized.Should().Be("src/Domain/User.cs");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void TemplateFileDefinition_Should_Throw_When_RelativePath_Is_Empty(string? invalidPath)
    {
        Action act = () => new TemplateFileDefinition(invalidPath!, "// content", "Domain");
        act.Should().Throw<ValidationException>().WithMessage("*caminho relativo do arquivo do template não pode ser vazio*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void TemplateFileDefinition_Should_Throw_When_Layer_Is_Empty(string? invalidLayer)
    {
        Action act = () => new TemplateFileDefinition("src/file.cs", "// content", invalidLayer!);
        act.Should().Throw<ValidationException>().WithMessage("*camada do arquivo do template deve ser especificada*");
    }

    [Fact]
    public void TemplateFileDefinition_Should_Throw_When_Both_IsForApi_And_IsForConsole_Are_False()
    {
        Action act = () => new TemplateFileDefinition("src/file.cs", "// content", "Domain", isForApi: false, isForConsole: false);
        act.Should().Throw<ValidationException>().WithMessage("*pelo menos uma modalidade*");
    }

    [Fact]
    public void UpdateContent_Should_Update_Content_Property()
    {
        var file = new TemplateFileDefinition("src/file.cs", "// old", "Domain");
        file.UpdateContent("// new content");

        file.Content.Should().Be("// new content");
    }
}
