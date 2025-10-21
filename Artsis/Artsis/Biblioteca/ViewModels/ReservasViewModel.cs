using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Entidades;
using Shared.DTOs;
using System.Collections.ObjectModel;

namespace Artsis.Biblioteca.ViewModels
{
    public partial class ReservasViewModel : ObservableValidator
    {
        [ObservableProperty]
        private int _nroFila;
       
        [ObservableProperty]
        private string _nombrePersona;
        [ObservableProperty]
        private string _nombreLibro;
        [ObservableProperty]
        private string _dni;
        [ObservableProperty]
        private string _estadoDescripcion;
        [ObservableProperty]
        private string _fechaInicio;
        [ObservableProperty]
        private string _fechaDevolucion;
        [ObservableProperty]
        private ObservableCollection<dynamic> _items;

        [ObservableProperty]
        private bool _puedeDevolver = false;

        private bool _tablaCompleta;
        private int _numeroPagina = 1;
        private int _cantRegistros = 70;
        private List<ReservaDTO> _reservas;

        private readonly BibliotecaApiService _bibliotecaApiService;


        [ObservableProperty]
        private EstadoReserva _estadoSeleccionado;
        [ObservableProperty]
        private FiltroReservaDTO filtroReserva;
        [ObservableProperty]
        private int _libroId;
        [ObservableProperty]
        private string _nombre;
        [ObservableProperty]
        private string _autor;
        [ObservableProperty]
        private string _genero;
        [ObservableProperty]
        private string _descripcion;
        [ObservableProperty]
        private bool _disponible;
        [ObservableProperty]
        private string _dniAbonado = "";

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private ObservableCollection<EstadoReserva> _estadosReserva;
      

        public ReservasViewModel(BibliotecaApiService bibliotecaApiService)
        {
            _bibliotecaApiService = bibliotecaApiService;
            Items = new ObservableCollection<dynamic>();
            EstadosReserva = new ObservableCollection<EstadoReserva>();
            _estadoSeleccionado = new EstadoReserva();
            ConsultarEstados();

        }
        [RelayCommand]
        private async Task BuscarReserva()
        {

            filtroReserva = new FiltroReservaDTO
            {
                DniAbonado = DniAbonado,
                EstadoReserva = EstadoSeleccionado.Descripcion,
                EstadoReservaId = EstadoSeleccionado.Id
            };
            var reservas = await _bibliotecaApiService.ConsultarReservas(filtroReserva);
            _reservas = reservas;
            Buscar();
        }


        [RelayCommand]
        private async Task Buscar()
        {
            if (_tablaCompleta) return;

            _tablaCompleta = true;
            _numeroPagina = 1;
            Items.Clear();
            await CargarItems(_reservas);
            _tablaCompleta = false;
        }

        [RelayCommand]
        private async Task CargarMas()
        {
            if (_tablaCompleta) return;

            _tablaCompleta = true;
            _numeroPagina++;
            await CargarItems(_reservas);
            _tablaCompleta = false;
        }

        [RelayCommand]
        private async Task CargarItems(List<ReservaDTO> reservas)
        {
            // Calcula el número total de páginas basado en la cantidad de registros
            var totalPaginas = (int)Math.Ceiling((double)reservas.Count / _cantRegistros);

            // Verifica si ya cargaste todos los elementos
            if (_numeroPagina > totalPaginas)
            {
                _tablaCompleta = true;
                return;
            }

            var itemsPagina = reservas
                .Skip((_numeroPagina - 1) * _cantRegistros)
                .Take(_cantRegistros)
                .ToList();

            foreach (var item in itemsPagina)
            {
                item.NroFila = (reservas.IndexOf(item) + 1);

                Items.Add(item);
            }

        }

        [RelayCommand]
        private async Task DevolverLibro(ReservaDTO reserva)
        {
            await _bibliotecaApiService.DevolverLibro(reserva.Id);

            OnPropertyChanged(nameof(Items));
            BuscarReserva();
        }


        [RelayCommand]
        private async Task ConsultarEstados()
        {
            var estados = await _bibliotecaApiService.ObtenerEstados();
            foreach (var estado in estados)
            {
                EstadosReserva.Add(estado);
            }
        }

        [RelayCommand]
        private async Task VerLibros(ReservaDTO reserva)
        {
            var nombresLibros = string.Join("\n", reserva.DetallesReserva.Select(d => d.NombreLibro));

            await Application.Current.MainPage.DisplayAlert("Libros Reservados", nombresLibros, "Cerrar");


        }


        [RelayCommand]
        private async Task Libros()
        {

            await Shell.Current.GoToAsync("//Biblioteca");

        }
        [RelayCommand]
        private async Task CerrarSesion()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Deseas cerrar sesión?", "Sí", "No");

            if (confirm)
            {
                await Shell.Current.GoToAsync("//Login");
            }
        }
    }
}