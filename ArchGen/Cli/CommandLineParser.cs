namespace ArchGen.Cli;

public class ParsedArgs
{
    public string Command { get; set; } = string.Empty;
    public string SubCommand { get; set; } = string.Empty;
    public List<string> PositionalArgs { get; set; } = new();
    public Dictionary<string, string> NamedArgs { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public HashSet<string> Flags { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public string? GetNamed(string key, string? defaultValue = null)
    {
        return NamedArgs.TryGetValue(key, out var val) ? val : defaultValue;
    }

    public bool HasFlag(string flag)
    {
        return Flags.Contains(flag);
    }
}

public static class CommandLineParser
{
    public static ParsedArgs Parse(string[] args)
    {
        var result = new ParsedArgs();
        if (args == null || args.Length == 0)
        {
            result.Flags.Add("--help");
            return result;
        }

        int index = 0;

        // Check if first arg is a flag like --help or -h
        if (args[0].StartsWith("-"))
        {
            // Process flags / named args
        }
        else
        {
            result.Command = args[0].ToLowerInvariant();
            index++;

            if (index < args.Length && !args[index].StartsWith("-"))
            {
                result.SubCommand = args[index];
                index++;
            }
        }

        while (index < args.Length)
        {
            var current = args[index];

            if (current.StartsWith("--") || current.StartsWith("-"))
            {
                var cleanKey = current.TrimStart('-');
                
                // Check if next arg is value
                if (index + 1 < args.Length && !args[index + 1].StartsWith("-"))
                {
                    result.NamedArgs[cleanKey] = args[index + 1];
                    index += 2;
                }
                else
                {
                    result.Flags.Add(current);
                    result.Flags.Add(cleanKey);
                    index++;
                }
            }
            else
            {
                result.PositionalArgs.Add(current);
                index++;
            }
        }

        return result;
    }
}
