namespace Extra.Tests.Utils;

public class ConfigurationOptions
{
    public string? PathToChrome { get; init; }

    public string? CacheDirectory { get; init; }

    public string? BuildId { get; init; }

    public bool Headless { get; init; }

    public bool ShouldBeInstall { get; init; }
}
