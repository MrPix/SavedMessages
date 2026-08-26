using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Briefcase.Maui.Models;
using Briefcase.Maui.Services;

namespace Briefcase.Maui.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    public SettingsViewModel(MockDataService data)
    {
        Title = "Settings";
        Devices = new ObservableCollection<MockDevice>(data.GetDevices());
        _selectedTheme = Application.Current?.UserAppTheme switch
        {
            AppTheme.Light => "Light",
            AppTheme.Dark => "Dark",
            _ => "System"
        };
    }

    [ObservableProperty]
    private ObservableCollection<MockDevice> _devices = [];

    // ── Appearance (the one live setting) ─────────────────────────────────────
    public string[] Themes { get; } = ["System", "Light", "Dark"];

    [ObservableProperty]
    private string _selectedTheme;

    partial void OnSelectedThemeChanged(string value)
    {
        if (Application.Current is null) return;
        Application.Current.UserAppTheme = value switch
        {
            "Light" => AppTheme.Light,
            "Dark" => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };
    }

    // ── Language (visual only) ────────────────────────────────────────────────
    public string[] Languages { get; } = ["English", "Українська"];

    [ObservableProperty]
    private string _selectedLanguage = "English";

    // ── End-to-end encryption (visual only) ───────────────────────────────────
    [ObservableProperty]
    private bool _e2eeEnabled;

    // ── Change password (visual only) ─────────────────────────────────────────
    [ObservableProperty]
    private string _currentPassword = string.Empty;

    [ObservableProperty]
    private string _newPassword = string.Empty;

    [ObservableProperty]
    private string _confirmPassword = string.Empty;

    public string AppVersion => $"Version {AppInfo.Current.VersionString}";

    [RelayCommand]
    private Task LogOut() => GoToAsync("//landing");
}
