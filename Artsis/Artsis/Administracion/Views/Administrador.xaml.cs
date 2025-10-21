using Artsis.Administracion.ViewModels;

namespace Artsis.Administracion.Views;

public partial class Administrador : ContentPage
{

    public Administrador(AdministradorViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);



    }

}