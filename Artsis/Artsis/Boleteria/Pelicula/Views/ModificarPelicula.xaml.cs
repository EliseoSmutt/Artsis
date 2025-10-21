using Artsis.Boleteria.Pelicula.ViewModels;
using CommunityToolkit.Maui.Views;
using Shared.DTOs;

namespace Artsis.Boleteria.Pelicula.Views;


public partial class ModificarPelicula : Popup
{
    public ModificarPelicula(PeliculaDTO pelicula)
{
    var boleteriaApiService = App.Services.GetService<BoleteriaApiService>();
    BindingContext = new ModificarPeliculaViewModel(this, boleteriaApiService, pelicula);
    InitializeComponent();
}
}