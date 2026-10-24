using ArchGen.Domain.Entities;
using ArchGen.Domain.Enums;
using ArchGen.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace ArchGen.Tests.Domain;

public class ProjectHistoryTests
{
    [Fact]
    public void ProjectHistory_Should_Be_Created_With_Valid_Parameters()
    {
        var history = new ProjectHistory("MinhaApi", @"C:\Projetos\MinhaApi", "API", "Default");

        history.ProjectName.Should().Be("MinhaApi");
        history.ProjectPath.Should().Be(@"C:\Projetos\MinhaApi");
        history.ProjectType.Should().Be("API");
        history.TemplateUsed.Should().Be("Default");
        history.Status.Should().Be(ProjectStatusEnum.Success.ToString());
        history.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void ProjectHistory_Should_Throw_When_ProjectName_Is_Empty(string? invalidName)
    {
        Action act = () => new ProjectHistory(invalidName!, @"C:\Projetos", "API", "Default");
        act.Should().Throw<ValidationException>().WithMessage("*nome do projeto não pode ser vazio*");
    }

    [Theory]
    [InlineData("Meu Projeto")]
    [InlineData("123Projeto")]
    [InlineData("-MeuProjeto")]
    public void ProjectHistory_Should_Throw_When_ProjectName_Is_Invalid_Format(string invalidName)
    {
        Action act = () => new ProjectHistory(invalidName, @"C:\Projetos", "API", "Default");
        act.Should().Throw<ValidationException>().WithMessage("*iniciar com letra ou sublinhado e não conter espaços*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void ProjectHistory_Should_Throw_When_ProjectPath_Is_Empty(string? invalidPath)
    {
        Action act = () => new ProjectHistory("MeuProjeto", invalidPath!, "API", "Default");
        act.Should().Throw<ValidationException>().WithMessage("*caminho de destino do projeto não pode ser vazio*");
    }

    [Theory]
    [InlineData("WEB")]
    [InlineData("DESKTOP")]
    [InlineData("INVALID")]
    public void ProjectHistory_Should_Throw_When_ProjectType_Is_Invalid(string invalidType)
    {
        Action act = () => new ProjectHistory("MeuProjeto", @"C:\Projetos", invalidType, "Default");
        act.Should().Throw<ValidationException>().WithMessage("*Tipo de projeto inválido*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void ProjectHistory_Should_Throw_When_TemplateUsed_Is_Empty(string? invalidTemplate)
    {
        Action act = () => new ProjectHistory("MeuProjeto", @"C:\Projetos", "API", invalidTemplate!);
        act.Should().Throw<ValidationException>().WithMessage("*template utilizado deve ser informado*");
    }

    [Fact]
    public void MarkAsSuccess_Should_Update_Status_And_Details()
    {
        var history = new ProjectHistory("MeuProjeto", @"C:\Projetos", "API", "Default");
        history.MarkAsSuccess("30 arquivos gerados com sucesso.");

        history.Status.Should().Be(ProjectStatusEnum.Success.ToString());
        history.Details.Should().Be("30 arquivos gerados com sucesso.");
    }

    [Fact]
    public void MarkAsFailed_Should_Update_Status_And_Details()
    {
        var history = new ProjectHistory("MeuProjeto", @"C:\Projetos", "API", "Default");
        history.MarkAsFailed("Erro de I/O no disco");

        history.Status.Should().Be(ProjectStatusEnum.Failed.ToString());
        history.Details.Should().Be("Erro de I/O no disco");
    }
}
