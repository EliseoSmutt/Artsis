using Artsis.Biblioteca.ViewModels;

namespace Artsis.Biblioteca.Views;

public partial class Biblioteca : ContentPage
{
    public Biblioteca(BibliotecaViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

    }
}