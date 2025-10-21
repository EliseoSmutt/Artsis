using Artsis.Boleteria.Funcion.ViewModels;
using CommunityToolkit.Maui.Views;
using Shared.DTOs;

namespace Artsis.Boleteria.Funcion.Views;

public partial class ModificarFuncion : Popup
{
    public ModificarFuncion(FuncionDTO funcion)
    {
        var boleteriaApiService = App.Services.GetService<BoleteriaApiService>();
        BindingContext = new ModificarFuncionViewModel(this, boleteriaApiService, funcion);
        InitializeComponent();
    }
}