using CommunityToolkit.Mvvm.Input;

namespace Briefcase.Maui.ViewModels;

public partial class LandingViewModel : BaseViewModel
{
    public LandingViewModel()
    {
        Title = "Welcome";
    }

    [RelayCommand]
    private Task SignIn() => GoToAsync("//login");

    [RelayCommand]
    private Task CreateAccount() => GoToAsync("//signup");

    [RelayCommand]
    private Task ReceiveFile() => GoToAsync("//transfer");
}
