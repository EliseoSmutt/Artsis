using Artsis.Administracion.ViewModels;
using Artsis.Escuela;
using Artsis.Administracion;
using Artsis.Escuela.ViewModels;
using CommunityToolkit.Maui.Views;

namespace Artsis.Escuela.Views;

public partial class NuevoProfesor : Popup
{
    public NuevoProfesor()
    {
        var abonadoApiService = App.Services.GetService<AbonadoApiService>();
        var escuelaApiService = App.Services.GetService<EscuelaApiService>();
        BindingContext = new NuevoProfesorViewModel(this, escuelaApiService,abonadoApiService);
        InitializeComponent();
    }
    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is NuevoProfesorViewModel viewModel)
        {
            viewModel.MostrarLocalidades = true;
        }
    }

    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is NuevoProfesorViewModel viewModel)
        {
            viewModel.MostrarLocalidades = false;
        }
    }
}