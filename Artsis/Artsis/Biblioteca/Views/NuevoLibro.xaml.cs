using Artsis.Biblioteca.ViewModels;
using Artsis.Boleteria.Funcion.ViewModels;
using Artsis.Boleteria;
using Artsis.Models;
using CommunityToolkit.Maui.Views;
using Core.Entidades;

namespace Artsis.Biblioteca.Views;

public partial class NuevoLibro : Popup
{
    public NuevoLibro()
    {
        var bibliotecaApiService = App.Services.GetService<BibliotecaApiService>();
        BindingContext = new NuevoLibroViewModel(this, bibliotecaApiService);
        InitializeComponent();
    }
}