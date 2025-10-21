using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Entidades;
using Shared.DTOs;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Security.Cryptography;

namespace Artsis.Boleteria.Pelicula.ViewModels
{
    public partial class ModificarPeliculaViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _director;
        [ObservableProperty]
        private int _genero_PeliculasId;

        [ObservableProperty]
        private string _titulo;

        [ObservableProperty]
        private ObservableCollection<Genero_Pelicula> _generos;
        [ObservableProperty]
        private int _duracion;
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private string _nombreGenero;
        [ObservableProperty]
        private string _descripcion;
        [ObservableProperty]
        private Genero_Pelicula _genero_Seleccionado;

        private readonly Popup _popup;
        private readonly BoleteriaApiService _boleteriaApiService;
        public ModificarPeliculaViewModel(Popup popup, BoleteriaApiService boleteriaApiService, PeliculaDTO peliculaActualizar)
        {
            _popup = popup;
            _boleteriaApiService = boleteriaApiService;

            Generos = new ObservableCollection<Genero_Pelicula>();


            CargarCamposYAsignarlos(peliculaActualizar);


            Id = peliculaActualizar.Id;
            Titulo = peliculaActualizar.Titulo;
            Director = peliculaActualizar.Director;
            Duracion = peliculaActualizar.Duracion;
            Genero_PeliculasId = peliculaActualizar.Genero_PeliculasId;
        }

        [RelayCommand]
        private async Task ModificarPelicula()
        {
            if (string.IsNullOrWhiteSpace(Titulo))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El título no puede estar vacío.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Director))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El director no puede estar vacío.", "OK");
                return;
            }

            if (!int.TryParse(Duracion.ToString(), out _) || Duracion <= 0 || Duracion.ToString().Length > 4)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "La duración debe ser un número positivo de hasta 4 dígitos.", "OK");
                return;
            }

            if (Genero_Seleccionado == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe seleccionar un género para la película.", "OK");
                return;
            }

            var pelicula = new PeliculaDTO
            {
                Id = Id,
                Titulo = Titulo,
                Duracion = Duracion,
                Director = Director,
                Genero_PeliculasId = Genero_Seleccionado.Id,
            };
            try
            {
                await _boleteriaApiService.ModificarPelicula(pelicula);
                await Application.Current.MainPage.DisplayAlert("Modificar datos de la Pelicula", "Los datos se actualizaron correctamente", "OK");
                _popup.Close();
            }
            catch (Exception ex) when (ex.Message.Contains("El título ingresado ya está registrado"))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El título ingresado ya pertenece a una película registrada. Por favor, verifique los datos e intente nuevamente.", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Ocurrió un error inesperado. Por favor, intente nuevamente más tarde.", "OK");
                Console.WriteLine($"Error: {ex.Message}");
            }
        } 
private async Task CargarCamposYAsignarlos(PeliculaDTO peliculaActualizar)
        {
           
           await ObtenerNombresGeneros();

            Genero_Seleccionado = Generos.FirstOrDefault(p => p.Descripcion == peliculaActualizar.Genero);
            
        }

        private async Task ObtenerNombresGeneros()
        {
            var generos = await _boleteriaApiService.ObtenerGeneros();

            foreach (var genero in generos)
            {
                Generos.Add(genero);
            }
        }

        [RelayCommand]
        private Task Cancelar()
        {
            _popup.Close();
            return Task.CompletedTask;
        }
    }
}
