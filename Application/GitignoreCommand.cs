using System.ComponentModel;
using System.Text.Json;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Rbxpack.Application;

public class GitignoreCommand : Command<GitignoreCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-d|--directory")]
        [Description("The project directory. Default is the current directory.")]
        [DefaultValue("")]
        public required string ProjectDirectory { get; set; }

        [CommandOption("-f|--force")]
        [Description("Force creation, even if it means overwriting files.")]
        [DefaultValue(false)]
        public required bool ForceCreate { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        settings.ProjectDirectory = settings.ProjectDirectory != "" ? settings.ProjectDirectory : Directory.GetCurrentDirectory();

        var gitignorePath = Path.Combine(settings.ProjectDirectory, ".gitignore");

        if (File.Exists(gitignorePath) && !settings.ForceCreate)
        {
            AnsiConsole.MarkupLine("[red]This would overwrite the existing gitignore file. To create anyway, run the command with the '--force' option.[/]");
            return 1;
        }

        File.WriteAllText(gitignorePath, "rbxpack.launcherlinks.json");

        AnsiConsole.MarkupLine($"The gitignore file was created successfully.");

        return 0;
    }
}