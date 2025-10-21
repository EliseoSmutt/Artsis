using Artsis.Boleteria.Funcion.ViewModels;
using CommunityToolkit.Maui.Views;
using Shared.Requests;

namespace Artsis.Boleteria.Funcion.Views;

public partial class NuevaFuncion : Popup
{
    public NuevaFuncion(FuncionRequest funcion)
    {
        var bibliotecaApiService = App.Services.GetService<BoleteriaApiService>();
        BindingContext = new NuevaFuncionViewModel(this, bibliotecaApiService, funcion);
        InitializeComponent();
    }
}