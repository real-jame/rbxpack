using System.ComponentModel;
using System.Text.RegularExpressions;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Rbxpack.Application;

public class BuildCommand : Command<BuildCommand.Settings>
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
        Config config = ConfigManager.GetConfig(settings.ProjectDirectory);
        ConfigManager.EnsureData(settings.ProjectDirectory);

        DirectoryInfo assets = new DirectoryInfo(Path.Combine(settings.ProjectDirectory, "assets"));
        string projectRbxlPath = Path.Combine(settings.ProjectDirectory, config.ProjectRbxl);

        DirectoryInfo buildDir = Directory.CreateDirectory(Path.Combine(settings.ProjectDirectory, "build"));

        string matchUrls = @"rbxasset://(?<filePath>[\w/\\.-]+)";
        Regex regex = new Regex(matchUrls, RegexOptions.Multiline | RegexOptions.IgnoreCase);

        AnsiConsole.WriteLine("Bundling for Novetus.");
        DirectoryInfo buildNovetus = Directory.CreateDirectory(Path.Combine(buildDir.FullName, "novetus"));
        // This will work by going through the XML and when it encounters an asset request,
        // it will copy that over to the necessary spot in build directory and rewrite the URL
        MatchEvaluator novetusReplacer = new MatchEvaluator((Match match) =>
        {
            AnsiConsole.WriteLine(match.Groups["filePath"].Value);
            return "rbxasset://textures/face.png";
        });
        string novetusRbxlPath = Path.Combine(buildNovetus.FullName, $"{config.ProjectName}.rbxl");
        File.WriteAllText(novetusRbxlPath, regex.Replace(File.ReadAllText(projectRbxlPath), novetusReplacer));


        AnsiConsole.WriteLine("Bundling for ORRH.");
        DirectoryInfo buildOrrh = Directory.CreateDirectory(Path.Combine(buildDir.FullName, "orrh"));

        return 0;
    }
}