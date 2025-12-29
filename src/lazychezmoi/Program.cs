using ConsoleAppFramework;
using DotNetPathUtils;
using LazyChezmoi;
using LazyChezmoi.Commands;
using LazyChezmoi.Filters;
using LazyChezmoi.Services;
using Microsoft.Extensions.DependencyInjection;
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

AppInitializer.Initialize();

var services = new ServiceCollection();

services.RegisterAppServices();
ConsoleApp.ServiceProvider = services.BuildServiceProvider();

var app = ConsoleApp.Create();

app.Add<MyCommands>();

app.UseFilter<ExceptionFilter>();

await app.RunAsync(args);
