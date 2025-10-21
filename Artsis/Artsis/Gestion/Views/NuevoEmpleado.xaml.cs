using Artsis.Administracion;
using Artsis.Gestion.ViewModels;
using CommunityToolkit.Maui.Views;

namespace Artsis.Gestion.Views;

public partial class NuevoEmpleado : Popup
{
    public NuevoEmpleado()
    {
        var abonadoApiService = App.Services.GetService<AbonadoApiService>();
        var empeladoApiService = App.Services.GetService<EmpleadoApiService>();
        BindingContext = new NuevoEmpleadoViewModel(this, empeladoApiService, abonadoApiService);
        InitializeComponent();
    }

    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is NuevoEmpleadoViewModel viewModel)
        {
            viewModel.MostrarLocalidades = true;
        }
    }

    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is NuevoEmpleadoViewModel viewModel)
        {
            viewModel.MostrarLocalidades = false;
        }
    }

}