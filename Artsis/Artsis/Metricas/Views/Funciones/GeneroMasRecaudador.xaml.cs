using Artsis.Metricas.Views.Abonados;
using System.Text;

namespace Artsis.Metricas.Views.Funciones;

public partial class GeneroMasRecaudador : ContentPage
{
    public GeneroMasRecaudador()
	{
		InitializeComponent();
        CargarHTML();

    }

    private async void CargarHTML()
    {
        var htmlSource = new HtmlWebViewSource();
        var assembly = typeof(GeneroMasRecaudador).Assembly;
        using var stream = assembly.GetManifestResourceStream("Artsis.Metricas.Html.Funciones.GraficoGeneroMasRecaudador.html");
       // using var reader = new StreamReader(stream);
        using var reader = new StreamReader(stream, Encoding.UTF8);
        var htmlContent = await reader.ReadToEndAsync();

        var title = "Cantidad de entradas vendidas por género";
        htmlContent = htmlContent.Replace("/*TITLE_PLACEHOLDER*/", title);

        htmlSource.Html = htmlContent;
        chartWebView.Source = htmlSource;
    }
    private async void OnMetricasButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Metricas");
    }
}