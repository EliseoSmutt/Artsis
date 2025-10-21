using Artsis.Administracion.ViewModels;
using CommunityToolkit.Maui.Views;
using Shared.DTOs;

namespace Artsis.Administracion.Views;

public partial class ModificarAbonado : Popup
{
    public ModificarAbonado(AbonadoDTO abonado)
    {
        var abonadoApiService = App.Services.GetService<AbonadoApiService>();
        BindingContext = new ModificarAbonadoViewModel(this, abonadoApiService, abonado);
        InitializeComponent();
    }

    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is ModificarAbonadoViewModel viewModel)
        {
            viewModel.MostrarLocalidades = true;
        }
    }

    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is ModificarAbonadoViewModel viewModel)
        {
            viewModel.MostrarLocalidades = false;
        }
    }
}