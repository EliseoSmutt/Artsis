namespace Artsis.Boleteria;

public partial class Boleteria : ContentPage
{
    public Boleteria(BoleteriaViewModel vm)
    {
        BindingContext = vm;        
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
       

    }
}