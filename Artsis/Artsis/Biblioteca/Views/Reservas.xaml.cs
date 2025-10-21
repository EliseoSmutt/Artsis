using Artsis.Biblioteca.ViewModels;

namespace Artsis.Biblioteca.Views;

public partial class Reservas : ContentPage
{
	public Reservas(ReservasViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }
}