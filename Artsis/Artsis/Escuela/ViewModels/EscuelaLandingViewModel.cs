using Artsis.Escuela.Views;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.DTOs;
using System.Collections.ObjectModel;

namespace Artsis.Escuela.ViewModels
{
    public partial class EscuelaLandingViewModel : ObservableValidator
    {




        public EscuelaLandingViewModel()
        {



        }
        [RelayCommand]
        private async Task Profesores()
        {

            await Shell.Current.GoToAsync("//Profesores");
        }


        [RelayCommand]
        private async Task Escuela()
        {

            await Shell.Current.GoToAsync("//Escuela");
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