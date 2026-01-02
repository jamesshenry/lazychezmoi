using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace LazyChezmoi.Configuration;

public class LazyChezmoiSettings
{
    public int Port { get; set; }
    public bool Enabled { get; set; }

    [ConfigurationKeyName("api-url")]
    public string? ApiUrl { get; set; }
}

[JsonSourceGenerationOptions(
    WriteIndented = true,
    AllowTrailingCommas = true,
    UseStringEnumConverter = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    PropertyNameCaseInsensitive = true
)]
[JsonSerializable(typeof(LazyChezmoiSettings))]
public partial class LazyChezmoiSettingsContext : JsonSerializerContext;
