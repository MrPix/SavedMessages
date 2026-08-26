using Briefcase.Maui.ViewModels;

namespace Briefcase.Maui.Views;

public partial class TrashPage : ContentPage
{
    public TrashPage(TrashViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
