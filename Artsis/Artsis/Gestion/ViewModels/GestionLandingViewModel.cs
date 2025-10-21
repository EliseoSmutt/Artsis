using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace Artsis.Gestion.ViewModels
{
    public partial class GestionLandingViewModel : ObservableObject
    {



        public GestionLandingViewModel()
        {


        }


        [RelayCommand]
        private async Task Usuarios()
        {

            await Shell.Current.GoToAsync("//Gestion");
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
