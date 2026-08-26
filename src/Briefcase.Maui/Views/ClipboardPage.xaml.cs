using Briefcase.Maui.ViewModels;

namespace Briefcase.Maui.Views;

public partial class ClipboardPage : ContentPage
{
    public ClipboardPage(ClipboardViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
