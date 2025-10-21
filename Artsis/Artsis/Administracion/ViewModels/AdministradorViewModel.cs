using Artsis.Administracion.Views;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.DTOs;
using System.Collections.ObjectModel;


namespace Artsis.Administracion.ViewModels
{
    public partial class AdministradorViewModel : ObservableObject
    {
        private readonly AbonadoApiService _abonadoApiService;
        private bool _tablaCompleta;

        [ObservableProperty]
        private ObservableCollection<dynamic> _items;

        [ObservableProperty]
        private string paseSeleccionado;

        [ObservableProperty]
        private string _nombre;
        [ObservableProperty]
        private string _dni;
        [ObservableProperty]
        private string _apellido;
        [ObservableProperty]
        private string _nroAbonado;
        [ObservableProperty]
        private string _ultimoMesPagado;
        [ObservableProperty]
        private string _fechaDeRegistro;
        [ObservableProperty]
        private string _categoria;
        [ObservableProperty]
        private bool _paseAnual;
        [ObservableProperty]
        private string _nroFila;
        [ObservableProperty]
        private FiltroAbonadosDTO? _filtroAbonadosDTO;

        public AdministradorViewModel(AbonadoApiService abonadoApiService)
        {
            _abonadoApiService = abonadoApiService;
            Items = new ObservableCollection<dynamic>();
            FiltroAbonadosDTO = new FiltroAbonadosDTO();
         
        
        }

        [RelayCommand]
        private async Task RegistrarAbonado()
        {

            var popup =  new NuevoAbonado();
            await Application.Current.MainPage.ShowPopupAsync(popup);
            await Buscar();
        }

        [RelayCommand]
        private async Task ModificarAbonado(AbonadoDTO abonadoModificar)
        {
            var popup = new ModificarAbonado(abonadoModificar);
            await Application.Current.MainPage.ShowPopupAsync(popup);
            await Buscar();
        }

        [RelayCommand]
        private async Task Buscar()
        {
            if (_tablaCompleta) return;

            _tablaCompleta = true;
            Items.Clear();
            await CargarItems(FiltroAbonadosDTO);
            _tablaCompleta = false;
        }

        [RelayCommand]
        private async Task CargarMas()
        {
            if (_tablaCompleta) return;

            _tablaCompleta = true;
            await CargarItems(FiltroAbonadosDTO);
            _tablaCompleta = false;
        }

        [RelayCommand]
        private async Task CargarItems(FiltroAbonadosDTO? filtro)
        {
            var resultado = await _abonadoApiService.ConsultarAbonados(filtro);

            foreach (var item in resultado)
            {
                item.NroFila = (resultado.IndexOf(item) + 1).ToString();
                Items.Add(item);
            }
        }

        [RelayCommand]
        private async Task BorrarAbonado(string dni)
        {

            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Deseas eliminar este abonado?", "Sí", "No");
            if (confirm)
            {
                await _abonadoApiService.BorrarAbonado(dni);
                await Application.Current.MainPage.DisplayAlert("Eliminar Abonado", "Abonado Eliminado Con Exito", "Ok");
                await Buscar();
            }
        }

        partial void OnPaseSeleccionadoChanged(string value)
        {
            switch (value)
            {
                case "Ambos":
                    FiltroAbonadosDTO.PaseAnual = null;
                    break;
                case "Si":
                    FiltroAbonadosDTO.PaseAnual = true;
                    break;
                case "No":
                    FiltroAbonadosDTO.PaseAnual = false;
                    break;
            }
        }

            [RelayCommand]
        private async Task GoToMetricas()
        {

            await Shell.Current.GoToAsync("//Metricas");
        }
        //-------------------------------------------------------------------------------------------------------------------
        [RelayCommand]
        private async Task ActualizarUltimoPago(string nroAbonado)
        {
            // Confirmación del usuario
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirmación",
                "¿Deseas actualizar el último pago al momento actual?",
                "Sí",
                "No"
            );

            if (confirm)
            {
                // Llamada al servicio para actualizar el último pago
                await _abonadoApiService.ActualizarUltimoPago(nroAbonado);

                // Mostrar un mensaje de éxito
                await Application.Current.MainPage.DisplayAlert(
                    "Actualizar Último Pago",
                    "El último pago ha sido actualizado exitosamente.",
                    "Ok"
                );
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
