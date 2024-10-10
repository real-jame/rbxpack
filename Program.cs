using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Rbxpack.Application;
using Rbxpack.Application.Link;
using Spectre.Console;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    System.Console.OutputEncoding = Encoding.UTF8;
    System.Console.InputEncoding = Encoding.UTF8;

    config.SetApplicationName("rbxpack");

    // config.SetExceptionHandler(ex =>
    // {
    //     AnsiConsole.WriteException(ex);
    //     return -99;
    // });

    config.AddCommand<BuildCommand>("build").WithDescription("Create sharable copies of the project that work on Novetus and ORRH launchers");
    config.AddCommand<GitignoreCommand>("gitignore").WithDescription("Creates a gitignore file for an rbxpack project");
    config.AddCommand<InitCommand>("init").WithDescription("Start a new rbxpack project");
    config.AddBranch("link", branch =>
    {
        branch.AddCommand<LinkAddCommand>("add").WithDescription("Link a launcher to the project");
        branch.AddCommand<LinkRemoveCommand>("remove").WithDescription("Unlink a launcher from the project");
        branch.AddCommand<LinkRefreshCommand>("refresh").WithDescription("Refresh a launcher's integration with the project files");
    });
});

return app.Run(args);
