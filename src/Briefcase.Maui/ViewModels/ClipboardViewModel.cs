using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

    [ObservableProperty]
    private ObservableCollection<MessageGroup> _groups = [];

    [ObservableProperty]
    private bool _isEmpty;

    // Compose bar text — visual only, not sent anywhere.
    [ObservableProperty]
    private string _draft = string.Empty;

    // Category filtering moved to drawer; show all (pinned + by date).
    private void BuildGroups()
    {
        var groups = new ObservableCollection<MessageGroup>();

        var all = _data.GetMessages();
        var pinned = all.Where(m => m.IsPinned).ToList();
        if (pinned.Count > 0)
            groups.Add(new MessageGroup("Pinned", pinned));

        foreach (var group in GroupByDate(all.Where(m => !m.IsPinned)))
            groups.Add(group);

        Groups = groups;
        IsEmpty = groups.Count == 0;
    }

    private static IEnumerable<MessageGroup> GroupByDate(IEnumerable<MockMessage> messages) =>
        messages
            .GroupBy(m => m.DateGroup)
            .Select(g => new MessageGroup(g.Key, g));

    [RelayCommand]
    private void SaveMessage()
    {
        // Visual-only mock: clear the draft text.
        Draft = string.Empty;
    }
}
