using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Rbxpack.Application;

public class LinkCommand : Command<LinkCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-d|--directory")]
        [Description("The project directory. Default is the current directory.")]
        [DefaultValue("")]
        public required string ProjectDirectory { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        settings.ProjectDirectory = settings.ProjectDirectory != "" ? settings.ProjectDirectory : Directory.GetCurrentDirectory();

        var Config = ConfigManager.GetConfig(settings.ProjectDirectory);
        var LauncherLinks = ConfigManager.GetLinks(settings.ProjectDirectory);
        ConfigManager.EnsureData(settings.ProjectDirectory);


        AnsiConsole.WriteLine("Refresh link TODO...");
        return 0;
    }
}