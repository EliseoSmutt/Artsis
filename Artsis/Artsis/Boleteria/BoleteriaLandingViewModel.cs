using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.DTOs;
using System.Collections.ObjectModel;


namespace Artsis.Boleteria
{
    public partial class BoleteriaLandingViewModel : ObservableObject
    {



        public BoleteriaLandingViewModel()
        {


        }

        [RelayCommand]

        private async Task BoleteriaPeliculas()
        {

            await Shell.Current.GoToAsync("//BoleteriaPeliculas");
        }

        [RelayCommand]
        private async Task BoleteriaFunciones()
        {

            await Shell.Current.GoToAsync("//Boleteria");
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
