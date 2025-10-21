using Artsis.Escuela.Views;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.DTOs;
using System.Threading.Tasks;

using System.Collections.ObjectModel;

namespace Artsis.Escuela.ViewModels
{
    public partial class ProfesoresViewModel : ObservableValidator
    {

        private readonly EscuelaApiService _escuelaApiService;
        private bool _tablaCompleta;

        [ObservableProperty]
        private ObservableCollection<TalleristaDTO> _items;

        [ObservableProperty]
        private string _nroFila;

        [ObservableProperty]
        private string _nombreTallerista;
        [ObservableProperty]
        private string _dni;
        [ObservableProperty]
        private string _nombre;
        [ObservableProperty]
        private string _apellido;
        [ObservableProperty]
        private string _telefono;
        [ObservableProperty]
        private string _email;
        [ObservableProperty]
        private string _direccion;
        [ObservableProperty]
        private int _id;


        public ProfesoresViewModel(EscuelaApiService escuelaApiService)
        {
            _escuelaApiService = escuelaApiService;
            Items = new ObservableCollection<TalleristaDTO>();
            
        }

        [RelayCommand]
        private async Task Buscar()
        {
            if (_tablaCompleta) return;

            _tablaCompleta = true;
            Items.Clear();
            await CargarItems();
            _tablaCompleta = false;
        }
        [RelayCommand]
        private async Task CargarMas()
        {
            if (_tablaCompleta) return;

            _tablaCompleta = true;
            await CargarItems();
            _tablaCompleta = false;
        }

        [RelayCommand]
        private async Task CargarItems()
        {
            var resultado = await _escuelaApiService.ConsultarTalleristas();

            foreach (var item in resultado)
            {
                item.NroFila = (resultado.IndexOf(item) + 1).ToString();
                Items.Add(item);
            }
        }

        [RelayCommand]
        private async Task ModificarTallerista(TalleristaDTO tallerista)
        {
            try
            {
                var popup = new ModificarProfesor(tallerista);
                await Application.Current.MainPage.ShowPopupAsync(popup);
                await Buscar();
            }
            catch(Exception)
            {
                throw;
            }         
        }

        [RelayCommand]
        private async Task RegistrarTallerista()
        {
            try
            {
                var popup = new NuevoProfesor();
                await Application.Current.MainPage.ShowPopupAsync(popup);
                await Buscar();
            }
            catch(Exception) { throw; }
        }
        [RelayCommand]
        private async Task Escuela()
        {

            await Shell.Current.GoToAsync("//Escuela");
        }
        [RelayCommand]
        private async Task BorrarProfesor(string dni)
        {

            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Deseas eliminar este Profesor?", "Sí", "No");
            if (confirm)
            {
                await _escuelaApiService.BorrarProfesor(dni);
                await Application.Current.MainPage.DisplayAlert("Eliminar Profesor", "Profesor Eliminado Con Exito", "Ok");
                await Buscar();
            }
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