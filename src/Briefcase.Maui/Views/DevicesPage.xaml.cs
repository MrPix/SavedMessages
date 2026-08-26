using Briefcase.Maui.ViewModels;

namespace Briefcase.Maui.Views;

public partial class DevicesPage : ContentPage
{
    public DevicesPage(DevicesViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
