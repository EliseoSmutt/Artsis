using Artsis.Administracion;
using Artsis.Commons;
using Artsis.Gestion.Views;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.DTOs;
using System.Collections.ObjectModel;


namespace Artsis.Gestion.ViewModels
{
    public partial class GestionViewModel : ObservableObject
    {
        private readonly EmpleadoApiService _empleadoApiService;
        private readonly CommonsApiService _commonsApiService;

        private bool _tablaCompleta;

        [ObservableProperty]
        private ObservableCollection<EmpleadoDTO> _items;



        [ObservableProperty]
        private string _nombre;
        [ObservableProperty]
        private string _dni;
        [ObservableProperty]
        private string _areaDescripcion;
        [ObservableProperty]
        private string _apellido;
        [ObservableProperty]
        private string _pass;
        [ObservableProperty]
        private string _nroFila;
        [ObservableProperty]
        private bool _passVisible;
        [ObservableProperty]
        private string _iconoMostrarPass = "ojo.png";

        public GestionViewModel(EmpleadoApiService empleadoApiService, CommonsApiService commonsApiService)
        {
            _empleadoApiService = empleadoApiService;
            _commonsApiService = commonsApiService;

            Items = new ObservableCollection<EmpleadoDTO>();
        }

        [RelayCommand]
        private async Task RegistrarUsuario()
        {

            var popup = new NuevoEmpleado();
            await Application.Current.MainPage.ShowPopupAsync(popup);
            await Buscar();
        }

        [RelayCommand]
        private async Task MostrarPasswords()
        {
            if (PassVisible)
            {
                // Si ya están visibles, las ocultamos.
                PassVisible = false;
                IconoMostrarPass = "ojo.png";
                return;
            }

            // Pedir contraseña del administrador

            var popup = new ValidarPassAdmin();
            var result = await Application.Current.MainPage.ShowPopupAsync(popup);
            if (result != null && result is string pass)
            {
                var correcta = await _commonsApiService.ValidarAdminPass(pass);
                if (correcta) 
                {
                    IconoMostrarPass = "ojos_cruzados.png";
                    PassVisible = true;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Contraseña incorrecta", "OK");
                }
            }

        }


        [RelayCommand]
        private async Task ModificarEmpleado(EmpleadoDTO empleadoModificar)
        {
            var popup = new ModificarEmpleado(empleadoModificar);
            await Application.Current.MainPage.ShowPopupAsync(popup);
            await Buscar();
        }
        [RelayCommand]
        private async Task EliminarEmpleado(string dni)
        {

            try
            {
                bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Deseas eliminar este emplead@?", "Sí", "No");
                if (confirm)
                {
                    await _empleadoApiService.EliminarEmpleado(dni);
                    await Application.Current.MainPage.DisplayAlert("Eliminar Empleado", "Empleado eliminado con éxito", "Ok");
                    await Buscar();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert($"Error al eliminar emplead@", ex.Message, "OK");
            }
        }


        [RelayCommand]
        private async Task Buscar()
        {
            PassVisible = false;
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
            var resultado = await _empleadoApiService.ConsultarEmpleados();

            foreach (var item in resultado)
            {
                item.NroFila = (resultado.IndexOf(item) + 1).ToString();
                Items.Add(item);
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
