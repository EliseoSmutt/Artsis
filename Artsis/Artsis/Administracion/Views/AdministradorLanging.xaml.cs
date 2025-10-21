namespace Artsis.Administracion.Views;
using Artsis.Administracion.ViewModels;

public partial class AdministradorLanding : ContentPage
{
    public AdministradorLanding(AdministradorLandingViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }
}