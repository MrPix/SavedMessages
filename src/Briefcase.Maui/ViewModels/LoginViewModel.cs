using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Briefcase.Maui.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    public LoginViewModel()
    {
        Title = "Sign in";
    }

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [RelayCommand]
    private Task SignIn() => GoToAsync("//clipboard");

    [RelayCommand]
    private Task CreateAccount() => GoToAsync("//signup");
}
