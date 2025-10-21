using Artsis.Administracion;
using Artsis.Commons;
using Artsis.Gestion.ViewModels;
using CommunityToolkit.Maui.Views;
using Shared.DTOs;

namespace Artsis.Gestion.Views;

public partial class ModificarEmpleado : Popup
{
    public ModificarEmpleado(EmpleadoDTO empleado)
    {
        var empleadoApiService = App.Services.GetService<EmpleadoApiService>();
        var abonadoApiService = App.Services.GetService<AbonadoApiService>();
        var commonsApiService = App.Services.GetService<CommonsApiService>();
        BindingContext = new ModificarEmpleadoViewModel(this, empleadoApiService, empleado, abonadoApiService, commonsApiService);
        InitializeComponent();
    }

    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is ModificarEmpleadoViewModel viewModel)
        {
            viewModel.MostrarLocalidades = true;
        }
    }

    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is ModificarEmpleadoViewModel viewModel)
        {
            viewModel.MostrarLocalidades = false;
        }
    }
}