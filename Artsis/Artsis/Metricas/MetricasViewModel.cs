using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Artsis.Metricas
{
    public partial class MetricasViewModel : ObservableObject
    {

        public MetricasViewModel()
        {

        }

        [RelayCommand]
        public static async Task VerAbonadosPorMes()
        {
            await Shell.Current.GoToAsync("//AbonadosPorMes");
        }

        [RelayCommand]
        public static async Task VerEntradasPorMes()
        {
            await Shell.Current.GoToAsync("//EntradasVendidasPorMes");
        }

        [RelayCommand]
        public static async Task VerAbonadosConReservas()
        {
            await Shell.Current.GoToAsync("//AbonadosConReservas");
        }

        [RelayCommand]
        public static async Task VerGeneroMasRecaudador()
        {
            await Shell.Current.GoToAsync("//GeneroMasRecaudador");
        }
        [RelayCommand]
        public static async Task VerRenovacionesAbonos()
        {
            await Shell.Current.GoToAsync("//RenovacionesAbonos");
        }
        [RelayCommand]
        private static async Task SociosAbonados()
        {

            await Shell.Current.GoToAsync("//Administrador");
        }

       



        [RelayCommand]
        private static async Task CerrarSesion()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Deseas cerrar sesión?", "Sí", "No");

            if (confirm)
            {
                await Shell.Current.GoToAsync("//Login");
            }
        }
        [RelayCommand]
        private static async Task Volver()
        {
                await Shell.Current.GoToAsync("//AdministradorLanding");            
        }
    }
}
