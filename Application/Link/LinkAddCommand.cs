using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Rbxpack.Application.Link;

public class LinkAddCommand : Command<LinkAddCommand.Settings>
{
    public class Settings : CommandSettings
    {
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        AnsiConsole.WriteLine("Add link TODO...");
        return 0;
    }
}