using Artsis.Biblioteca.Views;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.DTOs;
using System.Collections.ObjectModel;

namespace Artsis.Biblioteca.ViewModels
{
    public partial class BibliotecaLandingViewModel : ObservableValidator
    {




        public BibliotecaLandingViewModel()
        {



        }

        [RelayCommand]
        private async Task Biblioteca()
        {

            await Shell.Current.GoToAsync("//Biblioteca");
        }
        [RelayCommand]
        private async Task Reservas()
        {

            await Shell.Current.GoToAsync("//Reservas");
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