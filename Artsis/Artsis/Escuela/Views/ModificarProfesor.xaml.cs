using CommunityToolkit.Maui.Views;
using Shared.DTOs;
using Artsis.Escuela.ViewModels;
using Artsis.Administracion;

namespace Artsis.Escuela.Views;

public partial class ModificarProfesor : Popup
{
	public ModificarProfesor(TalleristaDTO tallerista)
	{
        var abonadoApiService = App.Services.GetService<AbonadoApiService>();
        var escuelaApiService = App.Services.GetService<EscuelaApiService>();
        BindingContext = new ModificarProfesorViewModel(this, escuelaApiService, tallerista,abonadoApiService);
        InitializeComponent();
	}
    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is ModificarProfesorViewModel viewModel)
        {
            viewModel.MostrarLocalidades = true;
        }
    }

    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is ModificarProfesorViewModel viewModel)
        {
            viewModel.MostrarLocalidades = false;
        }
    }
}