using Artsis.Biblioteca.Views;
using Artsis.Models;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Entidades;
using Shared.Requests;
using System.Collections.ObjectModel;

namespace Artsis.Biblioteca.ViewModels
{
    public partial class BibliotecaViewModel : ObservableValidator
    {
        private readonly BibliotecaApiService _bibliotecaApiService;
        private bool _tablaCompleta;

        [ObservableProperty]
        private ObservableCollection<LibroModel> _items;

        private List<LibroModel> _allItems;

        [ObservableProperty]
        private EstadoReserva _estadoSeleccionado;
        [ObservableProperty]
        private string _estadoDescripcion;
        [ObservableProperty]
        private string _filtroLibros;
        [ObservableProperty]
        private int _libroId;
        [ObservableProperty]
        private string _nombre;
        [ObservableProperty]
        private string _autor;
        [ObservableProperty]
        private string _nroFila;
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

        public BibliotecaViewModel(BibliotecaApiService bibliotecaApiService)
        {
            _bibliotecaApiService = bibliotecaApiService;
            Items = new ObservableCollection<LibroModel>();
            EstadosReserva = new ObservableCollection<EstadoReserva>();
            CargarItems();
        }


        [RelayCommand]
        private async Task RegistrarReserva()
        {
            var librosSeleccionados = Items
            .Where(item => item.IsSelected)
            .Select(item => item)
            .ToList();

            if (librosSeleccionados.Count > 4)
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", "No se pueden prestar mas de 4 libros a la vez", "OK");
                return;
            }

            if (!librosSeleccionados.Any())
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", "No se ha seleccionado ningún libro.", "OK");
                return;
            }


            var popup = new NuevaReserva(librosSeleccionados);
            await Application.Current.MainPage.ShowPopupAsync(popup);
            await CargarItems();
        }

        [RelayCommand]
        private async Task Restablecer()
        {
            FiltroLibros = string.Empty;

            var librosSeleccionados = Items.Where(item => item.IsSelected).ToList();

            foreach (var libro in librosSeleccionados)
            {
                libro.IsSelected = false;
            }
            await CargarItems();
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
            var resultado = await _bibliotecaApiService.ConsultarLibros();

            _allItems = resultado.Select((libroDto, index) => new LibroModel(libroDto)
            {
                NroFila = (index + 1).ToString(),
                IsSelected = false
            }).ToList();

            Items = new ObservableCollection<LibroModel>(_allItems);

        }
        partial void OnFiltroLibrosChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // Si el texto de búsqueda está vacío, mostramos todos los elementos
                Items = new ObservableCollection<LibroModel>(_allItems);
            }
            else
            {
                // Filtramos los elementos
                var librosFiltrados = _allItems.Where(item =>
                    item.Nombre.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    item.Autor.Contains(value, StringComparison.OrdinalIgnoreCase)).ToList();

                Items = new ObservableCollection<LibroModel>(librosFiltrados);
            }
        }




        [RelayCommand]
        private async Task Reservas()
        {
            await Shell.Current.GoToAsync("//Reservas");
        }

        [RelayCommand]
        private async Task RegistrarLibro()
        {
            var popup = new NuevoLibro();
            await Application.Current.MainPage.ShowPopupAsync(popup);
            await CargarItems();
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