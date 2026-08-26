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

    /// <summary>Compute a date-group label matching the web app's format (Today, Yesterday, Weekday, Month day, etc.).</summary>
    public static string GetDateLabel(DateTime createdAt, DateTime today)
    {
        var date = new DateTime(createdAt.Year, createdAt.Month, createdAt.Day);
        var t = new DateTime(today.Year, today.Month, today.Day);
        var diffDays = (int)Math.Round((t - date).TotalDays);

        if (diffDays == 0) return "Today";
        if (diffDays == 1) return "Yesterday";
        if (diffDays >= 2 && diffDays <= 6)
            return date.ToString("dddd"); // e.g., "Monday"
        if (date.Year == t.Year)
            return date.ToString("MMMM d"); // e.g., "August 17"
        return date.ToString("MMMM d, yyyy"); // e.g., "August 17, 2025"
    }
}
