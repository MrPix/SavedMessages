using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Briefcase.Maui.ViewModels;

public partial class SignupViewModel : BaseViewModel
{
    public SignupViewModel()
    {
        Title = "Create account";
    }

    [ObservableProperty]
    private string _displayName = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _confirmPassword = string.Empty;

    [RelayCommand]
    private Task CreateAccount() => GoToAsync("//clipboard");

    [RelayCommand]
    private Task SignIn() => GoToAsync("//login");
}
