using System.Diagnostics;
namespace Artsis.Login;


public partial class Login : ContentPage
{
    public Login(LoginViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

    }
    //TODO: NO EN EL CODEBEHIND
    void IniciarSesion(object sender, EventArgs e)
    {
        var button = (Button)sender;
    }

}