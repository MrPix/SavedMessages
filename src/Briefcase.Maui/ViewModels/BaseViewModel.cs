using CommunityToolkit.Mvvm.ComponentModel;

namespace Briefcase.Maui.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title = string.Empty;

    protected static Task GoToAsync(string route) => Shell.Current.GoToAsync(route);
}
