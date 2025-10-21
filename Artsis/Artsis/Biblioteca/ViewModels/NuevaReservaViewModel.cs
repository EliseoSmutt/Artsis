using Artsis.Models;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.Requests;

namespace Artsis.Biblioteca.ViewModels
{
    public partial class NuevaReservaViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _nombre;
        [ObservableProperty]
        private List<LibroModel> _libros;
        [ObservableProperty]
        private string _dni;
        [ObservableProperty]
        private string _observaciones;


        private readonly Popup _popup;
        private readonly BibliotecaApiService _bibliotecaApiService;

        public NuevaReservaViewModel(Popup popup, BibliotecaApiService bibliotecaApiService, List<LibroModel> libros)
        {
            _popup = popup;
            _bibliotecaApiService = bibliotecaApiService;
            Libros = libros;
        }


        [RelayCommand]
        private async Task GuardarReserva()
        {
            if (string.IsNullOrWhiteSpace(Dni))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe ingresar un DNI.", "OK");
                return;
            }
            var tieneReservaActiva = await _bibliotecaApiService.TieneReservaActiva(Dni);
            if (tieneReservaActiva)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El abonado ya tiene una reserva activa y no puede realizar otra.", "OK");
                return;
            }
            if (Libros == null || !Libros.Any())
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe seleccionar al menos un libro para la reserva.", "OK");
                return;
            }
            var reserva = new ReservaRequest()
            {
                Dni = Dni,
                Observaciones = string.IsNullOrWhiteSpace(Observaciones) ? null : Observaciones,
                LibrosIds = Libros.Select(x => x.Id).ToList()
            };
            await _bibliotecaApiService.RegistrarReserva(reserva);
            await Application.Current.MainPage.DisplayAlert("Registrar Reserva", "Reserva registrado exitosamente!", "OK");
            _popup.Close();
        }
        [RelayCommand]
        private Task Cancelar()
        {
            _popup.Close();
            return Task.CompletedTask;
        }
    }
}
