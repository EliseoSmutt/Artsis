namespace Artsis.Escuela.Views;
using Artsis.Escuela;
using global::Artsis.Escuela.ViewModels;

public partial class Escuela : ContentPage
{
    public Escuela(EscuelaViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

    }
}