using Artsis.Boleteria.Funcion.ViewModels;
using Artsis.Boleteria;
using Artsis.Escuela.ViewModels;
using CommunityToolkit.Maui.Views;
using Shared.DTOs;
namespace Artsis.Escuela.Views;

public partial class Inscripciones : ContentPage
{
	public Inscripciones(TallerDTO taller)
	{
        var escuelaApiService = App.Services.GetService<EscuelaApiService>();
        BindingContext = new InscripcionesViewModel(escuelaApiService, taller);
        InitializeComponent();
    }
}