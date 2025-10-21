using Shared.DTOs;
using Artsis.Escuela.ViewModels;
using CommunityToolkit.Maui.Views;
using Shared.DTOs;


namespace Artsis.Escuela.Views;

public partial class ModificarTaller : Popup
{
    public ModificarTaller(TallerDTO taller)
{
    var escuelaApiService = App.Services.GetService<EscuelaApiService>();
    BindingContext = new ModificarTallerViewModel(this, escuelaApiService, taller);
    InitializeComponent();
}
    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is ModificarTallerViewModel viewModel)
        {
            viewModel.MostrarTalleristas = true;
        }
    }

    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is ModificarTallerViewModel viewModel)
        {
            viewModel.MostrarTalleristas = false;
        }
    }
}