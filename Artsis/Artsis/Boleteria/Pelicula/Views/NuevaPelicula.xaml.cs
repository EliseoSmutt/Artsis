using Artsis.Boleteria.Pelicula.ViewModels;
using CommunityToolkit.Maui.Views;
using Shared.Requests;

namespace Artsis.Boleteria.Pelicula.Views;

public partial class NuevaPelicula : Popup
{
    public NuevaPelicula(PeliculaRequest pelicula)
    {
        var bibliotecaApiService = App.Services.GetService<BoleteriaApiService>();
        BindingContext = new NuevaPeliculaViewModel(this, bibliotecaApiService, pelicula);
        InitializeComponent();
    }
}