using Artsis.Escuela;
using Artsis.Escuela.ViewModels;
using CommunityToolkit.Maui.Views;

namespace Artsis.Escuela.Views;

public partial class NuevoTaller : Popup
{
    public NuevoTaller()
    {
        var escuelaApiService = App.Services.GetService<EscuelaApiService>();
        BindingContext = new NuevoTallerViewModel(this, escuelaApiService);
        InitializeComponent();
    }

    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is NuevoTallerViewModel viewModel)
        {
            viewModel.MostrarTalleristas = true;
        }
    }

    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is NuevoTallerViewModel viewModel)
        {
            viewModel.MostrarTalleristas = false;
        }
    }
}