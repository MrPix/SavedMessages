using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Briefcase.Maui.Models;
using Briefcase.Maui.Services;

namespace Briefcase.Maui.ViewModels;

public partial class DevicesViewModel : BaseViewModel
{
    public DevicesViewModel(MockDataService data)
    {
        Title = "Devices";
        Devices = new ObservableCollection<MockDevice>(data.GetDevices());
    }

    [ObservableProperty]
    private ObservableCollection<MockDevice> _devices = [];
}
