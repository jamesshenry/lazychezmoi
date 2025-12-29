using DotNetPathUtils;
using lazychezmoi;
using LazyChezmoi;
using LazyChezmoi.Services;
using LazyChezmoi.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Terminal.Gui.App;
using Terminal.Gui.Views;
using Velopack;

if (OperatingSystem.IsWindows())
{
    var appDirectory = Path.GetDirectoryName(AppContext.BaseDirectory)!;
    var pathHelper = new PathEnvironmentHelper(new PathUtilsOptions() { PrefixWithPeriod = false });
    VelopackApp
        .Build()
        .OnAfterInstallFastCallback(v => pathHelper.EnsureDirectoryIsInPath(appDirectory))
        .OnBeforeUninstallFastCallback(v => pathHelper.RemoveDirectoryFromPath(appDirectory!))
        .Run();
}

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("logs/bootstrap.log")
    .CreateLogger();

try
{
    var builder = Host.CreateApplicationBuilder(args);
    builder.AddTuiLogging();
    builder.AddTuiInfrastructure();
    builder.AddTuiScreens();

    using IHost host = builder.Build();

    var app = host.Services.GetRequiredService<IApplication>();
    var mainShell = host.Services.GetRequiredService<MainShell>();

    app.Init();
    app.Run(mainShell);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
