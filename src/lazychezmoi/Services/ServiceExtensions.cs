using Kuddle.Extensions.Configuration;
using Kuddle.Serialization;
using LazyChezmoi.Configuration;
using LazyChezmoi.Logging;
using LazyChezmoi.Modals;
using LazyChezmoi.Navigation;
using LazyChezmoi.ViewModels;
using LazyChezmoi.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Formatting.Display;
using Terminal.Gui.App;
using Terminal.Gui.Views;

namespace LazyChezmoi.Services;

public static class ServiceExtensions
{
    public static void ConfigureSerilog(this ILoggingBuilder builder)
    {
        const string outputTemplate =
            "[{Timestamp:HH:mm:ss} {Level:u3}] ({SourceClass}) {Message:lj}{NewLine}{Exception}";
        builder.AddSerilog(
            new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(
                    formatter: new MessageTemplateTextFormatter(outputTemplate),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "app-.log"),
                    restrictedToMinimumLevel: LogEventLevel.Debug,
                    shared: true,
                    rollingInterval: RollingInterval.Day
                )
                .Enrich.WithProperty("ApplicationName", "<APP NAME>")
                .Enrich.With<SourceClassEnricher>()
                .CreateLogger()
        );
    }

    public static void AddTuiLogging(this HostApplicationBuilder builder)
    {
        builder.Services.AddSerilog(
            (services, lc) =>
            {
                const string hostTemplate =
                    "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}";
                const string appTemplate =
                    "[{Timestamp:HH:mm:ss} {Level:u3}] ({SourceClass}) {Message:lj}{NewLine}{Exception}";

                lc.ReadFrom.Configuration(builder.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.With<SourceClassEnricher>()
                    // Host Logs (Framework/System)
                    .WriteTo.Conditional(
                        evt =>
                            Matching.FromSource("Microsoft").Invoke(evt)
                            || Matching.FromSource("System").Invoke(evt),
                        wt =>
                            wt.File(
                                "logs/host-.log",
                                outputTemplate: hostTemplate,
                                rollingInterval: RollingInterval.Day
                            )
                    )
                    // App Logs (Business Logic)
                    .WriteTo.Conditional(
                        evt =>
                            !Matching.FromSource("Microsoft").Invoke(evt)
                            && !Matching.FromSource("System").Invoke(evt),
                        wt =>
                            wt.File(
                                "logs/app-.log",
                                outputTemplate: appTemplate,
                                rollingInterval: RollingInterval.Day
                            )
                    );
            }
        );
    }

    public static void AddTuiInfrastructure(this HostApplicationBuilder builder)
    {
        builder.Configuration.Sources.Clear();

        builder.Configuration.AddKdlFile("config.kdl");
        var settings = new LazyChezmoiSettings();
        builder.Configuration.GetSection("lazy-chezmoi-settings").Bind(settings);

        // Core Terminal.Gui v2 Instance
        builder.Services.AddSingleton(_ => Application.Create());

        // MVVM Infrastructure
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IDialogService, DialogService>();
        builder.Services.AddSingleton<IAppLifetime, AppLifetime>();
    }

    public static void AddTuiScreens(this HostApplicationBuilder builder)
    {
        // ViewModels
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();
        builder.Services.AddTransient<ConfirmDialogViewModel>();

        // Views
        builder.Services.AddSingleton<MainShell>();
        builder.Services.AddTransient<HomeView>();
        builder.Services.AddTransient<SettingsView>();
        builder.Services.AddTransient<ConfirmDialog>();
    }
}

public interface IDialogService
{
    bool Confirm(string title, string message, string okText = "Yes", string cancelText = "No");
    void ShowInfo(string title, string message);
    void ShowError(string title, string message);
}

public class DialogService : IDialogService
{
    private readonly IApplication _app;

    public DialogService(IApplication app)
    {
        _app = app;
    }

    public bool Confirm(
        string title,
        string message,
        string okText = "Yes",
        string cancelText = "No"
    )
    {
        int? result = MessageBox.Query(_app, title, message, okText, cancelText);
        return result == 0;
    }

    public void ShowInfo(string title, string message)
    {
        MessageBox.Query(_app, title, message, "Ok");
    }

    public void ShowError(string title, string message)
    {
        MessageBox.ErrorQuery(_app, title, message);
    }
}

public interface IAppLifetime
{
    void Quit();
}

public class AppLifetime(IApplication app) : IAppLifetime
{
    private readonly IApplication _app = app;

    public void Quit() => _app.RequestStop();
}
