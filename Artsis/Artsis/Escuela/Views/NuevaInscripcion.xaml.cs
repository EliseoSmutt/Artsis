using Artsis.Escuela;
using Artsis.Escuela.ViewModels;
using CommunityToolkit.Maui.Views;
using Artsis.Administracion;

namespace Artsis.Escuela.Views;

public partial class NuevaInscripcion : Popup
{
    public NuevaInscripcion(int Id)
    {
        var abonadoApiService = App.Services.GetService<AbonadoApiService>();
        var escuelaApiService = App.Services.GetService<EscuelaApiService>();
        BindingContext = new NuevaInscripcionViewModel(this, escuelaApiService,Id,abonadoApiService);
        InitializeComponent();
    }
    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is NuevaInscripcionViewModel viewModel)
        {
            viewModel.MostrarLocalidades = true;
        }
    }

    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is NuevaInscripcionViewModel viewModel)
        {
            viewModel.MostrarLocalidades = false;
        }
    }
}