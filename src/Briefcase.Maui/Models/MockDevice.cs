namespace Briefcase.Maui.Models;

public enum ClientPlatform
{
    Windows,
    Android,
    iOS,
    macOS,
    Web
}

/// <summary>Lightweight mock device shown on the Devices and Settings screens.</summary>
public class MockDevice
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = string.Empty;
    public ClientPlatform Platform { get; init; }
    public DateTime LastSeenAt { get; init; }
    public bool IsCurrent { get; init; }

    public string PlatformLabel => Platform switch
    {
        ClientPlatform.iOS => "iOS",
        ClientPlatform.macOS => "macOS",
        _ => Platform.ToString()
    };

    public string LastSeenLabel => IsCurrent
        ? "This device"
        : $"Last seen {LastSeenAt:MMM d, h:mm tt}";
}
