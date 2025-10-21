using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace Artsis.Administracion.ViewModels
{
    public partial class AdministradorLandingViewModel : ObservableObject
    {



        public AdministradorLandingViewModel()
        {


        }


        [RelayCommand]
        private async Task SociosAbonados()
        {

            await Shell.Current.GoToAsync("//Administrador");
        }
        [RelayCommand]
        private async Task GoToMetricas()
        {

            await Shell.Current.GoToAsync("//Metricas");
        }


        [RelayCommand]
        private async Task CerrarSesion()
        {
            // Mostrar el cuadro de diálogo de confirmación
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Deseas cerrar sesión?", "Sí", "No");

            // Si el usuario selecciona "Sí", redirigir al login
            if (confirm)
            {
                await Shell.Current.GoToAsync("//Login");
            }
        }
    }
}
