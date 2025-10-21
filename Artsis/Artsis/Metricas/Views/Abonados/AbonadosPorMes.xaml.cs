namespace Artsis.Metricas.Views.Abonados;

public partial class AbonadosPorMes : ContentPage
{
    public AbonadosPorMes()
    {
        InitializeComponent();
        CargarHTML();

    }

    private async void CargarHTML()
    {
        var htmlSource = new HtmlWebViewSource();
        var assembly = typeof(AbonadosPorMes).Assembly;
        using var stream = assembly.GetManifestResourceStream("Artsis.Metricas.Html.Abonados.GraficoAbonadosPorMes.html");
        using var reader = new StreamReader(stream);
        var htmlContent = await reader.ReadToEndAsync();

        htmlSource.Html = htmlContent;
        chartWebView.Source = htmlSource;
    }
    private async void OnMetricasButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Metricas");
    }
}