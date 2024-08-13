using Spectre.Console;
using Spectre.Console.Cli;

namespace Rbxpack.Application.Link;

public class LinkRefreshCommand : Command<LinkRefreshCommand.Settings>
{
    public class Settings : CommandSettings
    {
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        AnsiConsole.WriteLine("Refresh link TODO...");
        return 0;
    }
}