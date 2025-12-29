using System.Text.Json;
using ConsoleAppFramework;
using LazyChezmoi.Configuration;
using LazyChezmoi.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LazyChezmoi.Commands;

public class MyCommands(
    ILogger<MyCommands> logger,
    IService service,
    IOptions<LazyChezmoiSettings> options
)
{
    private readonly LazyChezmoiSettings config = options.Value;

    /// <summary>Root command test.</summary>
    /// <param name="msg">-m, Message to show.</param>
    [Command("")]
    public void Root(string msg)
    {
        logger.LogInformation($"Hello from logger");
        service.DoSomething();
        Console.WriteLine(msg);
    }

    /// <summary>Display message.</summary>
    /// <param name="msg">Message to show.</param>
    public void Echo(string msg) => Console.WriteLine(msg);

    /// <summary>Sum parameters.</summary>
    /// <param name="x">left value.</param>
    /// <param name="y">right value.</param>
    public void Sum(int x, int y) => Console.WriteLine(x + y);

    [Command("config")]
    public void Config()
    {
        var opts = options;
        logger.LogInformation("Displaying IOptions wrapped config");

        var text = JsonSerializer.Serialize(
            config,
            typeof(LazyChezmoiSettings),
            LazyChezmoiSettingsContext.Default
        );

        Console.WriteLine(text);
    }
}
