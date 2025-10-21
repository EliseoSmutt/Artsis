using Artsis.Biblioteca.ViewModels;

namespace Artsis.Biblioteca.Views;

public partial class BibliotecaLanding : ContentPage
{
    public BibliotecaLanding(BibliotecaLandingViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

    }
}