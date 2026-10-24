using System.Text;
using ArchGen.Application.DTOs;
using ArchGen.Application.Interfaces;

namespace ArchGen.Application.UseCases;

public class HelpUseCase : IHelpUseCase
{
    public const int DefaultTableWidth = 120;
    public const int DefaultRowHeight = 3;

    public List<HelpItemDto> GetHelpItems()
    {
        return new List<HelpItemDto>
        {
            new()
            {
                Command = "Ajuda Geral",
                Syntax = "ArchGen --help (ou -h)",
                Description = "Exibe a tabela de ajuda completa com comandos, sintaxe e exemplos.",
                Options = "--help, -h",
                Example = "ArchGen --help"
            },
            new()
            {
                Command = "Gerar Projeto",
                Syntax = "ArchGen new <Nome> [Tipo] [Path] [Opções]",
                Description = "Gera solução C# em Clean Arch (.NET 8.0) com Domain, App, Infra, API/Console, Tests e sln.",
                Options = "API | CONSOLE (padrão: API)\n--template <nome>",
                Example = "ArchGen new MeuApp API\nArchGen new MeuCli CONSOLE"
            },
            new()
            {
                Command = "Listar Templates",
                Syntax = "ArchGen template list",
                Description = "Lista todos os templates disponíveis (padrão e os salvos no banco SQLite).",
                Options = "Nenhuma",
                Example = "ArchGen template list"
            },
            new()
            {
                Command = "Ver Template",
                Syntax = "ArchGen template view <Nome> [--file <Caminho>]",
                Description = "Exibe os arquivos do template ou o conteúdo do arquivo com --file.",
                Options = "--file <caminho>",
                Example = "ArchGen template view Default\n--file Domain/Entities/User.cs"
            },
            new()
            {
                Command = "Criar Template",
                Syntax = "ArchGen template create <NovoNome> [Opções]",
                Description = "Cria um novo template no banco clonando o template padrão ou existente.",
                Options = "--from <Base> (padrão: Default)\n--desc <Descrição>",
                Example = "ArchGen template create CustomApi\n--from Default"
            },
            new()
            {
                Command = "Editar Arquivo",
                Syntax = "ArchGen template edit-file <Nome> --file <Caminho> [Opções]",
                Description = "Modifica ou adiciona arquivo no template e salva no banco de dados.",
                Options = "--file, --content, --from-file",
                Example = "ArchGen template edit-file Custom\n--file Domain/User.cs"
            },
            new()
            {
                Command = "Excluir Template",
                Syntax = "ArchGen template delete <Nome>",
                Description = "Remove um template customizado do banco (o padrão 'Default' é protegido).",
                Options = "Nenhuma",
                Example = "ArchGen template delete CustomApi"
            },
            new()
            {
                Command = "Histórico",
                Syntax = "ArchGen history",
                Description = "Exibe o histórico de todos os projetos gerados pelo ArchGen salvos no banco.",
                Options = "Nenhuma",
                Example = "ArchGen history"
            }
        };
    }

    public string RenderHelpTable()
    {
        return RenderHelpTable(DefaultTableWidth, DefaultRowHeight);
    }

    public string RenderHelpTable(int tableWidth, int rowHeight)
    {
        // Enforce safe minimum bounds
        int safeWidth = Math.Max(100, tableWidth);
        int safeRowHeight = Math.Max(2, rowHeight);

        // Distribute columns proportionally inside the defined width
        // Left border (2) + 3 interior dividers (3 each = 9) + Right border (2) = 13 separator chars
        int contentWidth = safeWidth - 13;
        int col1Width = Math.Max(12, (int)(contentWidth * 0.14)); // Comando
        int col2Width = Math.Max(22, (int)(contentWidth * 0.27)); // Sintaxe
        int col3Width = Math.Max(28, (int)(contentWidth * 0.35)); // Descrição
        int col4Width = contentWidth - (col1Width + col2Width + col3Width); // Exemplo de Uso

        var items = GetHelpItems();
        var sb = new StringBuilder();

        // Build exact horizontal borders matching defined column widths
        string topBorder = $"╔{new string('═', safeWidth - 2)}╗";
        string headerDivider = $"╠{new string('═', col1Width + 2)}╦{new string('═', col2Width + 2)}╦{new string('═', col3Width + 2)}╦{new string('═', col4Width + 2)}╣";
        string subheaderBorder = $"╠{new string('═', col1Width + 2)}╬{new string('═', col2Width + 2)}╬{new string('═', col3Width + 2)}╬{new string('═', col4Width + 2)}╣";
        string rowSeparator = $"╟{new string('─', col1Width + 2)}╫{new string('─', col2Width + 2)}╫{new string('─', col3Width + 2)}╫{new string('─', col4Width + 2)}╢";
        string bottomBorder = $"╚{new string('═', col1Width + 2)}╩{new string('═', col2Width + 2)}╩{new string('═', col3Width + 2)}╩{new string('═', col4Width + 2)}╝";

        // Top Banner / Titles
        sb.AppendLine(topBorder);
        sb.AppendLine(FormatTitleLine("ARCHGEN - GERADOR DE ARQUITETURA LIMPA EM C# (.NET 8.0)", safeWidth));
        sb.AppendLine(FormatTitleLine("Guia Completo de Comandos, Templates e Gerenciamento", safeWidth));
        sb.AppendLine(headerDivider);

        // Column Headers
        string headerLine = $"║ {"Comando".PadRight(col1Width)} ║ {"Sintaxe".PadRight(col2Width)} ║ {"Descrição".PadRight(col3Width)} ║ {"Exemplo de Uso".PadRight(col4Width)} ║";
        sb.AppendLine(headerLine);
        sb.AppendLine(subheaderBorder);

        // Render each item with defined height
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];

            var col1Lines = FormatCell(item.Command, col1Width, safeRowHeight);
            var col2Lines = FormatCell(item.Syntax, col2Width, safeRowHeight);
            var col3Lines = FormatCell(item.Description, col3Width, safeRowHeight);
            var col4Lines = FormatCell(item.Example, col4Width, safeRowHeight);

            for (int lineIdx = 0; lineIdx < safeRowHeight; lineIdx++)
            {
                string c1 = col1Lines[lineIdx].PadRight(col1Width);
                string c2 = col2Lines[lineIdx].PadRight(col2Width);
                string c3 = col3Lines[lineIdx].PadRight(col3Width);
                string c4 = col4Lines[lineIdx].PadRight(col4Width);

                sb.AppendLine($"║ {c1} ║ {c2} ║ {c3} ║ {c4} ║");
            }

            // Separator between items, bottom border after the last item
            if (i < items.Count - 1)
            {
                sb.AppendLine(rowSeparator);
            }
            else
            {
                sb.AppendLine(bottomBorder);
            }
        }

        // Quick tip guide
        sb.AppendLine();
        sb.AppendLine("💡 Dica rápida:");
        sb.AppendLine("  1. Crie uma nova API: dotnet run --project ArchGen -- new MinhaApi API");
        sb.AppendLine("  2. Crie um Console:   dotnet run --project ArchGen -- new MeuConsole CONSOLE");
        sb.AppendLine("  3. Crie um Template:  dotnet run --project ArchGen -- template create MeuTemplate --from Default");
        sb.AppendLine("  4. Edite um arquivo:  dotnet run --project ArchGen -- template edit-file MeuTemplate --file Domain/Entities/User.cs --content \"// meu codigo\"");
        sb.AppendLine("  5. Ver histórico:     dotnet run --project ArchGen -- history");

        return sb.ToString();
    }

    private static string FormatTitleLine(string text, int tableWidth)
    {
        int innerWidth = tableWidth - 4; // for "║ " and " ║"
        string centered = CenterText(text, innerWidth);
        return $"║ {centered} ║";
    }

    private static string CenterText(string text, int width)
    {
        if (string.IsNullOrEmpty(text)) return new string(' ', width);
        if (text.Length >= width) return text.Substring(0, width);

        int totalPadding = width - text.Length;
        int padLeft = totalPadding / 2;
        int padRight = totalPadding - padLeft;
        return new string(' ', padLeft) + text + new string(' ', padRight);
    }

    private static List<string> FormatCell(string text, int maxWidth, int rowHeight)
    {
        var lines = new List<string>();

        if (!string.IsNullOrWhiteSpace(text))
        {
            var rawLines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (var rawLine in rawLines)
            {
                var trimmed = rawLine.Trim();
                if (string.IsNullOrEmpty(trimmed))
                {
                    lines.Add(string.Empty);
                    continue;
                }

                var words = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var currentLine = new StringBuilder();

                foreach (var word in words)
                {
                    if (currentLine.Length == 0)
                    {
                        if (word.Length <= maxWidth)
                        {
                            currentLine.Append(word);
                        }
                        else
                        {
                            // Word exceeds maxWidth: chunk it safely
                            var remaining = word;
                            while (remaining.Length > maxWidth)
                            {
                                lines.Add(remaining.Substring(0, maxWidth));
                                remaining = remaining.Substring(maxWidth);
                            }
                            currentLine.Append(remaining);
                        }
                    }
                    else if (currentLine.Length + 1 + word.Length <= maxWidth)
                    {
                        currentLine.Append(' ').Append(word);
                    }
                    else
                    {
                        lines.Add(currentLine.ToString());
                        currentLine.Clear();

                        if (word.Length <= maxWidth)
                        {
                            currentLine.Append(word);
                        }
                        else
                        {
                            var remaining = word;
                            while (remaining.Length > maxWidth)
                            {
                                lines.Add(remaining.Substring(0, maxWidth));
                                remaining = remaining.Substring(maxWidth);
                            }
                            currentLine.Append(remaining);
                        }
                    }
                }

                if (currentLine.Length > 0)
                {
                    lines.Add(currentLine.ToString());
                }
            }
        }

        // Enforce defined rowHeight (truncate with ellipsis if exceeded)
        if (lines.Count > rowHeight)
        {
            var truncated = lines.Take(rowHeight).ToList();
            var lastLine = truncated[rowHeight - 1];
            if (lastLine.Length > maxWidth - 3)
            {
                lastLine = lastLine.Substring(0, Math.Max(0, maxWidth - 3)) + "...";
            }
            else
            {
                lastLine = lastLine + "...";
            }
            truncated[rowHeight - 1] = lastLine;
            return truncated;
        }

        // Pad with empty lines if fewer than rowHeight
        while (lines.Count < rowHeight)
        {
            lines.Add(string.Empty);
        }

        return lines;
    }
}
