using Artsis.Administracion.ViewModels;
using CommunityToolkit.Maui.Views;

namespace Artsis.Administracion.Views;

public partial class NuevoAbonado : Popup
{
    public NuevoAbonado()
    {
        var abonadoApiService = App.Services.GetService<AbonadoApiService>();
        BindingContext = new NuevoAbonadoViewModel(this, abonadoApiService);
        InitializeComponent();
    }

    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is NuevoAbonadoViewModel viewModel)
        {
            viewModel.MostrarLocalidades = true;
        }
    }

    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is NuevoAbonadoViewModel viewModel)
        {
            viewModel.MostrarLocalidades = false;
        }
    }

}