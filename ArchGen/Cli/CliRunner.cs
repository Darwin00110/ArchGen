using ArchGen.Application.DTOs;
using ArchGen.Application.Interfaces;
using ArchGen.Domain.Interfaces;

namespace ArchGen.Cli;

public class CliRunner
{
    private readonly IGenerateProjectUseCase _generateUseCase;
    private readonly ITemplateUseCase _templateUseCase;
    private readonly IProjectHistoryUseCase _historyUseCase;
    private readonly IHelpUseCase _helpUseCase;
    private readonly IFileService _fileService;

    public CliRunner(
        IGenerateProjectUseCase generateUseCase,
        ITemplateUseCase templateUseCase,
        IProjectHistoryUseCase historyUseCase,
        IHelpUseCase helpUseCase,
        IFileService fileService)
    {
        _generateUseCase = generateUseCase;
        _templateUseCase = templateUseCase;
        _historyUseCase = historyUseCase;
        _helpUseCase = helpUseCase;
        _fileService = fileService;
    }

    public async Task<int> RunAsync(string[] args)
    {
        var parsed = CommandLineParser.Parse(args);

        if (parsed.HasFlag("--help") || parsed.HasFlag("-h") || parsed.HasFlag("help") || string.IsNullOrEmpty(parsed.Command))
        {
            ShowHelp();
            return 0;
        }

        try
        {
            return parsed.Command switch
            {
                "new" or "generate" or "create-project" => await HandleNewProject(parsed),
                "template" or "templates" => await HandleTemplates(parsed),
                "history" or "list-projects" => await HandleHistory(parsed),
                _ => HandleUnknownCommand(parsed)
            };
        }
        catch (Exception ex)
        {
            PrintColored($"❌ Erro: {ex.Message}", ConsoleColor.Red);
            return 1;
        }
    }

    private void ShowHelp()
    {
        var helpTable = _helpUseCase.RenderHelpTable();
        Console.WriteLine(helpTable);
    }

    private async Task<int> HandleNewProject(ParsedArgs parsed)
    {
        // Syntaxes supported:
        // ArchGen new <NomeDoProjeto> [TipoProjeto] [Caminho] [--template <Template>]
        string projectName = "";
        string projectType = "API";
        string? targetPath = null;

        // In parsed: if command was "new", subCommand might be the project name if user ran `ArchGen new MeuProj`
        if (!string.IsNullOrWhiteSpace(parsed.SubCommand))
        {
            projectName = parsed.SubCommand;

            if (parsed.PositionalArgs.Count > 0)
            {
                var arg0 = parsed.PositionalArgs[0].ToUpperInvariant();
                if (arg0 == "API" || arg0 == "CONSOLE")
                {
                    projectType = arg0;
                    if (parsed.PositionalArgs.Count > 1)
                        targetPath = parsed.PositionalArgs[1];
                }
                else
                {
                    targetPath = parsed.PositionalArgs[0];
                }
            }
        }
        else if (parsed.PositionalArgs.Count > 0)
        {
            projectName = parsed.PositionalArgs[0];
            if (parsed.PositionalArgs.Count > 1)
            {
                var arg1 = parsed.PositionalArgs[1].ToUpperInvariant();
                if (arg1 == "API" || arg1 == "CONSOLE")
                {
                    projectType = arg1;
                    if (parsed.PositionalArgs.Count > 2)
                        targetPath = parsed.PositionalArgs[2];
                }
                else
                {
                    targetPath = parsed.PositionalArgs[1];
                }
            }
        }

        var templateName = parsed.GetNamed("template", "Default");

        if (string.IsNullOrWhiteSpace(projectName))
        {
            PrintColored("❌ Erro: É obrigatório informar o nome do projeto.", ConsoleColor.Red);
            Console.WriteLine("Exemplo: ArchGen new MeuProjeto API");
            return 1;
        }

        PrintColored($"🏗️  Iniciando geração de solução Clean Architecture '{projectName}' ({projectType})...", ConsoleColor.Cyan);
        Console.WriteLine($"   Template: {templateName}");
        Console.WriteLine($"   Destino:  {(string.IsNullOrWhiteSpace(targetPath) ? _fileService.GetCurrentDirectory() : targetPath)}");
        
        var request = new GenerateProjectRequest
        {
            ProjectName = projectName,
            ProjectType = projectType,
            TargetPath = targetPath,
            TemplateName = templateName
        };

        var result = await _generateUseCase.ExecuteAsync(request);

        if (result.Success)
        {
            PrintColored("\n✅ " + result.Message, ConsoleColor.Green);
            Console.WriteLine($"   Arquivos gerados: {result.GeneratedFilesCount}");
            Console.WriteLine("\n📁 Estrutura da solução criada:");
            foreach (var file in result.GeneratedFiles.Take(15))
            {
                Console.WriteLine($"   ├── {file}");
            }
            if (result.GeneratedFiles.Count > 15)
            {
                Console.WriteLine($"   └── ... e mais {result.GeneratedFiles.Count - 15} arquivos.");
            }

            PrintColored("\n🎉 Pronto para rodar! Execute:", ConsoleColor.Yellow);
            Console.WriteLine($"   cd \"{result.OutputPath}\"");
            Console.WriteLine("   dotnet build");
            if (projectType == "API")
            {
                Console.WriteLine($"   dotnet run --project src/{projectName}.API");
            }
            else
            {
                Console.WriteLine($"   dotnet run --project src/{projectName}.Console");
            }
            return 0;
        }
        else
        {
            PrintColored("\n❌ " + result.Message, ConsoleColor.Red);
            foreach (var err in result.Errors)
            {
                PrintColored($"   - {err}", ConsoleColor.DarkRed);
            }
            return 1;
        }
    }

    private async Task<int> HandleTemplates(ParsedArgs parsed)
    {
        var action = parsed.SubCommand.ToLowerInvariant();

        return action switch
        {
            "list" or "ls" => await ListTemplates(),
            "view" or "show" => await ViewTemplate(parsed),
            "create" or "add" => await CreateTemplate(parsed),
            "edit-file" or "update-file" => await EditTemplateFile(parsed),
            "delete" or "remove" or "rm" => await DeleteTemplate(parsed),
            _ => ShowTemplateHelp()
        };
    }

    private async Task<int> ListTemplates()
    {
        var templates = await _templateUseCase.GetAllTemplatesAsync();

        Console.WriteLine("\n📋 TEMPLATES DISPONÍVEIS NO ARCHGEN:");
        Console.WriteLine(new string('─', 95));
        Console.WriteLine($"{"Nome",-20} | {"Tipo",-10} | {"Arquivos",-10} | {"Criado em",-12} | {"Descrição"}");
        Console.WriteLine(new string('─', 95));

        foreach (var t in templates)
        {
            var tipo = t.IsDefault ? "Padrão" : "Custom";
            var data = t.CreatedAt.ToString("yyyy-MM-dd");
            Console.WriteLine($"{t.Name,-20} | {tipo,-10} | {t.FilesCount,-10} | {data,-12} | {t.Description}");
        }

        Console.WriteLine(new string('─', 95));
        Console.WriteLine("Use 'ArchGen template view <Nome>' para inspecionar os arquivos.");
        return 0;
    }

    private async Task<int> ViewTemplate(ParsedArgs parsed)
    {
        var templateName = parsed.PositionalArgs.FirstOrDefault() ?? parsed.GetNamed("name");
        if (string.IsNullOrWhiteSpace(templateName))
        {
            PrintColored("❌ Informe o nome do template. Exemplo: ArchGen template view Default", ConsoleColor.Red);
            return 1;
        }

        var details = await _templateUseCase.GetTemplateDetailsAsync(templateName);
        if (details == null)
        {
            PrintColored($"❌ Template '{templateName}' não encontrado.", ConsoleColor.Red);
            return 1;
        }

        var requestedFile = parsed.GetNamed("file");
        if (!string.IsNullOrWhiteSpace(requestedFile))
        {
            var file = details.Files.FirstOrDefault(f => f.RelativePath.Equals(requestedFile, StringComparison.OrdinalIgnoreCase) || f.RelativePath.EndsWith(requestedFile, StringComparison.OrdinalIgnoreCase));
            if (file == null)
            {
                PrintColored($"❌ Arquivo '{requestedFile}' não encontrado no template '{templateName}'.", ConsoleColor.Red);
                return 1;
            }

            Console.WriteLine($"\n📄 Arquivo: {file.RelativePath} (Camada: {file.Layer})");
            Console.WriteLine(new string('─', 80));
            Console.WriteLine(file.Content);
            Console.WriteLine(new string('─', 80));
            return 0;
        }

        Console.WriteLine($"\n📦 Template: {details.Name} {(details.IsDefault ? "[Padrão]" : "[Customizado]")}");
        Console.WriteLine($"Descrição: {details.Description}");
        Console.WriteLine($"Arquivos no template ({details.Files.Count}):");
        Console.WriteLine(new string('─', 80));
        Console.WriteLine($"{"Camada",-15} | {"API",-5} | {"Console",-7} | Caminho Relativo");
        Console.WriteLine(new string('─', 80));

        foreach (var f in details.Files)
        {
            var api = f.IsForApi ? "Sim" : "Não";
            var con = f.IsForConsole ? "Sim" : "Não";
            Console.WriteLine($"{f.Layer,-15} | {api,-5} | {con,-7} | {f.RelativePath}");
        }

        Console.WriteLine(new string('─', 80));
        Console.WriteLine("Dica: Use '--file <caminho>' para ver o conteúdo de um arquivo.");
        return 0;
    }

    private async Task<int> CreateTemplate(ParsedArgs parsed)
    {
        var newName = parsed.PositionalArgs.FirstOrDefault() ?? parsed.GetNamed("name");
        var baseName = parsed.GetNamed("from", "Default");
        var desc = parsed.GetNamed("desc", $"Clonado de {baseName}");

        if (string.IsNullOrWhiteSpace(newName))
        {
            PrintColored("❌ Nome do novo template não informado. Exemplo: ArchGen template create MeuTemplate --from Default", ConsoleColor.Red);
            return 1;
        }

        var created = await _templateUseCase.CreateTemplateAsync(new CreateTemplateRequest
        {
            Name = newName,
            BaseTemplateName = baseName,
            Description = desc
        });

        PrintColored($"✅ Template customizado '{created.Name}' criado com sucesso e salvo no banco de dados!", ConsoleColor.Green);
        Console.WriteLine($"   Base: {baseName}");
        Console.WriteLine($"   Arquivos clonados: {created.FilesCount}");
        Console.WriteLine($"   Agora você pode manipular arquivos nele com 'ArchGen template edit-file {created.Name} --file <caminho> ...'");
        return 0;
    }

    private async Task<int> EditTemplateFile(ParsedArgs parsed)
    {
        var templateName = parsed.PositionalArgs.FirstOrDefault() ?? parsed.GetNamed("name");
        var fileRelativePath = parsed.GetNamed("file");
        var content = parsed.GetNamed("content");
        var fromFile = parsed.GetNamed("from-file");

        if (string.IsNullOrWhiteSpace(templateName))
        {
            PrintColored("❌ Informe o nome do template. Exemplo: ArchGen template edit-file MeuTemplate --file ...", ConsoleColor.Red);
            return 1;
        }

        if (string.IsNullOrWhiteSpace(fileRelativePath))
        {
            PrintColored("❌ Informe o caminho relativo do arquivo com --file <caminho>.", ConsoleColor.Red);
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(fromFile))
        {
            if (!_fileService.FileExists(fromFile))
            {
                PrintColored($"❌ Arquivo local '{fromFile}' não foi encontrado no disco.", ConsoleColor.Red);
                return 1;
            }
            content = await _fileService.ReadAllTextAsync(fromFile);
        }

        if (content == null)
        {
            PrintColored("❌ Informe o novo conteúdo com --content \"<texto>\" ou --from-file <caminhoLocal>.", ConsoleColor.Red);
            return 1;
        }

        var result = await _templateUseCase.UpdateTemplateFileAsync(new UpdateTemplateFileRequest
        {
            TemplateName = templateName,
            RelativePath = fileRelativePath,
            Content = content
        });

        PrintColored($"✅ Arquivo '{result.RelativePath}' do template '{templateName}' atualizado com sucesso e salvo no banco!", ConsoleColor.Green);
        return 0;
    }

    private async Task<int> DeleteTemplate(ParsedArgs parsed)
    {
        var templateName = parsed.PositionalArgs.FirstOrDefault() ?? parsed.GetNamed("name");
        if (string.IsNullOrWhiteSpace(templateName))
        {
            PrintColored("❌ Informe o nome do template a ser excluído.", ConsoleColor.Red);
            return 1;
        }

        await _templateUseCase.DeleteTemplateAsync(templateName);
        PrintColored($"✅ Template '{templateName}' excluído com sucesso do banco de dados.", ConsoleColor.Green);
        return 0;
    }

    private int ShowTemplateHelp()
    {
        Console.WriteLine("\nComandos de template disponíveis:");
        Console.WriteLine("  ArchGen template list");
        Console.WriteLine("  ArchGen template view <Nome> [--file <caminho>]");
        Console.WriteLine("  ArchGen template create <NovoNome> [--from <Base>] [--desc <Desc>]");
        Console.WriteLine("  ArchGen template edit-file <Nome> --file <caminho> (--content <texto> | --from-file <arquivo>)");
        Console.WriteLine("  ArchGen template delete <Nome>");
        return 0;
    }

    private async Task<int> HandleHistory(ParsedArgs parsed)
    {
        var history = await _historyUseCase.GetAllHistoryAsync();

        if (history.Count == 0)
        {
            Console.WriteLine("\nNenhum projeto foi gerado ainda. Use 'ArchGen new <Nome>' para criar um novo projeto!");
            return 0;
        }

        Console.WriteLine("\n📜 HISTÓRICO DE PROJETOS GERADOS NO ARCHGEN:");
        Console.WriteLine(new string('─', 115));
        Console.WriteLine($"{"ID",-4} | {"Projeto",-20} | {"Tipo",-8} | {"Template",-15} | {"Data de Criação",-19} | {"Status",-8} | {"Caminho"}");
        Console.WriteLine(new string('─', 115));

        foreach (var h in history)
        {
            var data = h.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"{h.Id,-4} | {h.ProjectName,-20} | {h.ProjectType,-8} | {h.TemplateUsed,-15} | {data,-19} | {h.Status,-8} | {h.ProjectPath}");
        }

        Console.WriteLine(new string('─', 115));
        return 0;
    }

    private int HandleUnknownCommand(ParsedArgs parsed)
    {
        PrintColored($"Comando desconhecido: '{parsed.Command}'", ConsoleColor.Yellow);
        ShowHelp();
        return 1;
    }

    private static void PrintColored(string text, ConsoleColor color)
    {
        var prev = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = prev;
    }
}
