namespace Briefcase.Maui.Models;

public enum MessageKind
{
    Text,
    Url,
    File
}

/// <summary>Lightweight mock message used to populate the UI without any backend.</summary>
public class MockMessage
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public MessageKind Kind { get; init; }
    public string? Content { get; init; }
    public string? FileName { get; init; }
    public string? FileComment { get; init; }
    public bool IsPinned { get; init; }
    public DateTime CreatedAt { get; init; }
    public string DateGroup { get; init; } = string.Empty;

    // ── Display helpers for XAML bindings ─────────────────────────────────────
    public bool IsFile => Kind == MessageKind.File;
    public bool HasComment => !string.IsNullOrWhiteSpace(FileComment);
    public string PrimaryText => IsFile ? (FileName ?? "Attachment") : (Content ?? string.Empty);
    public string TimeLabel => CreatedAt.ToString("h:mm tt");

    public string IconSource => Kind switch
    {
        MessageKind.Url => "icon_link.png",
        MessageKind.File => "icon_file.png",
        _ => "icon_text.png"
    };

    public string KindLabel => Kind switch
    {
        MessageKind.Url => "Link",
        MessageKind.File => "File",
        _ => "Note"
    };
}
