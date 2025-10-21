namespace Artsis.Metricas.Views.Abonados;

public partial class RenovacionesAbonos : ContentPage
{
    public RenovacionesAbonos()
    {
        InitializeComponent();
        CargarHTML();

    }

    private async void CargarHTML()
    {
        var htmlSource = new HtmlWebViewSource();
        var assembly = typeof(AbonadosPorMes).Assembly;
        using var stream = assembly.GetManifestResourceStream("Artsis.Metricas.Html.Abonados.GraficoRenovacionesAbonos.html");
        using var reader = new StreamReader(stream);
        var htmlContent = await reader.ReadToEndAsync();

        var title = "Cantidad de personas que renovaron su abono";
        htmlContent = htmlContent.Replace("/*TITLE_PLACEHOLDER*/", title);

        htmlSource.Html = htmlContent;
        chartWebView.Source = htmlSource;
    }
    private async void OnMetricasButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Metricas");
    }
}