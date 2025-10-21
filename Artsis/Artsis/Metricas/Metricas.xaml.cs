namespace Artsis.Metricas;

public partial class Metricas : ContentPage
{
    public Metricas()
    {
        BindingContext = new MetricasViewModel();
        InitializeComponent();
       
    }
}