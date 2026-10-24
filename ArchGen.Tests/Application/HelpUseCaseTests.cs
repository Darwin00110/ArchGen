using ArchGen.Application.UseCases;
using FluentAssertions;
using Xunit;

namespace ArchGen.Tests.Application;

public class HelpUseCaseTests
{
    private readonly HelpUseCase _helpUseCase = new();

    [Fact]
    public void GetHelpItems_Should_Return_All_Documented_Commands()
    {
        var items = _helpUseCase.GetHelpItems();

        items.Should().NotBeEmpty();
        items.Should().Contain(i => i.Command.Contains("Ajuda"));
        items.Should().Contain(i => i.Command.Contains("Gerar Projeto"));
        items.Should().Contain(i => i.Command.Contains("Listar Templates"));
        items.Should().Contain(i => i.Command.Contains("Template"));
        items.Should().Contain(i => i.Command.Contains("Criar Template"));
        items.Should().Contain(i => i.Command.Contains("Arquivo"));
        items.Should().Contain(i => i.Command.Contains("Excluir Template"));
        items.Should().Contain(i => i.Command.Contains("Histórico"));
    }

    [Fact]
    public void RenderHelpTable_Should_Return_Formatted_Table_With_Headers_And_Commands()
    {
        var table = _helpUseCase.RenderHelpTable();

        table.Should().NotBeNullOrWhiteSpace();
        table.Should().Contain("ARCHGEN - GERADOR DE ARQUITETURA LIMPA EM C#");
        table.Should().Contain("Comando");
        table.Should().Contain("Sintaxe");
        table.Should().Contain("Descrição");
        table.Should().Contain("Exemplo de Uso");
        table.Should().Contain("ArchGen --help");
        table.Should().Contain("ArchGen new");
        table.Should().Contain("ArchGen template");
        table.Should().Contain("ArchGen history");
    }

    [Fact]
    public void RenderHelpTable_Should_Have_Exact_Defined_Width_For_All_Table_Lines()
    {
        const int expectedWidth = HelpUseCase.DefaultTableWidth; // 120
        var table = _helpUseCase.RenderHelpTable();

        var lines = table.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        // Find the lines of the table (from top border '╔' to bottom border '╚')
        var tableLines = lines.SkipWhile(l => !l.StartsWith("╔"))
                              .TakeWhile(l => !l.StartsWith("╚"))
                              .ToList();

        // Include the bottom border line '╚'
        var bottomBorderLine = lines.FirstOrDefault(l => l.StartsWith("╚"));
        bottomBorderLine.Should().NotBeNull();
        tableLines.Add(bottomBorderLine!);

        tableLines.Should().NotBeEmpty();

        // Every line in the rendered table must strictly match expectedWidth
        foreach (var line in tableLines)
        {
            line.Length.Should().Be(expectedWidth, $"line '{line}' should have width {expectedWidth}");
        }
    }

    [Theory]
    [InlineData(110, 2)]
    [InlineData(120, 3)]
    [InlineData(130, 4)]
    public void RenderHelpTable_With_Custom_Width_And_Height_Should_Respect_Defined_Dimensions(int customWidth, int customRowHeight)
    {
        var table = _helpUseCase.RenderHelpTable(customWidth, customRowHeight);

        var lines = table.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        var tableLines = lines.SkipWhile(l => !l.StartsWith("╔"))
                              .TakeWhile(l => !l.StartsWith("╚"))
                              .ToList();
        var bottomBorderLine = lines.FirstOrDefault(l => l.StartsWith("╚"));
        bottomBorderLine.Should().NotBeNull();
        tableLines.Add(bottomBorderLine!);

        foreach (var line in tableLines)
        {
            line.Length.Should().Be(customWidth);
        }
    }
}
