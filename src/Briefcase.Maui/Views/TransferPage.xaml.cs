using Briefcase.Maui.ViewModels;

namespace Briefcase.Maui.Views;

public partial class TransferPage : ContentPage
{
    public TransferPage(TransferViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
