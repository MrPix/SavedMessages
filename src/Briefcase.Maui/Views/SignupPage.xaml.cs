using Briefcase.Maui.ViewModels;

namespace Briefcase.Maui.Views;

public partial class SignupPage : ContentPage
{
    public SignupPage(SignupViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
