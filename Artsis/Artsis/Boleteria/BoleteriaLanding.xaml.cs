namespace Artsis.Boleteria;
using Artsis.Boleteria;

public partial class BoleteriaLanding : ContentPage
{
    public BoleteriaLanding(BoleteriaLandingViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

    }
}