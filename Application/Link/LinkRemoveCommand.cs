using Spectre.Console;
using Spectre.Console.Cli;

namespace Rbxpack.Application.Link;

public class LinkRemoveCommand : Command<LinkRemoveCommand.Settings>
{
    public class Settings : CommandSettings
    {
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        AnsiConsole.WriteLine("Remove link TODO...");
        return 0;
    }
}