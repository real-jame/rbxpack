using Spectre.Console;
using Spectre.Console.Cli;

namespace Rbxpack.Application;

public class InitCommand : Command<InitCommand.Settings>
{
    public class Settings : CommandSettings
    {
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        AnsiConsole.WriteLine("Init TODO...");
        return 0;
    }
}