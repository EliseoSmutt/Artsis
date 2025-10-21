using Artsis.Metricas.Views.Abonados;

namespace Artsis.Metricas.Views.Funciones;

public partial class EntradasVendidasPorMes : ContentPage
{
    public EntradasVendidasPorMes()
	{
		InitializeComponent();
        CargarHTML();

    }

    private async void CargarHTML()
    {
        var htmlSource = new HtmlWebViewSource();
        var assembly = typeof(AbonadosPorMes).Assembly;
        using var stream = assembly.GetManifestResourceStream("Artsis.Metricas.Html.Funciones.GraficoEntradasVendidasPorMes.html");
        using var reader = new StreamReader(stream);
        var htmlContent = await reader.ReadToEndAsync();

        var title = "Cantidad de entradas vendidas por mes";
        htmlContent = htmlContent.Replace("/*TITLE_PLACEHOLDER*/", title);

        htmlSource.Html = htmlContent;
        chartWebView.Source = htmlSource;
    }
    private async void OnMetricasButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Metricas");
    }
}