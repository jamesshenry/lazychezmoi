using LazyChezmoi.Commands;
using LazyChezmoi.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.SystemConsole.Themes;

namespace LazyChezmoi.Services;

public static class ServiceExtensions
{
    public static IConfiguration CreateConfiguration()
    {
        var configBuilder = new ConfigurationBuilder().AddJsonFile(
            "config.json",
            optional: false,
            reloadOnChange: true
        );

        return configBuilder.Build();
    }

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
                .WriteTo.Console(
                    outputTemplate: outputTemplate,
                    theme: AnsiConsoleTheme.Sixteen,
                    restrictedToMinimumLevel: LogEventLevel.Information
                )
                .CreateLogger()
        );
    }

    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        var configuration = CreateConfiguration();

        services.AddLogging(ConfigureSerilog);
        services.AddSingleton(configuration);
        services.AddSingleton<IService, ServiceImplementation>();
        services.AddSingleton<MyCommands>();

        return services;
    }
}
