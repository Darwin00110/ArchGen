using ArchGen.Domain.Entities;
using ArchGen.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace ArchGen.Tests.Domain;

public class TemplateDefinitionTests
{
    [Fact]
    public void TemplateDefinition_Should_Be_Created_With_Valid_Parameters()
    {
        var template = new TemplateDefinition("CustomTemplate", "Descrição do template customizado", false);

        template.Name.Should().Be("CustomTemplate");
        template.Description.Should().Be("Descrição do template customizado");
        template.IsDefault.Should().BeFalse();
        template.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        template.Files.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void TemplateDefinition_Should_Throw_When_Name_Is_Empty(string? invalidName)
    {
        Action act = () => new TemplateDefinition(invalidName!, "Descrição");
        act.Should().Throw<ValidationException>().WithMessage("*nome do template não pode ser vazio*");
    }

    [Theory]
    [InlineData("Template Com Espaço")]
    [InlineData("-Template")]
    public void TemplateDefinition_Should_Throw_When_Name_Has_Invalid_Characters(string invalidName)
    {
        Action act = () => new TemplateDefinition(invalidName, "Descrição");
        act.Should().Throw<ValidationException>().WithMessage("*alfanumérico e não conter espaços*");
    }

    [Fact]
    public void Clone_Should_Create_Deep_Copy_With_All_Files()
    {
        var original = new TemplateDefinition("Original", "Original Desc", isDefault: true);
        original.AddOrUpdateFile("src/Domain/Entity.cs", "// code", "Domain");
        original.AddOrUpdateFile("src/API/Program.cs", "// api", "API", isForApi: true, isForConsole: false);

        var cloned = original.Clone("ClonedTemplate", "Novo clone");

        cloned.Name.Should().Be("ClonedTemplate");
        cloned.Description.Should().Be("Novo clone");
        cloned.IsDefault.Should().BeFalse();
        cloned.Files.Should().HaveCount(2);

        // Modifying cloned should not modify original
        cloned.AddOrUpdateFile("src/Domain/Entity.cs", "// modified code", "Domain");
        original.GetFile("src/Domain/Entity.cs")!.Content.Should().Be("// code");
        cloned.GetFile("src/Domain/Entity.cs")!.Content.Should().Be("// modified code");
    }

    [Fact]
    public void Clone_Should_Throw_When_NewName_Is_Invalid()
    {
        var original = new TemplateDefinition("Original", "Desc");
        Action act = () => original.Clone("");
        act.Should().Throw<ValidationException>();
    }

    [Fact]
    public void AddOrUpdateFile_Should_Add_New_File_When_Not_Exists()
    {
        var template = new TemplateDefinition("T1", "Desc");
        var file = template.AddOrUpdateFile("src/Domain/Entity.cs", "// code", "Domain");

        template.Files.Should().ContainSingle();
        file.RelativePath.Should().Be("src/Domain/Entity.cs");
        template.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void AddOrUpdateFile_Should_Update_Existing_File()
    {
        var template = new TemplateDefinition("T1", "Desc");
        template.AddOrUpdateFile("src/Domain/Entity.cs", "// initial", "Domain");

        var updated = template.AddOrUpdateFile("src/Domain/Entity.cs", "// updated", "Domain");

        template.Files.Should().ContainSingle();
        updated.Content.Should().Be("// updated");
    }

    [Fact]
    public void GetFile_Should_Find_Case_Insensitively()
    {
        var template = new TemplateDefinition("T1", "Desc");
        template.AddOrUpdateFile("src/Domain/User.cs", "// code", "Domain");

        var found = template.GetFile(@"SRC\DOMAIN\USER.CS");
        found.Should().NotBeNull();
        found!.Content.Should().Be("// code");
    }

    [Fact]
    public void RemoveFile_Should_Remove_File_And_Return_True_When_Found()
    {
        var template = new TemplateDefinition("T1", "Desc");
        template.AddOrUpdateFile("src/Domain/User.cs", "// code", "Domain");

        var removed = template.RemoveFile("src/Domain/User.cs");
        removed.Should().BeTrue();
        template.Files.Should().BeEmpty();
    }

    [Fact]
    public void RemoveFile_Should_Return_False_When_Not_Found()
    {
        var template = new TemplateDefinition("T1", "Desc");
        var removed = template.RemoveFile("nonexistent.cs");
        removed.Should().BeFalse();
    }
}
