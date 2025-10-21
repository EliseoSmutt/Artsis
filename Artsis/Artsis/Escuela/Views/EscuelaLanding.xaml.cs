namespace Artsis.Escuela.Views;
using Artsis.Escuela;
using global::Artsis.Escuela.ViewModels;

public partial class EscuelaLanding : ContentPage
{
    public EscuelaLanding(EscuelaLandingViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

    }
}