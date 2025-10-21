using Artsis.Biblioteca.ViewModels;
using Artsis.Models;
using CommunityToolkit.Maui.Views;

namespace Artsis.Biblioteca.Views;

public partial class NuevaReserva : Popup
{
    public NuevaReserva(List<LibroModel> libros)
    {
        var bibliotecaApiService = App.Services.GetService<BibliotecaApiService>();
        BindingContext = new NuevaReservaViewModel(this, bibliotecaApiService, libros);
        InitializeComponent();
    }
}