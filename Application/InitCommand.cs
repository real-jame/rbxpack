using System.ComponentModel;
using System.Text.Json;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Rbxpack.Application;

public class InitCommand : Command<InitCommand.Settings>
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
        AnsiConsole.MarkupLine($"[bold]rbxpack init[/] will create an rbxpack.json project config in [blue]{settings.ProjectDirectory}[/]. [gray]Press ? for info on the current prompt, or CTRL+C anytime to quit[/]");
        // TODO: use ForceCreate to avoid overwriting, as done in gitignore command

        var projectName = AnsiConsole.Ask<string>("[green]?[/] Project name:");
        var friendlyName = AnsiConsole.Ask<string>("[green]?[/] Friendly name:");
        var description = AnsiConsole.Ask<string>("[green]?[/] Description:");
        var author = AnsiConsole.Ask<string>("[green]?[/] Author:");
        var version = AnsiConsole.Ask<string>("[green]?[/] Version:");
        var clients = AnsiConsole.Ask<string>("[green]?[/] Clients (comma-separated):")
            .Split(',')
            .Select(client => client.Trim())
            .ToList();
        var projectRbxl = AnsiConsole.Ask<string>("[green]?[/] Name of your project's .rbxl file:");

        var links = new List<LauncherLink>();
        while (true)
        {
            // Kinda clunky phrasing
            var addNewLink = AnsiConsole.Ask<string>("[green]?[/] Add a new launcher link? [gray]type anything to continue or leave blank to skip[/]", "");
            if (!string.IsNullOrEmpty(addNewLink))
            {
                var clientsDir = AnsiConsole.Ask<string>("[green]?[/] Full path name of the launcher's clients directory");
                var mapsDir = AnsiConsole.Ask<string>("[green]?[/] Full path name of the launcher's maps directory");
                links.Add(new LauncherLink { ClientsDir = clientsDir, MapsDir = mapsDir });
            }
            else
            {
                break;
            }
        }

        var config = new Config
        {
            ProjectName = projectName,
            FriendlyName = friendlyName,
            Description = description,
            Author = author,
            Version = version,
            Clients = clients,
            ProjectRbxl = projectRbxl,
        };

        var configJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        var configPath = Path.Combine(settings.ProjectDirectory, "rbxpack.json");

        var linksJson = JsonSerializer.Serialize(links, new JsonSerializerOptions { WriteIndented = true });
        var linksPath = Path.Combine(settings.ProjectDirectory, "rbxpack.launcherlinks.json");

        File.WriteAllText(configPath, configJson);
        File.WriteAllText(linksPath, linksJson);

        // TODO: Fill in the gitignore command with the directory used here                                    
        AnsiConsole.MarkupLine($"[green]{Emoji.Known.GreenHeart}[/] [bold]rbxpack.json[/] was created successfully. Have fun! [gray]If you're using Git (you should), make sure to add [bold]rbxpack.launcherlinks.json[/] to your gitignore. Or, run [bold]rbxpack gitignore -d {settings.ProjectDirectory}[/] to create a gitignore file.[/]");

        return 0;
    }
}