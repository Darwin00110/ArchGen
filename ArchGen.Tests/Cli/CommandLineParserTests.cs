using ArchGen.Cli;
using FluentAssertions;
using Xunit;

namespace ArchGen.Tests.Cli;

public class CommandLineParserTests
{
    [Fact]
    public void Parse_Should_Return_Help_Flag_When_Args_Are_Empty()
    {
        var result = CommandLineParser.Parse(Array.Empty<string>());
        result.HasFlag("--help").Should().BeTrue();
    }

    [Theory]
    [InlineData("--help")]
    [InlineData("-h")]
    public void Parse_Should_Detect_Help_Flags(string flag)
    {
        var result = CommandLineParser.Parse(new[] { flag });
        result.HasFlag(flag).Should().BeTrue();
    }

    [Fact]
    public void Parse_Should_Parse_New_Project_Command_Correctly()
    {
        var args = new[] { "new", "MeuApp", "API", @"C:\Projetos", "--template", "MeuTemplate" };
        var result = CommandLineParser.Parse(args);

        result.Command.Should().Be("new");
        result.SubCommand.Should().Be("MeuApp");
        result.PositionalArgs.Should().Contain(new[] { "API", @"C:\Projetos" });
        result.GetNamed("template").Should().Be("MeuTemplate");
    }

    [Fact]
    public void Parse_Should_Parse_Template_EditFile_Command_Correctly()
    {
        var args = new[] { "template", "edit-file", "MeuTemplate", "--file", "Domain/User.cs", "--content", "class User {}" };
        var result = CommandLineParser.Parse(args);

        result.Command.Should().Be("template");
        result.SubCommand.Should().Be("edit-file");
        result.PositionalArgs.Should().Contain("MeuTemplate");
        result.GetNamed("file").Should().Be("Domain/User.cs");
        result.GetNamed("content").Should().Be("class User {}");
    }
}
