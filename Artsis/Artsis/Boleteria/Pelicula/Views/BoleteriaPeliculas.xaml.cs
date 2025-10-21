using Artsis.Boleteria.Pelicula.ViewModels;

namespace Artsis.Boleteria.Pelicula.Views;

public partial class BoleteriaPeliculas : ContentPage
{
    public BoleteriaPeliculas(BoleteriaPeliculasViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

    }
}