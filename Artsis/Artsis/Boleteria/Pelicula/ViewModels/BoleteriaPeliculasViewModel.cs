using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.DTOs;
using Shared.Requests;
using System.Collections.ObjectModel;
using Artsis.Boleteria.Pelicula.Views;
using Artsis.Boleteria.Funcion.Views;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.DTOs;
using Shared.Requests;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Artsis.Boleteria.Pelicula.ViewModels
{
    public partial class BoleteriaPeliculasViewModel : ObservableValidator
    {
        private readonly BoleteriaApiService _boleteriaApiService;
        private bool _tablaCompleta;

        [ObservableProperty]
        private ObservableCollection<PeliculaDTO> _items;


        [ObservableProperty]
        private int _nroPelicula;
        [ObservableProperty]
        private string _titulo;
        [ObservableProperty]
        private int _duracion; //Esta en minutos
        [ObservableProperty]
        private string _genero;
        [ObservableProperty]
        private string _director;

        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private FiltroPeliculasDTO _filtroPeliculasDTO;

        public BoleteriaPeliculasViewModel(BoleteriaApiService boleteriaApiService)
        {
            _boleteriaApiService = boleteriaApiService;
            Items = new ObservableCollection<PeliculaDTO>();
            FiltroPeliculasDTO = new FiltroPeliculasDTO();
        }

        [RelayCommand]
        private async Task Buscar()
        {
            if (_tablaCompleta) return;

            _tablaCompleta = true;
            Items.Clear();

            await CargarItems(FiltroPeliculasDTO);
            _tablaCompleta = false;
        }

        [RelayCommand]
        private async Task CargarMas(FiltroPeliculasDTO filtro)
        {
            if (_tablaCompleta) return;

            _tablaCompleta = true;
            await CargarItems(filtro);
            _tablaCompleta = false;
        }
  

        [RelayCommand]
        private async Task CargarItems(FiltroPeliculasDTO? filtro)
        {
            var resultado = await _boleteriaApiService.ObtenerPeliculasFiltradas(filtro);

            foreach (var item in resultado)
            {
                item.NroPelicula = (resultado.IndexOf(item) + 1).ToString();
                Items.Add(item);
            }
        }

        [RelayCommand]
        private async Task RegistrarPelicula(PeliculaRequest pelicula)
        {
            var popup = new NuevaPelicula(pelicula);
            await Application.Current.MainPage.ShowPopupAsync(popup);
            await Buscar();
        }

        [RelayCommand]
        private async Task ModificarPelicula(PeliculaDTO peliculaModificar)
        {
            var popup = new ModificarPelicula(peliculaModificar);
            await Application.Current.MainPage.ShowPopupAsync(popup);
            await Buscar();
        }

        [RelayCommand]
        private async Task BoleteriaFunciones()
        {

            await Shell.Current.GoToAsync("//Boleteria");
        }

        [RelayCommand]
        private async Task BorrarPelicula(int id)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Deseas borrar la pelicula?", "Sí", "No");

            if (confirm)
            {
                await _boleteriaApiService.BorrarPelicula(id);
                await Application.Current.MainPage.DisplayAlert("Eliminar Pelicula", "Pelicula Eliminada Con Exito", "Ok");
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
