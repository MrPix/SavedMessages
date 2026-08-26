using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Briefcase.Maui.Models;
using Briefcase.Maui.Services;

namespace Briefcase.Maui.ViewModels;

public partial class TrashViewModel : BaseViewModel
{
    public TrashViewModel(MockDataService data)
    {
        Title = "Trash";
        Items = new ObservableCollection<MockMessage>(data.GetTrash());
        IsEmpty = Items.Count == 0;
    }

    [ObservableProperty]
    private ObservableCollection<MockMessage> _items = [];

    [ObservableProperty]
    private bool _isEmpty;
}
