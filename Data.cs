using System.Text.Json;
using Spectre.Console;

namespace Rbxpack
{
    public class Config
    {
        public required string ProjectName { get; set; }
        public required string FriendlyName { get; set; }
        public required string Description { get; set; }
        public required string Author { get; set; }
        public required string Version { get; set; }
        public required List<string> Clients { get; set; }
        public required string ProjectRbxl { get; set; }
    }

    // Represents the 'link' section of the config
    public class LauncherLink
    {
        public required string ClientsDir { get; set; }
        public required string MapsDir { get; set; }
    }

    public class ConfigManager
    {
        public static Config GetConfig(string projectDirectory = "")
        {
            if (projectDirectory == "")
            {
                projectDirectory = Directory.GetCurrentDirectory();
            }

            if (!FileExists(projectDirectory, "rbxpack.json"))
            {
                AnsiConsole.MarkupLine($"[bold red]Error:[/] No config file found in directory {projectDirectory}. Please set up the project with 'rbxpack init' first.");
                throw new FileNotFoundException($"The 'rbxpack.json' config file was not found in directory {projectDirectory}");
            }

            string path = Path.Combine(projectDirectory, "rbxpack.json");
#pragma warning disable CS8603 // Possible null reference return.
            return JsonSerializer.Deserialize<Config>(File.ReadAllText(path));
#pragma warning restore CS8603 // Possible null reference return.
        }

        public static List<LauncherLink> GetLinks(string projectDirectory = "")
        {
            if (projectDirectory == "")
            {
                projectDirectory = Directory.GetCurrentDirectory();
            }

            if (!FileExists(projectDirectory, "rbxpack.launcherlinks.json"))
            {
                AnsiConsole.MarkupLine($"[bold red]Error:[/] No launcher links config file found in directory {projectDirectory}. Please add links with 'rbxpack link add' first.");
                throw new FileNotFoundException($"The 'rbxpack.launcherlinks.json' config file was not found in directory {projectDirectory}");
            }

            string path = Path.Combine(projectDirectory, "rbxpack.launcherlinks.json");
#pragma warning disable CS8603 // Possible null reference return.
            return JsonSerializer.Deserialize<List<LauncherLink>>(File.ReadAllText(path));
#pragma warning restore CS8603 // Possible null reference return.
        }

        public static bool DirectoryExists(string projectDirectory, string directoryName)
        {
            string path = Path.Combine(projectDirectory, directoryName);
            return Directory.Exists(path);
        }

        public static bool FileExists(string projectDirectory, string fileName)
        {
            string path = Path.Combine(projectDirectory, fileName);
            return File.Exists(path);
        }

        public static void EnsureData(string projectDirectory = "")
        {
            if (projectDirectory == "")
            {
                projectDirectory = Directory.GetCurrentDirectory();
            }

            if (!DirectoryExists(projectDirectory, "assets"))
            {
                throw new DirectoryNotFoundException($"The 'assets' directory was not found in directory {projectDirectory}.");
            }

            if (!FileExists(projectDirectory, GetConfig(projectDirectory).ProjectRbxl ?? ""))
            {
                throw new FileNotFoundException($"The project rbxl file was not found in directory {projectDirectory}. Check your 'ProjectRbxl' setting in the config.");
            }
        }

        /// <summary>
        /// Gets the actual folder containing the Roblox client in a launcher's client folder
        /// Novetus and ORRH have different structures, so this method is necessary to get the correct folder.
        /// </summary>
        /// <param name="clientDirectory">The path to the client directory, should be directly under the launcher's "clients" folder</param>
        /// <returns>The path to the actual client folder</returns>
        public static string GetClient(string clientDirectory)
        {
            // SciLexer.dll is found in all old(?) Roblox clients, so we can use that to know if it's a Roblox client folder
            if (FileExists(clientDirectory, "SciLexer.dll") {
                return clientDirectory;
            }
            else if (DirectoryExists(clientDirectory, "Player") && FileExists(Path.Combine(clientDirectory, "bin"), "SciLexer.dll"))
            {
                return Path.Combine(clientDirectory, "Player");
            }
            else
            {
                throw new DirectoryNotFoundException($"The client directory {clientDirectory} does not appear to be a Roblox client folder.");

            }
        }
    }
}