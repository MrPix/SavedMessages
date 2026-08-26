using Briefcase.Maui.Models;

namespace Briefcase.Maui.Services;

/// <summary>In-memory sample data for the UI mockups. No networking or persistence.</summary>
public class MockDataService
{
    private readonly List<MockMessage> _messages;
    private readonly List<MockMessage> _trash;
    private readonly List<MockDevice> _devices;

    public MockDataService()
    {
        var now = DateTime.Now;

        _messages =
        [
            new MockMessage { Kind = MessageKind.Text, Content = "Wi-Fi password: sunset-harbor-42", IsPinned = true, CreatedAt = now.AddMinutes(-8), DateGroup = "Today" },
            new MockMessage { Kind = MessageKind.Url, Content = "https://github.com/MrPix/Briefcase", IsPinned = true, CreatedAt = now.AddHours(-2), DateGroup = "Today" },
            new MockMessage { Kind = MessageKind.Text, Content = "Remember to renew the domain briefcase.page before June.", CreatedAt = now.AddHours(-3), DateGroup = "Today" },
            new MockMessage { Kind = MessageKind.File, FileName = "boarding-pass.pdf", FileComment = "Flight LX318 — gate B24", CreatedAt = now.AddHours(-5), DateGroup = "Today" },
            new MockMessage { Kind = MessageKind.Url, Content = "https://maps.app.goo.gl/8xQ2mP", FileComment = "Meeting spot", CreatedAt = now.AddHours(-6), DateGroup = "Today" },
            new MockMessage { Kind = MessageKind.Text, Content = "Grocery list: oat milk, avocados, coffee beans, sourdough", CreatedAt = now.AddDays(-1).AddHours(-1), DateGroup = "Yesterday" },
            new MockMessage { Kind = MessageKind.File, FileName = "design-mockup.png", FileComment = "v3 — dark theme", CreatedAt = now.AddDays(-1).AddHours(-4), DateGroup = "Yesterday" },
            new MockMessage { Kind = MessageKind.Url, Content = "https://learn.microsoft.com/dotnet/maui", CreatedAt = now.AddDays(-1).AddHours(-5), DateGroup = "Yesterday" },
            new MockMessage { Kind = MessageKind.Text, Content = "Parking level P2, spot 118", CreatedAt = now.AddDays(-3), DateGroup = "Earlier" },
            new MockMessage { Kind = MessageKind.File, FileName = "invoice-2026-04.pdf", CreatedAt = now.AddDays(-4), DateGroup = "Earlier" },
        ];

        _trash =
        [
            new MockMessage { Kind = MessageKind.Text, Content = "Old draft — call the dentist", CreatedAt = now.AddDays(-2), DateGroup = "Trash" },
            new MockMessage { Kind = MessageKind.Url, Content = "https://example.com/expired-link", CreatedAt = now.AddDays(-6), DateGroup = "Trash" },
        ];

        _devices =
        [
            new MockDevice { Name = "Surface Laptop", Platform = ClientPlatform.Windows, LastSeenAt = now, IsCurrent = true },
            new MockDevice { Name = "Pixel 8 Pro", Platform = ClientPlatform.Android, LastSeenAt = now.AddHours(-1) },
            new MockDevice { Name = "iPhone 15", Platform = ClientPlatform.iOS, LastSeenAt = now.AddDays(-1) },
            new MockDevice { Name = "Chrome · MacBook", Platform = ClientPlatform.Web, LastSeenAt = now.AddDays(-2) },
        ];
    }

    public IReadOnlyList<MockMessage> GetMessages(MessageKind? kind = null, bool pinnedOnly = false)
    {
        IEnumerable<MockMessage> query = _messages;
        if (pinnedOnly) query = query.Where(m => m.IsPinned);
        if (kind is not null) query = query.Where(m => m.Kind == kind);
        return query.ToList();
    }

    public IReadOnlyList<MockMessage> GetTrash() => _trash;

    public IReadOnlyList<MockDevice> GetDevices() => _devices;
}
