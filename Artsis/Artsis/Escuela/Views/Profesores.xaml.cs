namespace Artsis.Escuela.Views;
using Artsis.Escuela;
using global::Artsis.Escuela.ViewModels;

public partial class Profesores : ContentPage
{
    public Profesores(ProfesoresViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }
}