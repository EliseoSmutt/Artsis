using Artsis.Commons;
using Artsis.Gestion.ViewModels;

namespace Artsis.Gestion.Views;

public partial class Gestion : ContentPage
{
    public Gestion()
    {
        var empleadoApiService = App.Services.GetService<EmpleadoApiService>();
        var commonsApiService = App.Services.GetService<CommonsApiService>();
        BindingContext = new GestionViewModel(empleadoApiService,commonsApiService); 
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

    }



}