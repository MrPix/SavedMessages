using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Briefcase.Maui.ViewModels;

public partial class TransferViewModel : BaseViewModel
{
    public TransferViewModel()
    {
        Title = "Transfer";
    }

    // "device" or "link" — drives the tab visuals.
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsDeviceTab))]
    [NotifyPropertyChangedFor(nameof(IsLinkTab))]
    private string _selectedTab = "device";

    public bool IsDeviceTab => SelectedTab == "device";
    public bool IsLinkTab => SelectedTab == "link";

    // Receive mode shows a code other devices can enter.
    public string ReceiveCode { get; } = "K7P2QX";

    [ObservableProperty]
    private string _enteredCode = string.Empty;

    public string[] ExpiryOptions { get; } = ["1 hour", "24 hours", "7 days", "Never"];

    [ObservableProperty]
    private string _selectedExpiry = "24 hours";

    [ObservableProperty]
    private bool _selfDestruct;

    [RelayCommand]
    private void ShowDeviceTab() => SelectedTab = "device";

    [RelayCommand]
    private void ShowLinkTab() => SelectedTab = "link";
}
