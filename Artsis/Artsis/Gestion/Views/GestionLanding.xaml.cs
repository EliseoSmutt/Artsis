using Artsis.Gestion.ViewModels;

namespace Artsis.Gestion.Views;

public partial class GestionLanding : ContentPage
{
    public GestionLanding(GestionLandingViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);



    }
}