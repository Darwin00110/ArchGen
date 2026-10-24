using ArchGen.Domain.Enums;
using ArchGen.InfraStructure.Services;
using FluentAssertions;
using Xunit;

namespace ArchGen.Tests.InfraStructure;

public class DefaultTemplateProviderTests
{
    private readonly DefaultTemplateProvider _provider = new();

    [Fact]
    public void GetDefaultTemplate_Should_Contain_All_Clean_Architecture_Layers()
    {
        var template = _provider.GetDefaultTemplate();

        template.Should().NotBeNull();
        template.Name.Should().Be("Default");
        template.IsDefault.Should().BeTrue();

        var layers = template.Files.Select(f => f.Layer).Distinct().ToList();
        layers.Should().Contain(ProjectLayerEnum.Solution.ToString());
        layers.Should().Contain(ProjectLayerEnum.Domain.ToString());
        layers.Should().Contain(ProjectLayerEnum.Application.ToString());
        layers.Should().Contain(ProjectLayerEnum.InfraStructure.ToString());
        layers.Should().Contain(ProjectLayerEnum.API.ToString());
        layers.Should().Contain(ProjectLayerEnum.Console.ToString());
        layers.Should().Contain(ProjectLayerEnum.Tests.ToString());
    }

    [Fact]
    public void GetDefaultTemplate_Should_Contain_Core_Clean_Architecture_Files()
    {
        var template = _provider.GetDefaultTemplate();

        template.Files.Should().Contain(f => f.RelativePath == "{{PROJECT_NAME}}.sln");
        template.Files.Should().Contain(f => f.RelativePath.Contains("Domain.csproj"));
        template.Files.Should().Contain(f => f.RelativePath.Contains("Application.csproj"));
        template.Files.Should().Contain(f => f.RelativePath.Contains("InfraStructure.csproj"));
        template.Files.Should().Contain(f => f.RelativePath.Contains("API.csproj"));
        template.Files.Should().Contain(f => f.RelativePath.Contains("Console.csproj"));
        template.Files.Should().Contain(f => f.RelativePath.Contains("Tests.csproj"));
        template.Files.Should().Contain(f => f.RelativePath.Contains("User.cs"));
        template.Files.Should().Contain(f => f.RelativePath.Contains("UserUseCase.cs"));
        template.Files.Should().Contain(f => f.RelativePath.Contains("AppDbContext.cs"));
        template.Files.Should().Contain(f => f.RelativePath.Contains("UsersController.cs"));
    }
}
