using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Briefcase.Maui.Models;
using Briefcase.Maui.Services;

namespace Briefcase.Maui.ViewModels;

public partial class ClipboardViewModel : BaseViewModel
{
    private readonly MockDataService _data;

    public ClipboardViewModel(MockDataService data)
    {
        _data = data;
        Title = "Briefcase";
        BuildGroups();
    }

    public string[] Filters { get; } = ["All", "Favorites", "Files", "Links", "Notes"];

    [ObservableProperty]
    private string _selectedFilter = "All";

    [ObservableProperty]
    private ObservableCollection<MessageGroup> _groups = [];

    [ObservableProperty]
    private bool _isEmpty;

    // Compose bar text — visual only, not sent anywhere.
    [ObservableProperty]
    private string _draft = string.Empty;

    partial void OnSelectedFilterChanged(string value) => BuildGroups();

    private void BuildGroups()
    {
        var groups = new ObservableCollection<MessageGroup>();

        if (SelectedFilter == "All")
        {
            var pinned = _data.GetMessages().Where(m => m.IsPinned).ToList();
            if (pinned.Count > 0)
                groups.Add(new MessageGroup("Pinned", pinned));

            foreach (var group in GroupByDate(_data.GetMessages().Where(m => !m.IsPinned)))
                groups.Add(group);
        }
        else
        {
            var filtered = SelectedFilter switch
            {
                "Favorites" => _data.GetMessages(pinnedOnly: true),
                "Files" => _data.GetMessages(MessageKind.File),
                "Links" => _data.GetMessages(MessageKind.Url),
                "Notes" => _data.GetMessages(MessageKind.Text),
                _ => _data.GetMessages()
            };

            foreach (var group in GroupByDate(filtered))
                groups.Add(group);
        }

        Groups = groups;
        IsEmpty = groups.Count == 0;
    }

    private static IEnumerable<MessageGroup> GroupByDate(IEnumerable<MockMessage> messages) =>
        messages
            .GroupBy(m => m.DateGroup)
            .Select(g => new MessageGroup(g.Key, g));
}
